using UnityEngine;
using Fusion;


public class BasicSpawner : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef _playerPrefab = NetworkPrefabRef.Empty;
    
    private bool _gameIsReady = false;
    private GameStateController _gameStateController = null;

    private SpawnPoint[] _spawnPoints = null;

    public override void Spawned()
    {
        if (Object.HasStateAuthority == false) return;
        _spawnPoints = FindObjectsOfType<SpawnPoint>();
    }
    
    public void StartSpawner(GameStateController gameStateController)
    {
        _gameIsReady = true;
        _gameStateController = gameStateController;
        foreach (var player in Runner.ActivePlayers)
        {
            SpawnPlayer(player);
        }
    }
    
    private void SpawnPlayer(PlayerRef player)
    {
        // Modulo is used in case there are more players than spawn points.
        int index = player.PlayerId - 1; 
        var spawnPosition = _spawnPoints[index].transform.position;

        var playerObject = Runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        // Set Player Object to facilitate access across systems.
        Runner.SetPlayerObject(player, playerObject);

        _gameStateController.TrackNewPlayer(playerObject.GetComponent<PlayerDataNetworked>().Id);
    }
    
    public void PlayerJoined(PlayerRef player)
    {
        if (_gameIsReady == false) return;
        SpawnPlayer(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        DespawnPlayer(player);
    }
    
    private void DespawnPlayer(PlayerRef player)
    {
        if (Runner.TryGetPlayerObject(player, out var playerNetworkObject))
        {
            Runner.Despawn(playerNetworkObject);
        }
        
        Runner.SetPlayerObject(player, null);
    }
}
