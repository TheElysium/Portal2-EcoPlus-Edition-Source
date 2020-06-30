using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DevMenu : MonoBehaviour
{
    // Start is called before the first frame update
    bool state = false;
    public GameObject panel;
    public GameObject firstBtn;
    void Start()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleMenu();
            Timer.instance.SetTime(666666f); // anti triche pour le compteur du run
        }
    }

    void ToggleMenu()
    {
        if (!state)
        {
            OpenMenu();
        }
        else if (state)
        {
            CloseMenu();
        }
    }

    void OpenMenu()
    {
        panel.SetActive(true);
        state = true;
        EventSystem.current.SetSelectedGameObject(firstBtn);
    }

    void CloseMenu()
    {
        panel.SetActive(false);
        state = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ButtonClick(string name)
    {
        SceneManager.LoadScene(name);
    }
}
