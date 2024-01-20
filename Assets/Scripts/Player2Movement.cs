using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRT_Time = System.Int64;

public class Player2Movement : MonoBehaviour
{
    // RTDESK   
    [SerializeField]
    RTDESKEngine Engine;

    [SerializeField] float playerSpeed = 0.3f;
    void Start()
    {
        RTDESKEntity rtdeskEntity = GetComponent<RTDESKEntity>();
        Engine = rtdeskEntity.RTDESKEngineScript;
        RTDESKInputManager IM = rtdeskEntity.GetRTDESKInputManager();
        GetComponent<RTDESKEntity>().MailBox = ReceiveMessage;
        IM.RegisterKeyCode(ReceiveMessage, KeyCode.Return);
    }

    private void Move(int v)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, v) * playerSpeed;
    }

    void ReceiveMessage(MsgContent Msg)
    {
        switch (Msg.Type)
        {
            case (int)RTDESKMsgTypes.Input:
                RTDESKInputMsg IMsg = (RTDESKInputMsg)Msg;
                switch (IMsg.c)
                {
                    case KeyCode.UpArrow:
                        if (IMsg.s == KeyState.DOWN)
                        {
                            Move(1);
                        }
                        break;
                    case KeyCode.DownArrow:
                        if (IMsg.s == KeyState.DOWN)
                        {
                            Move(-1);
                        }
                        break;
                }
                Engine.PushMsg(Msg);
                break;
        }
    }
}
