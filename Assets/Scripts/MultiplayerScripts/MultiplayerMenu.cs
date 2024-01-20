using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerMenu : MonoBehaviour
{
    [SerializeField] private NetworkRunner _networkRunnerPrefab = null;
    [SerializeField] private PlayerData _playerDataPrefab = null;
    
    [SerializeField] private InputField _nickName = null;
    
    [SerializeField] private string _gameSceneName = null;
    [SerializeField] private InputField _roomName = null;
    
    private NetworkRunner _runnerInstance = null;

    public void StartHost()
    {
        SetPlayerData();
        StartGame(GameMode.AutoHostOrClient, _roomName.text, _gameSceneName);
    }
    
    public void StartClient()
    {
        SetPlayerData();
        StartGame(GameMode.Client, _roomName.text, _gameSceneName);
    }
    
    private void SetPlayerData()
    {
        var playerData = FindObjectOfType<PlayerData>();
        if (playerData == null)
        {
            playerData = Instantiate(_playerDataPrefab);
        }
        
        if (string.IsNullOrWhiteSpace(_nickName.text))
        {
            playerData.SetNickName(PlayerData.GetRandomNickName());
        }
        else
        {
            playerData.SetNickName(_nickName.text);
        }
    }
    
    private async void StartGame(GameMode mode, string roomName, string sceneName)
    {
        _runnerInstance = FindObjectOfType<NetworkRunner>();
        if (_runnerInstance == null)
        {
            _runnerInstance = Instantiate(_networkRunnerPrefab);
        }

        // Let the Fusion Runner know that we will be providing user input
        _runnerInstance.ProvideInput = true;

        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            ObjectProvider = _runnerInstance.GetComponent<NetworkObjectPoolDefault>(),
        };

        // GameMode.Host = Start a session with a specific name
        // GameMode.Client = Join a session with a specific name
        await _runnerInstance.StartGame(startGameArgs);

        if (_runnerInstance.IsServer)
        {
            _runnerInstance.LoadScene(sceneName);
        }
    }
}
