using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
        SceneManager.LoadScene("PreRun");
    }

    public void QuitGame()
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Good Select");
        Debug.Log("QUIT");
        Application.Quit();
    }
}
