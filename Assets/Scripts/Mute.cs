using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mute : MonoBehaviour
{
    public AudioSource GameMusic;
    public Text muteButton;


    public void ToggleMute()
    {
        if (GameMusic.isPlaying)
        {
            muteButton.text = "Unmute";
            GameMusic.Pause();
        }

        else if (!GameMusic.isPlaying)
        {
            muteButton.text = "Mute";
            GameMusic.UnPause();
        }
    }
}
