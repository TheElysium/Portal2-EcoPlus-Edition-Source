using UnityEngine;

public class Bumper : MonoBehaviour
{

    public int launchAngle = 0;
    public Transform destinationT;
    private bool isBumped = false;
    private GameObject bumpedObject = null;


    // Update is called once per frame
    void FixedUpdate()
    {
        if (isBumped)
        {
            Launch(bumpedObject);
            isBumped = false;
            bumpedObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");

        if (!other.CompareTag("PickedCube"))
        {
            isBumped = true;
            bumpedObject = other.gameObject;
        }
    }

    private void Launch(GameObject o)
    {
        
        Rigidbody rb = o.GetComponent<Rigidbody>();
        
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Quaternion launchRot = Quaternion.AngleAxis(launchAngle, transform.right.normalized);
        Vector3 launchVelocity = launchRot * (transform.forward.normalized * 12);
        Debug.Log(transform.forward);
        Debug.Log(transform.up);
        rb.velocity = launchVelocity;
         Debug.Log(rb.velocity);
      
    }

}
