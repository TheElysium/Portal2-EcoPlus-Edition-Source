using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class CubeDoor : MonoBehaviour
{
    private Animator animator; // contient les animations de la porte

    [SerializeField]
    private string layerName = "PickUpAble"; // nom du layer des objets ramassables 

    [SerializeField]
    private GameObject bridgeDoor = null; // gameobject du "pont" à déployer en même temps que la porte (Facultatif)
    private BridgeDoor bridgeScript = null; // script du "pont"
    void Start()
    {
        animator = GetComponentInParent<Animator>(); // on recupere l'animator 

        if (bridgeDoor != null) // si le "pont" est déclaré dans l'editeur
        {
            bridgeScript = bridgeDoor.GetComponent<BridgeDoor>(); // on recupere le script du "pont"
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerName) || other.gameObject.CompareTag("Player")) // si le joueur OU l'objet ramassable entre dans le trigger
        {
            if (!animator.GetBool("Open")) // si la porte est fermée
            {
                AudioManager.instance.Play("bouton");
                Open(); // on ouvre la porte
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerName) || other.gameObject.CompareTag("Player")) // si le joueur OU l'objet ramassable sors du trigger
        {
            if (animator.GetBool("Open")) // si la porte est ouverte
            {
                Close(); // on ferme la porte
            }
        }
    }

    private void Open()
    {
        AudioManager.instance.PlayAtPoint("open_door", transform.position);
        animator.SetBool("Open", true);// on ouvre la porte

        if (bridgeDoor != null) // si le "pont" existe
        {
            bridgeScript.setBridgeState(true); // on l'ouvre
        }
    }

    private void Close()
    {
        AudioManager.instance.PlayAtPoint("close_door", transform.position);
        animator.SetBool("Open", false); // on ferme la porte
        if (bridgeDoor != null) // si le "pont" existe
        {
            bridgeScript.setBridgeState(false); // on le ferme
        }
    }
}
