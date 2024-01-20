using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRT_Time = System.Int64;

public class AI : MonoBehaviour
{

    public float speed = 10;
    public GameObject ball;
    
    // RTDESK
    private string myName;
    HRT_Time userTime;
    HRT_Time tenMillis;
    [SerializeField]
    RTDESKEngine Engine;
    
    public void Start()
    {
        RTDESKEntity rtdeskEntity = GetComponent<RTDESKEntity>();
        Engine = rtdeskEntity.RTDESKEngineScript;
        RTDESKInputManager IM = rtdeskEntity.GetRTDESKInputManager();
        GetComponent<RTDESKEntity>().MailBox = ReceiveMessage;
        IM.RegisterKeyCode(ReceiveMessage, KeyCode.Return);
        
        myName = gameObject.name;
        
        tenMillis = Engine.ms2Ticks(10);
    }

    private void Move()
    {
        var moveDistance = speed * Time.deltaTime;
        var ballDistance = Mathf.Abs(transform.position.y - ball.transform.position.y);


        if (moveDistance > ballDistance)
        {
            moveDistance = ballDistance / 2;

        }

        if (ball.transform.position.y > transform.position.y)
        {
            transform.Translate(Vector2.up * moveDistance);
        }
        else if (ball.transform.position.y < transform.position.y)
        {
            transform.Translate(-(Vector2.up * moveDistance));
        }
    }

    void ReceiveMessage(MsgContent Msg)
    {
        switch (Msg.Type)
        {
            case (int)RTDESKMsgTypes.Input: 
                RTDESKInputMsg inputMsg = (RTDESKInputMsg)Msg;
                switch (inputMsg.c)
                {
                    case KeyCode.Return:
                        if (KeyState.DOWN == inputMsg.s)
                        {
                            // Create Move action
                            Action move = (Action) Engine.PopMsg((int)UserMsgTypes.Action);
                            move.action = (int)UserActions.Move;
                            Engine.SendMsg(move, gameObject, ReceiveMessage, tenMillis);
                        }
                        Engine.PushMsg(Msg);
                        break;
                }
                break;
            case (int)UserMsgTypes.Action:
                Action a = (Action)Msg;
                switch (a.action)
                {
                    case (int) UserActions.Move:
                        if (myName == Msg.Sender.name)
                        {
                            Move();
                            Engine.SendMsg(Msg, tenMillis);
                        }
                        break;
                }
                break;
                
        }
    }

}
