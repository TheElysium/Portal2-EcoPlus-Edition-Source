using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeDoor : MonoBehaviour
{
    // Script qui permet d'ouvrir le "pont" et de le fermer

    private Animator animator; // l'animator qui contient les animations du "pont"

    void Start()
    {
        animator = GetComponentInParent<Animator>(); // on recupere l'animator dans le parent
    }

    public void setBridgeState(bool state) // fonction qui permet de definir l'etat du "pont" (ouvert ou fermé)
    {
        animator.SetBool("deploy", state);
    }
}
