using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;

    private float lastCollideTime;

    [SerializeField]
    private float timeTillClose = 3f;
    void Start()
    {
        animator = GetComponent<Animator>();
        lastCollideTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        lastCollideTime += Time.deltaTime;

        if (lastCollideTime >= timeTillClose && animator.GetBool("Open"))
        {
            AudioManager.instance.PlayAtPoint("close_door", transform.position);
            animator.SetBool("Open", false);
            lastCollideTime = 0;
        }
    }
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!animator.GetBool("Open"))
            {
                AudioManager.instance.PlayAtPoint("open_door", transform.position);
                animator.SetBool("Open", true);
                lastCollideTime = 0;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lastCollideTime = 0;
        }
    }
}