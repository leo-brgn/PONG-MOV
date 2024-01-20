using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float _movementSpeed = 0.3f;
    
    private Rigidbody2D _rigidbody = null;
    private NetworkPlayerController _networkPlayerController = null;
    
    public override void Spawned()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _networkPlayerController = GetComponent<NetworkPlayerController>();
        
    }
    
    public override void FixedUpdateNetwork()
    {
        if (_networkPlayerController.AcceptInput == false) return;
        
        if (Runner.TryGetInputForPlayer<NetworkPlayerInput>(Object.InputAuthority, out var input))
        {
            Move(input);
        }
    }
    
    private void Move(NetworkPlayerInput input)
    {
        _rigidbody.velocity = new Vector2(0.0f, input.VerticalInput * _movementSpeed);
    }
}
