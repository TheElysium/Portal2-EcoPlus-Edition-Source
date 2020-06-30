using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerShoot : MonoBehaviour
{
	public PlayerWeapon weapon;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private LayerMask collisionMask;

	[SerializeField]
	private Portal portailGraphics2;
	[SerializeField]
	private Portal portailGraphics1;
	private Transform playerPosition;



	public bool portail2Floor = false;
	public bool portail1Floor = false;
	public bool noCollision = true;

	private Vector3 offset;
	private Vector3 hitPoint;
	private Quaternion playerPositionXR;

	private Portal portail;
	private Portal portail2;
	
	private Collider[] check;

	private bool rightClick;
	private bool leftClick;

	public Animation anim;

	void Start()
	{

		portail = null;
		portail2 = null;
		if (cam == null)
		{
			// Debug.LogError("Pas de camera référencée");
			this.enabled = false;
		}
	}
	void Update()
	{
		if (Input.GetMouseButtonDown(0)) //On détecte un clic gauche pour lancer la suite
		{
			noCollision = true;
			leftClick = true;
			Shoot();
			AudioManager.instance.Play("portalGunShootBlue");
			leftClick = false;
		}
		else if (Input.GetMouseButtonDown(1)) //On détecte un clic droit pour lancer la suite
		{
			noCollision = true;
			rightClick = true;
			Shoot();
			AudioManager.instance.Play("portalGunShootRed");
			rightClick = false;
		}
	}

	private void Shoot()
	{
		if (anim.isPlaying)
		{
			anim.Stop();
		}
		anim.Play();
		RaycastHit _hit;
		GameObject[] pickedCube = GameObject.FindGameObjectsWithTag("PickedCube");
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask) && pickedCube.Length == 0) //Raycast pour connaître la surface touché et où elle a été touchée
		{
			hitPoint = _hit.point;
			hitPoint = hitPoint * 0.99999f;
			playerPosition = GetComponent<Transform>();
			playerPositionXR = playerPosition.rotation;
			GameObject wall = _hit.collider.gameObject;

			check = Physics.OverlapBox(hitPoint, (portailGraphics1.transform.localScale) / 2 + new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity, collisionMask); //Permet d'éviter d'avoir un portail sur l'autre ou dans un mur
			Collider[] c = Physics.OverlapBox(hitPoint, (portailGraphics1.transform.localScale) / 2 + new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity, mask);
			for (int i = 0; i < check.Length; i++)
			{
				
				if (noCollision == true)
				{
					noCollision = (check[i].gameObject.layer == LayerMask.NameToLayer("portail1") && Input.GetMouseButtonDown(0)) ||
								  (check[i].gameObject.layer == LayerMask.NameToLayer("portail2") && Input.GetMouseButtonDown(1));
				}
				else break;

				//Permet de spammer les portails 
				//noCollision = (check[i].gameObject.layer == LayerMask.NameToLayer("portail1") && Input.GetMouseButtonDown(0)) ||
								  //(check[i].gameObject.layer == LayerMask.NameToLayer("portail2") && Input.GetMouseButtonDown(1));
			}
			Vector3 portalHeight = Vector3.zero;

			if (c.Length == 2)
			{
				Transform other = null;
				Transform curr = null;
				for (int i = 0; i < c.Length; i++)
				{
					if (c[i].gameObject == _hit.collider.gameObject)
					{
						curr = c[i].transform;
						portalHeight = Vector3.Scale((portailGraphics1.transform.localScale / 2), curr.up);
					}
					else other = c[i].gameObject.transform;
				}
				float up = Vector3.Scale(hitPoint, curr.up).magnitude;
				float upOther = Vector3.Scale(other.position, other.forward).magnitude;

				if (curr.eulerAngles.x == 0)
				{
					if (up > upOther)
					{
						float distance = up - upOther;
						offset = portalHeight - curr.up * (distance);
					}
					else
					{
						float distance = upOther - up;
						offset = -portalHeight + curr.up * distance;
					}
				}
				else offset = Vector3.zero;
			}

			if ((check.Length == 0 || noCollision)) //On check le fait qu'il n'y ait pas de collision non souhaité
			{
				if (_hit.collider.gameObject.layer == LayerMask.NameToLayer("floorSpawn")) //Permet de connaître la surface rencontrée
				{
					Vector3 hitRot = _hit.transform.eulerAngles;
					float f;
					if (hitRot.x == 90f) 
					{
						f = 0f;
					}
					else f = 180f;
					Quaternion portalRot = Quaternion.Euler(hitRot.x, playerPositionXR.eulerAngles.y + f, hitRot.z);

					if (leftClick) //Permet de connaître le portail à faire spawn
					{
						// Debug.Log("Objet touché : " + _hit.collider.name);
						portail1Floor = true;
						SpawnPortail(portailGraphics1, portalRot, wall);
					}
					else if (rightClick) //Permet de connaître le portail à faire spawn
					{
						// Debug.Log("Objet touché : " + _hit.collider.name);
						portail2Floor = true;
						SpawnPortail2(portailGraphics2, portalRot, wall);
					}
				}
				else if (_hit.collider.gameObject.layer == LayerMask.NameToLayer("wallSpawn")) //Permet de connaître la surface rencontrée
				{

					Quaternion rot = Quaternion.Euler(_hit.transform.eulerAngles.x, _hit.transform.eulerAngles.y, 0);

					if (leftClick) //Permet de connaître le portail à faire spawn
					{
						// Debug.Log("Objet touché : " + _hit.collider.name);
						portail1Floor = false;
						SpawnPortail(portailGraphics1, rot, wall);
					}
					else if (rightClick) //Permet de connaître le portail à faire spawn
					{
						// Debug.Log("Objet touché : " + _hit.collider.name);
						portail2Floor = false;
						SpawnPortail2(portailGraphics2, rot, wall);
					}
				}
			}offset = Vector3.zero;
		}
	}
	private void SpawnPortail(Portal objetSpawn, Quaternion surfaceSpawn, GameObject wall)
	{
		Debug.Log("portal1 created");
		Vector3 test = surfaceSpawn * Vector3.forward;
		hitPoint += test * 0.05f;
		if (portail != null) //On check si le portal n'est pas encore là, sinon, on le détruit
		{
			Destroy(portail.gameObject);
		}
		portail = Instantiate(objetSpawn, hitPoint + offset, surfaceSpawn); //Création du portail au point où le raycast a touché
		AudioManager.instance.PlayAtPoint("portal_open1", portail.transform.position); //Son de l'appartition du portail
		//Ajout des paramètres du portail
		portail.tag = "Portal1"; 
		portail.gameObject.layer = LayerMask.NameToLayer("portail1");
		portail.wall = wall;
	}
	private void SpawnPortail2(Portal objetSpawn, Quaternion surfaceSpawn, GameObject wall)
	{
		Debug.Log("portal2 created");

		Vector3 test = surfaceSpawn * Vector3.forward;
		hitPoint += test * 0.05f;
		if (portail2 != null) //On check si le portal n'est pas encore là, sinon, on le détruit
		{
			Destroy(portail2.gameObject);
		}
		
		portail2 = Instantiate(objetSpawn, hitPoint + offset, surfaceSpawn); //Création du portail au point où le raycast a touché
		AudioManager.instance.PlayAtPoint("portal_open2", portail2.transform.position); //Son de l'appartition du portail
		//Ajout des paramètres du portail
		portail2.tag = "Portal2";
		portail2.gameObject.layer = LayerMask.NameToLayer("portail2");
		portail2.wall = wall;
	}
}

