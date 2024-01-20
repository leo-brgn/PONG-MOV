using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameScript : MonoBehaviour
{
    private void Awake()
    {
        var nickNameInputField = GetComponentInChildren<InputField>();
        nickNameInputField.text = PlayerData.GetRandomNickName();
    }
}
