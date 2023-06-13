using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Game Over");
    }

    public void GotoMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
