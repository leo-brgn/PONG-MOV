
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

[RequireComponent(typeof(NetworkRigidbody2D))]
public class NetworkBallMovement : NetworkBehaviour
{
    float BallSpeed = 5f;
    public float BallDirectionX;
    public float BallDirectionY;
    bool isMoving; // = false;
    public GameStateController _gameStateController = null;
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Log.Debug("Collision detected");
        if (collision.gameObject.name == "Wall Left")
        {
            Launch();
            _gameStateController.ChangeScore(1);
        }
        else if (collision.gameObject.name == "Wall Right")
        {
            Launch();
            _gameStateController.ChangeScore(0);
        }
    }
    
    public void Launch()
    {
        if (_gameStateController == null)
        {
            Log.Error("HO");
        }
        
        if (isMoving)
        {
            Vector2 speed = new Vector2(0, 0);
            Vector3 position = new Vector3(0, -1, 1);
            GetComponent<Rigidbody2D>().velocity = speed;
            // GetComponent<Rigidbody2D>().transform.position = position;
            // Use Fusion teleport instead of Unity transform
            GetComponent<NetworkRigidbody2D>().Teleport(position);
            isMoving = false;
        }
        else
        {
            bool notWorking = true;
            while (notWorking)
            {
                BallDirectionX = Random.Range(-1f, -0.5f);
                BallDirectionY = Random.Range(-1f, -0.5f);
                
                if (BallDirectionY != 0 && BallDirectionX != 0)
                {
                    Vector2 speed = new Vector2(BallDirectionX, BallDirectionY) * BallSpeed;
                    GetComponent<Rigidbody2D>().velocity = speed;
                    isMoving = true;
                    notWorking = false;
                }
            }
        }
    }
}