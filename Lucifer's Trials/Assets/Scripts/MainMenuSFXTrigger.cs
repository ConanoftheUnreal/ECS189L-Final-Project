using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSFXTrigger : MonoBehaviour
{
    public void PlaySelectSound()
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
    }
}
