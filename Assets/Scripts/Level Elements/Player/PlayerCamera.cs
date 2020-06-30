using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    List<Portal> portals;



    void Update()
    {
        Portal[] tabPortals = FindObjectsOfType<Portal>();
        portals = new List<Portal>(tabPortals);
        
       

    }
    void OnPreCull()
    {
        if (portals.Count == 2)
        {
            for (int i = 0; i < portals.Count; i++)
            {
                portals[i].Render();
            }
        }
    }
}
