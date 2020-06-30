using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Ce script permet d'empêcher un objet/le joueur de sortir de la limite de jeu / la carte.


    public Transform Prefab; // Prefab à faire réapparaître en cas de sortie des limites de jeux

    public Transform SpawnPoint; // Empty qui sert à définir la position de réapparition du prefab

    public BoxCollider LevelTrigger; // Trigger qui défini les limites de jeux

    void OnTriggerExit(Collider col)
    {
        if (col.Equals(LevelTrigger)) // si quelque chose sort de la limite de jeu
        {
            if (CompareTag("Player"))
            {
                AudioManager.instance.Play("taunt_qi");
                DestroyPortals();
            }
            Teleport(); // on teleporte le prefab dans la zone de jeu
        }
    }

    void Teleport()
    {
        Prefab.GetComponent<Rigidbody>().velocity = Vector3.zero; // on remet a zero la velocite pour eviter les problemes de collision
        Prefab.GetComponent<Rigidbody>().rotation = Quaternion.identity; // on remet a zero la rotation
        Prefab.transform.position = SpawnPoint.transform.position; // on change la position du prefab par sa position initiale (où est placé l'empty)
    }

    void DestroyPortals()
    {
        Portal[] portals = FindObjectsOfType<Portal>();

        for (int i = 0; i < portals.Length; i++)
        {
            Destroy(portals[i].gameObject);
        }
    }
}
