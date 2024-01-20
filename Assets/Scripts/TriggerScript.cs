using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TriggerScript : MonoBehaviour
{

    public int player1 = 0;
    public int player2 = 0;

    public Text Player1Score;
    public Text Player2Score;
    
    // RTDESK
    [SerializeField]
    RTDESKEngine Engine;

    public void Start()
    {
        Player1Score.text = $"PLAYER 1 SCORE: {player1.ToString()}";
        Player2Score.text = $"PLAYER 2 SCORE: {player2.ToString()}";
        
        // RTDESK
        RTDESKEntity rtdeskEntity = GetComponent<RTDESKEntity>();
        Engine = rtdeskEntity.RTDESKEngineScript;
        RTDESKInputManager IM = rtdeskEntity.GetRTDESKInputManager();
        GetComponent<RTDESKEntity>().MailBox = ReceiveMessage;
        IM.RegisterKeyCode(ReceiveMessage, KeyCode.Escape);
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Wall Left")
        {

            ResetBallPositionAfterGoal();

            player2++;
            Player2Score.text = $"PLAYER 2 SCORE: {player2.ToString()}";
            PlayerWins();

        }

        else if (collision.gameObject.name == "Wall Right")
        {

            ResetBallPositionAfterGoal();

            player1++;
            Player1Score.text = $"PLAYER 1 SCORE: {player1.ToString()}";
            PlayerWins();
        }
    }

    public void ResetBallPositionAfterGoal()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().transform.position = new Vector3(0, -1, 1);
    }

    public void PlayerWins()
    {
        if (player1 >= 10 || player2 >= 10)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    void ReceiveMessage(MsgContent Msg)
    {
        switch (Msg.Type)
        {
            case (int)RTDESKMsgTypes.Input:
                RTDESKInputMsg IMsg = (RTDESKInputMsg)Msg;
                switch (IMsg.c)
                {
                    case KeyCode.Escape:
                        if (IMsg.s == KeyState.DOWN)
                        {
                            SceneManager.LoadScene("MainMenu");
                        }
                        break;
                    break;
                }
                Engine.PushMsg(Msg);
                break;
        }
    }

}
