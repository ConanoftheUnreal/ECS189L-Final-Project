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
        SceneManager.LoadScene("PreRun");
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            GotoMain();
        }
    }
}
