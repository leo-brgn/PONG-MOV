using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using HRT_Time = System.Int64;

[RequireComponent(typeof(RTDESKEntity))]
public class BallMovement : MonoBehaviour
{

    float BallSpeed = 5f;
    public float BallDirectionX;
    public float BallDirectionY;
    bool isMoving; // = false;
    
    // RTDESK
    private string myName;
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

    private void Launch()
    {
        if (isMoving)
        {
            //GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            // Becomes:
            Velocity velMsg = (Velocity)Engine.PopMsg((int)UserMsgTypes.Speed);
            velMsg.V2 = new Vector2(0, 0);
            Engine.SendMsg(velMsg, gameObject, ReceiveMessage, tenMillis);
            //GetComponent<Rigidbody2D>().transform.position = new Vector3(0, -1, 1);
            Transform posMsg = (Transform) Engine.PopMsg((int)UserMsgTypes.Position);
            posMsg.V3 = new Vector3(0, -1, 1);
            Engine.SendMsg(posMsg, gameObject, ReceiveMessage, tenMillis);
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
                    //GetComponent<Rigidbody2D>().velocity = new Vector2(BallDirectionX, BallDirectionY) * BallSpeed;
                    // Becomes:
                    Velocity velMsg = (Velocity)Engine.PopMsg((int)UserMsgTypes.Speed);
                    velMsg.V2 = new Vector2(BallDirectionX, BallDirectionY) * BallSpeed; 
                    Engine.SendMsg(velMsg, gameObject, ReceiveMessage, tenMillis);
                    notWorking = false;
                }
            }

            isMoving = true;
        }
    }
    
    void ReceiveMessage(MsgContent Msg)
    {
        Velocity v;
        Transform t;
        switch (Msg.Type)
        {
            case (int)RTDESKMsgTypes.Input:
                RTDESKInputMsg IMsg = (RTDESKInputMsg)Msg;
                switch (IMsg.c)
                {
                    case KeyCode.Return:
                        if (KeyState.DOWN == IMsg.s)
                        {
                            Launch();
                        }
                        break;
                }
                Engine.PushMsg(Msg);
                break;
            case (int)UserMsgTypes.Speed:
                if (myName == Msg.Sender.name)
                {
                    v = (Velocity)Msg;
                    GetComponent<Rigidbody2D>().velocity = v.V2;
                }
                Engine.PushMsg(Msg);
                break;
            case (int)UserMsgTypes.Position:
                if (myName == Msg.Sender.name)
                {
                    t = (Transform)Msg;
                    GetComponent<Rigidbody2D>().transform.position = t.V3;
                }
                Engine.PushMsg(Msg);
                break;
        }
    }
}
