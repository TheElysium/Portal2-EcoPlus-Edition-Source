using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    private float currentTime = 0f;
    public static Timer instance;
    bool isLevel = false;

    private GameObject[] timerText;
    private TextMeshProUGUI text;
    private bool textExists = false;

    private string timeStr = null;
    void Start()
    {
        updateOnLevelChange();
    }

    public float getTime()
    {
        return currentTime;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        
        if (isLevel) // si il y a le joueur
        {
            currentTime += Time.deltaTime;
        }


        if (textExists)
        {
            timeStr = ((int)currentTime / 60).ToString("00") + ":" + (currentTime % 60).ToString("00");
            text.SetText(timeStr);
        }
    }

    public void SetTime(float time)
    {
        currentTime = time;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        updateOnLevelChange();
    }

    void updateOnLevelChange()
    {
        textExists = false;
        isLevel = !FindObjectOfType<PlayerController>().Equals(null);
        timerText = GameObject.FindGameObjectsWithTag("timer");
        getTimerTextMeshPro();
        DiscordController.instance.UpdateLevel();
    }

    void getTimerTextMeshPro()
    {
        foreach (GameObject g in timerText)
        {
            if (g.CompareTag("timer"))
            {
                text = g.GetComponent<TextMeshProUGUI>();
                textExists = true;
                return;
            }
        }
    }
}
