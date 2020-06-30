using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseUI;
    private GameObject player;
    private PlayerController playerController;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        player = playerController.gameObject;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }
            else Pause();
            playerController.isSlowMotion = false;   
        }
    }

    public void Resume()
    {
        Debug.Log("Resume");
        player.GetComponent<PlayerShoot>().enabled = true;
        player.GetComponent<PlayerController>().enabled = true;
        pauseUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        isPaused = false;
        AudioManager.instance.UnPauseAll();
    }
    public void Pause()
    {
        Debug.Log("Pause");
        player.GetComponent<PlayerShoot>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
        pauseUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        isPaused = true;
        AudioManager.instance.PauseAll();
    }


    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Timer.instance.SetTime(0);
    }

    public void HoverSound()
    {
        AudioManager.instance.Play("hover_ui");
    }

    public void ClickSound()
    {
        AudioManager.instance.Play("click_ui");

    }
}
