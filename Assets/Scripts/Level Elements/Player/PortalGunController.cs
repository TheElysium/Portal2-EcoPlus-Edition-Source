using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGunController : MonoBehaviour
{
    private Animator animator;

    private Animation anim;
	void Awake()
	{
        DontDestroyOnLoad(gameObject);
	}
	void Start()
    {
        animator = GetComponent<Animator>();
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetMoveSpeed(float movespeed)
    {
        animator.SetFloat("MoveSpeed",movespeed);
    }
}
