using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    
    // booleans
    [Header("Main Booleans")]
    public bool canHold = true; // le joueur peut ramasser cet objet
    public bool isHoldingThis = false; // le joueur tiens cet objet
    public bool isLookingAtThis = false; // le joueur regarde cet objet
    
    [Header("Required")]
    public GameObject item; // l'objet ramassable
    public string layerName = "PickUpAble"; // layer des objets ramassables
    private int layerId = 0;

    //OBJET à bouger
    private Vector3 objectPos; // position de l'objet
    private Rigidbody itemRb; // rigidbody de l'objet

    //GUIDE de l'objet 'temp parent'
    private GameObject tempParent;

    // PLAYER
    private GameObject player;
    private Camera playerCam;
    private PlayerController playerController;

    // VALUES
    [Header("Settings")]
    public float maxPickUpRange = 2.5f; // la distance maximum pour ramasser l'objet

    private void Awake()
    {
        tempParent = GameObject.FindGameObjectWithTag("PickUpGuide");
        player = GameObject.FindWithTag("Player");
        itemRb = item.GetComponent<Rigidbody>();
        playerCam = Camera.main;
        playerController = player.GetComponent<PlayerController>();

        layerId = LayerMask.NameToLayer(layerName);

        // Changement automatique du layer de l'objet ramassable courant
    ChangeLayerRecursively(item.transform, layerId);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 objectPos = item.transform.position; // on sauvegarde la position de l'objet pour plus tard
        
        isLookingAtHim(); // on mets à jour le boolean pour savoir si le joueur regarde cet objet ou pas 

        if (isHoldingThis && !player.gameObject.GetComponent<PlayerController>().isOnPickUpAble()) // si le joueur tient le cube et qu'il n'est pas dessus
        {
            isHoldingMove(); // on lance le mouvement du cube
        }
        else // sinon
        {
            isHoldingStopMove(); // on lance la fonction pour lacher le cube
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canHold && isLookingAtThis && !playerController.isHolding && !playerController.isOnPickUpAble()) // on vérifie si le joueur : peut prendre l'objet, regarde cet objet, si il ne tient rien, et si il n'est pas sur l'objet
            {
                // le joueur ramasse le cube
                isHoldingThis = true; 
                playerController.isHolding = true; 
                tag = "PickedCube";
                AudioManager.instance.Play("cube");
            }
            else
            {
                if (isHoldingThis)
                {
                    // le joueur lache le cube
                    isHoldingThis = false;
                    playerController.isHolding = false;
                    tag = "Cube";
                    AudioManager.instance.Stop("cube");
                }
            }


        }

        if (!playerController.isHolding) // on arrête le son si le joueur ne tiens plus le cube (dans le cas de la destruction par le champ de force)
        {
            AudioManager.instance.Stop("cube");
        }
    }

    private void isLookingAtHim() // savoir si le joueur regarde le cube où ce script est appliqué
    {
        RaycastHit hit;
        bool a = Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, maxPickUpRange, ~layerId, QueryTriggerInteraction.Ignore);
        if (a) // si le raycast touche
        {
            if (GameObject.ReferenceEquals(hit.transform.gameObject, item)) isLookingAtThis = true; // on regarde si le cube touché est le même que celui de ce script et on met a jour le boolean

        }
        else isLookingAtThis = false;
    }

    private void isHoldingMove() // fonction déclenchée quand on prend le cube
    {
        Physics.IgnoreCollision(item.GetComponent<Collider>(), player.GetComponent<CapsuleCollider>(),true); // on desactive la collision entre le cube et le joueur
        itemRb.velocity = Vector3.zero;
        itemRb.angularVelocity = Vector3.zero;
        item.transform.SetParent(tempParent.transform); // on parente le cube au guide
        item.transform.position = tempParent.transform.position; // on met a jour la position du cube
        item.transform.rotation = tempParent.transform.rotation; // on met a jour la rotation du cube
    }

    private void isHoldingStopMove() // fonction déclenchée quand on lache le cube
    {
        Physics.IgnoreCollision(item.GetComponent<Collider>(), player.GetComponent<CapsuleCollider>(), false); // on active la collision entre le cube et le joueur
        objectPos = item.transform.position; // on sauvegarde la position du cube dans une variable
        item.transform.SetParent(null); // on enlève le lien de parenté
        item.transform.position = objectPos; // on redonne la bonne position au cube
    }

    // Fonctions utilitaires

    void ChangeLayerRecursively(Transform root, int layer) // change le layer d'un gameobject et de tous ses enfants
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            ChangeLayerRecursively(child, layer);
    }
}

