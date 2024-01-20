using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverviewPanel : MonoBehaviour
{
    [SerializeField] private Text _player1Text = null;
    [SerializeField] private Text _player2Text = null;
    private Dictionary<PlayerRef, string> _playerNickNames = new Dictionary<PlayerRef, string>();
    private Dictionary<PlayerRef, int> _playerScores = new Dictionary<PlayerRef, int>();
    
    public void AddEntry(PlayerRef playerRef, PlayerDataNetworked playerDataNetworked)
    {
        if (_playerNickNames.Count > 1) return;
        if (_playerScores.Count > 1) return;
        if (playerDataNetworked == null) return;
        
        string nickName = String.Empty;
        int score = 0;
        
        _playerNickNames.Add(playerRef, nickName);
        _playerScores.Add(playerRef, score);
        
        UpdateEntry(playerRef);
    }
    
    public void RemoveEntry(PlayerRef playerRef)
    {
        _playerNickNames.Remove(playerRef);
        _playerScores.Remove(playerRef);
    }
    
    public void UpdateEntry(PlayerRef playerRef)
    {
        if (_playerNickNames.TryGetValue(playerRef, out var nickName) == false) return;
        if (_playerScores.TryGetValue(playerRef, out var score) == false) return;
        
        if (playerRef.PlayerId == 1)
        {
            // We want text: 
            // Player 1: <nickName> 
            // Score: <score>
            _player1Text.text = $"{nickName}\nScore: {score}";
        }
        else if (playerRef.PlayerId == 2)
        {
            // We want text: 
            // Player 2: <nickName> 
            // Score: <score>
            _player2Text.text = $"{nickName}\nScore: {score}";
        }
    }
    
    public void UpdateScore(PlayerRef player, int score)
    {
        if (_playerScores.TryGetValue(player, out var scoreValue) == false) return;
        
        _playerScores[player] = score;
        UpdateEntry(player);
    }
    
    public void UpdateNickName(PlayerRef player, string nickName)
    {
        if (_playerNickNames.TryGetValue(player, out var nickNameValue) == false) return;
        
        _playerNickNames[player] = nickName;
        UpdateEntry(player);
    }
   
}