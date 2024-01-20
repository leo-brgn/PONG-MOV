using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HRT_Time = System.Int64;

public class StartMessage : MonoBehaviour
{

    public Text startText;
    
    [SerializeField]
    RTDESKEngine Engine;


    public void Start()
    {
        RTDESKEntity rtdeskEntity = GetComponent<RTDESKEntity>();
        Engine = rtdeskEntity.RTDESKEngineScript;
        RTDESKInputManager IM = rtdeskEntity.GetRTDESKInputManager();
        GetComponent<RTDESKEntity>().MailBox = ReceiveMessage;
        IM.RegisterKeyCode(ReceiveMessage, KeyCode.Return);
        
        StartBlinking();
    }
    public IEnumerator Blink()
    {
        while (true)
        {
            startText.color = Color.green;
            yield return new WaitForSeconds(0.1f);
            startText.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            startText.color = Color.blue;
            yield return new WaitForSeconds(0.1f);
            startText.color = Color.magenta;
            yield return new WaitForSeconds(0.1f);
            startText.color = Color.cyan;
            yield return new WaitForSeconds(0.1f);
            startText.color = Color.yellow;
            yield return new WaitForSeconds(0.1f);

        }
    }

    void StartBlinking()
    {
        StartCoroutine("Blink");
    }
    void StopBlinking()
    {
        StopCoroutine("Blink");
    }

    void ReceiveMessage(MsgContent Msg)
    {
        switch (Msg.Type)
        {
            case  (int)RTDESKMsgTypes.Input:
                RTDESKInputMsg IMsg = (RTDESKInputMsg)Msg;
                switch (IMsg.c)
                {
                    case KeyCode.Escape:
                        if (IMsg.s == KeyState.UP)
                        {
                            startText.text = "";
                            StopBlinking();
                        }
                        break;
                }
                Engine.PushMsg(Msg);
                break;
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
