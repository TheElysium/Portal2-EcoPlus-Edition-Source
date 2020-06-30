using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource buttonSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public void Start()
    {
        Timer.instance.SetTime(0);
    }
    public void playGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);      
    }
    
    public void quitGame()
    {
        Debug.Log("quit");
        Application.Quit();
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
}
