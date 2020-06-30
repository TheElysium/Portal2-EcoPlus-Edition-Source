using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    // pour faciliter l'implementation de l'ascenseur il existe deux prefabs , un avec la plateforme en bas et l'autre avec la plateforme en haut 
    // ce qui permet d'obtenir l'ascenseur de début et de fin du niveau

    public bool isUp = false; // boolean utilise pour definir l'etat de depart de la l'ascenseur (en haut ou en bas) pour determiner l'animation a jouer

    private Animator animator;

    [SerializeField]
    private ElevatorDoor doorScript = null; // la porte de l'ascenseur

    private bool hasElevated = false; // statut de l'ascenseur , si il a déjà été utilisé ou pas

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isUp", isUp); // on défini le boolean dans l'animator a partir de la position indiquée de l'ascenseur 
    }
    void Start()
    {
        if (isUp) // si l'ascenseur est en haut lors du démarrage du niveau
        {
            Elevate(); // on lance l'ascenseur dès le demarrage du niveau pour faire descendre le joueur
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // si le joueur entre dans l'ascenseur
        {
            if (!hasElevated) { // si l'ascenseur n'a pas été activé
                Elevate(); // on lance l'ascenseur
            }
        }

    }

    private void Elevate()
    {
            if (isUp) // si l'ascenseur est en haut 
            {
                AudioManager.instance.Play("asc_arrive");
     
                StartCoroutine(WaitForAnimation()); // on lance une coroutine
            }
            else // sinon 
            {
                AudioManager.instance.Play("asc_depart");
                doorScript.Close(); // on ferme la porte
            }
            
            animator.SetTrigger("elevate"); // dans tous les cas on lance l'animation de l'ascenseur
            hasElevated = true; // on actualise son statut 
}

    private IEnumerator WaitForAnimation()
    {
        // la coroutine permet dans le cas de la descente de l'ascenseur,
        // d'ouvrir la porte de l'ascenseur au bon moment
        yield return new WaitForSeconds(5);
        doorScript.Open();
    }

}
