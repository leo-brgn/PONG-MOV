using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateController : NetworkBehaviour
    {
        enum GameState
        {
            Starting,
            Waiting, // Waiting for 2 players
            Countdown, // 3, 2, 1, GO!
            Running,
            Ending
        }
        [SerializeField] private NetworkPrefabRef _ballPrefab = NetworkPrefabRef.Empty;
        private NetworkObject _ballObject = null;
        
        [SerializeField] private Text _startEndDisplay = null;
        
        [Networked] private GameState _gameState { get; set; }

        [Networked] private NetworkBehaviourId _winner { get; set; }
        [Networked] private TickTimer _countdownTimer { get; set; }

        private List<NetworkBehaviourId> _playerDataNetworkedIds = new List<NetworkBehaviourId>();

        public override void Spawned()
        {
            _startEndDisplay.gameObject.SetActive(false);
            
            if (_gameState != GameState.Starting)
            {
                foreach (var player in Runner.ActivePlayers)
                {
                    if (Runner.TryGetPlayerObject(player, out var playerObject) == false) continue;
                    TrackNewPlayer(playerObject.GetComponent<PlayerDataNetworked>().Id);
                }
            }
            
            Runner.SetIsSimulated(Object, true);
            
            if (Object.HasStateAuthority == false) return;
            
            _gameState = GameState.Starting;
        }

        public override void FixedUpdateNetwork()
        {
            switch (_gameState)
            {
                case GameState.Starting:
                    UpdateStartingDisplay();
                    break;
                case GameState.Waiting:
                    UpdateWaitingDisplay();
                    break;
                case GameState.Countdown:
                    UpdateCountdownDisplay();
                    break;
                case GameState.Running:
                    UpdateRunningDisplay();
                    break;
                case GameState.Ending:
                    UpdateEndingDisplay();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void UpdateStartingDisplay()
        {
            
            if (Object.HasStateAuthority == false) return;
            
            FindObjectOfType<BasicSpawner>().StartSpawner(this);
            // Spawn ball as network object
            if (_ballObject == null)
            {
                Vector3 position = new Vector3(0, -1f, 0);
                _ballObject = Runner.Spawn(_ballPrefab, position, Quaternion.identity);
                _ballObject.GetComponent<NetworkBallMovement>()._gameStateController = this;
            };

            _gameState = GameState.Waiting;
        }
        
        public void UpdateWaitingDisplay()
        {
            _startEndDisplay.gameObject.SetActive(true);
            _startEndDisplay.text = "Waiting for players...";
            
            if (Object.HasStateAuthority == false) return;
            if (_playerDataNetworkedIds.Count < 2) return;
            
            _gameState = GameState.Countdown;
            _countdownTimer = TickTimer.CreateFromSeconds(Runner, 3f);
        }
        
        public void UpdateCountdownDisplay()
        {
            _startEndDisplay.gameObject.SetActive(true);
            _startEndDisplay.text = $"{Mathf.RoundToInt(_countdownTimer.RemainingTime(Runner) ?? 00)}";
            
            if (Object.HasStateAuthority == false) return;
            if (_countdownTimer.RemainingTime(Runner) > 0) return;
            
            _gameState = GameState.Running;
       
            var ballMovement = _ballObject.GetComponent<NetworkBallMovement>();
            ballMovement.Launch();
        }
        
        public void UpdateRunningDisplay()
        {
            _startEndDisplay.gameObject.SetActive(false);
            
        }
        
        public void UpdateEndingDisplay()
        {
            if (Runner.TryFindBehaviour(_winner, out PlayerDataNetworked playerData) == false) return;

            Runner.Shutdown();
        }
        public void ChangeScore(int player)
        {
            Log.Debug("ChangeScore");
            if (Object.HasStateAuthority == false) return;
            Log.Debug("ChangeScore");

            var playerId = _playerDataNetworkedIds[player];
            if (Runner.TryFindBehaviour((playerId), out PlayerDataNetworked playerData) == false)
            {
                Log.Debug("NOT FOUND ?");
            };
            playerData.AddScore(1);

            _gameState = GameState.Countdown;
            _countdownTimer = TickTimer.CreateFromSeconds(Runner, 3f);
            CheckForEnd(playerData.Score, playerId);
        }
        
        public void CheckForEnd(int score, NetworkBehaviourId playerId)
        {
            if (score >= 5)
            {
                _winner = playerId;
                _gameState = GameState.Ending;
            }
        }
        
        public void TrackNewPlayer(NetworkBehaviourId playerDataNetworkedId)
        {
            _playerDataNetworkedIds.Add(playerDataNetworkedId);
        }
    }
