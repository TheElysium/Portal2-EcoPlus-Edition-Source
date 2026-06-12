using UnityEngine;

public class Bumper : MonoBehaviour
{
    public Animation animation;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger bumper");
        animation.Play();
    }




}
