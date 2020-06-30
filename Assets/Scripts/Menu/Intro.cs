using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public GameObject instructionRoom1;
    public GameObject instructionRoom2;
    public GameObject instructionRoom3;
    public GameObject instructionRoom4;

    PlayerController playerC;
    PlayerShoot playerS;
    // Start is called before the first frame update
    void Start()
    {
        playerC = GetComponent<PlayerController>();
        playerS = GetComponent<PlayerShoot>();
        //playerC.enabled = false;
        playerS.enabled = false;
        Startup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "room1Exit":
                Room1Exit();
                break;
            case "room2Exit":
                Room2Exit();
                break;
            case "room3Exit":
                Room3Exit();
                break;
            case "room4Exit":
                Room4Exit();
                break;
            case "levelExit":
                levelExit();
                break;
            default:
                break;
        }
    }

    void Startup()
    {
        instructionRoom1.gameObject.SetActive(true);
        //Voix ?
        AudioManager.instance.Play("intro_voiceline");
        playerC.enabled = true;

    }

    void Room1Exit()
    {
        instructionRoom1.gameObject.SetActive(false);
        //Jouer voix ?

        instructionRoom2.gameObject.SetActive(true);
        playerS.enabled = true;
        
    }

    void Room2Exit()
    {
        instructionRoom2.gameObject.SetActive(false);
        //Jouer voix ?
        AudioManager.instance.Stop("intro_voiceline");
        AudioManager.instance.Play("intro_bouton");
        instructionRoom3.gameObject.SetActive(true);
    }

    void Room3Exit()
    {
        instructionRoom3.gameObject.SetActive(false);
        AudioManager.instance.Stop("intro_bouton");
        AudioManager.instance.Play("intro_vitesse");
    }

    void Room4Exit()
    {
        instructionRoom4.gameObject.SetActive(true);
        AudioManager.instance.Stop("intro_vitesse");
        AudioManager.instance.Play("intro_ralenti");
    }

    void levelExit()
    {
        instructionRoom4.gameObject.SetActive(false);
        AudioManager.instance.Stop("intro_ralenti");
        AudioManager.instance.Play("intro_finniveau");
    }


}
