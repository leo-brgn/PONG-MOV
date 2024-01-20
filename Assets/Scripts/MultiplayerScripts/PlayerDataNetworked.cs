using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerDataNetworked : NetworkBehaviour
{

    private ChangeDetector _changeDetector;
    
    private PlayerOverviewPanel _overviewPanel = null;
    
    [HideInInspector]
    [Networked]
    public NetworkString<_16> NickName { get; private set; }
    
    [HideInInspector]
    [Networked]
    public int Score { get; private set; }
    
    public override void Spawned()
    {
        // --- Client
        // Find the local non-networked PlayerData to read the data and communicate it to the Host via a single RPC 
        if (Object.HasInputAuthority)
        {
            var nickName = FindObjectOfType<PlayerData>().GetNickName();
            RpcSetNickName(nickName);
        }

        // --- Host
        // Initialized game specific settings
        if (Object.HasStateAuthority)
        {
            Score = 0;
        }
        
        _overviewPanel = FindObjectOfType<PlayerOverviewPanel>();
        // Add an entry to the local Overview panel with the information of this spaceship
        _overviewPanel.AddEntry(Object.InputAuthority, this);
        
        _overviewPanel.UpdateNickName(Object.InputAuthority, NickName.ToString());
        _overviewPanel.UpdateScore(Object.InputAuthority, Score);
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }
    
    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            switch (change)
            {
                case nameof(NickName):
                    _overviewPanel.UpdateNickName(Object.InputAuthority, NickName.ToString());
                    break;
                case nameof(Score):
                    _overviewPanel.UpdateScore(Object.InputAuthority, Score);
                    break;
            }
        }
    }
    
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    private void RpcSetNickName(string nickName)
    {
        if (string.IsNullOrEmpty(nickName)) return;
        NickName = nickName;
    }
    
    public void AddScore(int score)
    {
        Score += score;
        _overviewPanel.UpdateScore(Object.InputAuthority, Score);
    }
}
