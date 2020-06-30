using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public GameObject crosshairF;
    public GameObject crosshairE;
    public GameObject crosshairL;
    public GameObject crosshairR;

    private GameObject lastCrosshair;

    private void Start()
    {
        lastCrosshair = crosshairE;
        lastCrosshair.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Portal[] portals = FindObjectsOfType<Portal>();
        if (portals.Length == 1 && (lastCrosshair != crosshairL || lastCrosshair != crosshairR))
        {
            foreach (Portal p in portals)
            {
                if(p.gameObject.layer == 13) //Portail1
                {
                    changeCrosshair(crosshairL);
                }
                else
                {
                    changeCrosshair(crosshairR);
                }
            }
        }
        else if(portals.Length == 2 && lastCrosshair != crosshairF)
        {
            changeCrosshair(crosshairF);
        }
        else if(portals.Length == 0)
        {
            changeCrosshair(crosshairE);
        }
    }
    
    void changeCrosshair(GameObject newCrosshair)
    {
        lastCrosshair.SetActive(false);
        lastCrosshair = newCrosshair;
        lastCrosshair.SetActive(true);
    }
}
