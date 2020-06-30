using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    // Script qui permet d'ouvrir ou de fermer la porte de l'ascenseur
    public string voiceline;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Open()
    {
        AudioManager.instance.PlayAtPoint("open_door_asc", transform.position);
        animator.SetTrigger("Open"); // on ouvre la porte
        StartCoroutine(WaitForAnimation());
    }

    public void Close()
    {
        AudioManager.instance.PlayAtPoint("close_door_asc", transform.position);
        animator.SetTrigger("Close"); // on ferme la porte
    }

    private IEnumerator WaitForAnimation()
    {
        // la coroutine permet dans le cas de la descente de l'ascenseur,
        // d'ouvrir la porte de l'ascenseur au bon moment
        yield return new WaitForSeconds(1);
        AudioManager.instance.Play(voiceline);
    }
}
