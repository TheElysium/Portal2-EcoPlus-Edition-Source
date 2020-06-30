using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyField : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerController pC = null;
    void Start()
    {
        pC = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) DestroyPortal();
        else if (other.gameObject.CompareTag("Cube")) DestroyCube(other.gameObject);
        else if (other.gameObject.CompareTag("PickedCube"))
        {
            StopPickUp(other.gameObject);
            DestroyCube(other.gameObject);
        }
    }



    void DestroyPortal()
    {
        Portal[] portals = FindObjectsOfType<Portal>();
        Debug.LogError(portals.Length);

        for (int i = 0; i < portals.Length; i++)
        {
            Destroy(portals[i].gameObject);
        }
    }

    void DestroyCube(GameObject cube)
    {
        cube.transform.position = cube.GetComponent<Respawn>().SpawnPoint.position; // on teleporte le cube a la position de son apparition
    }

    void StopPickUp(GameObject cube)
    {
        cube.GetComponent<PickUp>().isHoldingThis = false;
        pC.isHolding = false;
        cube.tag = "Cube";
    }
}
