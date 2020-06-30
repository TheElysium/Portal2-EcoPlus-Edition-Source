using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : PortalTraveller
{

    Rigidbody rb;
    public float gravity = 15f;
    public float maxYVelocity = -17.5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CompareTag("Cube"))
        {
            rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));
            float velocityChange = rb.velocity.y;
            //Empêche la velocité en y (verticale) d'être supérieure à maxYVelocity (problème de téléportation si l'objet va trop vite)
            if (velocityChange <= maxYVelocity)
            {
                velocityChange = -maxYVelocity - 12.5f;
                rb.AddForce(new Vector3(0f,velocityChange,0f), ForceMode.VelocityChange);
            }

        }
    }
}
