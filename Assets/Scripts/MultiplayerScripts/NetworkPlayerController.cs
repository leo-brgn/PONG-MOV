
using Fusion;
using UnityEngine;

public class NetworkPlayerController : NetworkBehaviour
{
    
    private ChangeDetector _changeDetector;
    private Rigidbody _rigidbody = null;
    private PlayerDataNetworked _playerDataNetworked = null;
    
    public bool AcceptInput => _isAlive && Object.IsValid;
    
    [Networked] private NetworkBool _isAlive { get; set; }
    
    public override void Spawned()
    {
        // --- Host & Client
        // Set the local runtime references.
        _rigidbody = GetComponent<Rigidbody>();
        _playerDataNetworked = GetComponent<PlayerDataNetworked>();
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        
        // --- Host
        // The Game Session SPECIFIC settings are initialized
        if (Object.HasStateAuthority == false) return;
        // We need to check if all the players are ready before we can start the game.
        _isAlive = true;
    }
}