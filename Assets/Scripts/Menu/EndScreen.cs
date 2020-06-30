using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndScreen : MonoBehaviour
{
    public AudioSource buttonSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioClip ouaisSound;
    public TextMeshProUGUI time;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        float currentTime = Timer.instance.getTime();
        time.text = ((int)currentTime / 60).ToString("00") + ":" + (currentTime % 60).ToString("00");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void HoverSound()
    {
        buttonSource.clip = hoverSound;
        buttonSource.Play();
    }

    public void ClickSound()
    {
        buttonSource.clip = clickSound;
        buttonSource.Play();
    }

    public void OuaisSound()
    {
        buttonSource.clip = ouaisSound;
        buttonSource.pitch = Random.Range(0.3f, 2f);
        buttonSource.Play();
    }
}

