using System;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

	public Transform player;
	private Portal otherPortal;
	private Camera cameraPlayer;
	private List<PortalTraveller> trackedTravellers;

	private MeshRenderer screen;
	public int recursionLimit = 6;

	RenderTexture viewTexture;
	Camera portalCam;
	Material firstRecursionMat;
	MeshFilter screenMeshFilter;

	public GameObject wall;
	private Collider wallCollider;
	
	// Start is called before the first frame update
	void Awake()
	{
		//Instanciation des variables
		wall = null;
		wallCollider = null;
		otherPortal = null;
		cameraPlayer = Camera.main;
		portalCam = GetComponentInChildren<Camera>();
		player = FindObjectOfType<PlayerController>().transform;
		portalCam.enabled = false;
		screen = GetComponentInChildren<MeshRenderer>();
		trackedTravellers = new List<PortalTraveller>();
		screenMeshFilter = screen.GetComponent<MeshFilter>();
	}

	private void LateUpdate()
	{

		if(wallCollider == null) wallCollider = wall.GetComponent<Collider>();
		if (otherPortal == null)
		{
			findOtherPortal();
			return;
		}

		Transform otherPortalT = otherPortal.transform;
		Transform portalT = transform;

		//Pour chaque objet entré dans le trigger du portail
		for (int i = 0; i < trackedTravellers.Count; i++)
		{
			PortalTraveller traveller = trackedTravellers[i];
			Transform travellerT = traveller.transform;

			//On calcule de quel coté du portail se trouve l'objet avec Vector3.Dot
			Vector3 positionDiff = travellerT.position - portalT.position;
			int angle = System.Math.Sign(Vector3.Dot(positionDiff, portalT.forward));
			int prevAngle = System.Math.Sign(Vector3.Dot(traveller.previousOffsetFromPortal, portalT.forward));

			Vector3 otherPortalCamRot = otherPortal.GetComponentInChildren<Camera>().transform.rotation.eulerAngles;
			Matrix4x4 m = otherPortalT.localToWorldMatrix * portalT.worldToLocalMatrix * travellerT.localToWorldMatrix;
			
			
			//Si l'objet est passé derrière le portail
			if (angle != prevAngle && !traveller.CompareTag("PickedCube"))
			{
				Rigidbody rigidbody = traveller.GetComponentInChildren<Rigidbody>();
				Vector3 velocity = rigidbody.velocity;
				float bump = 0;
				

				if (traveller.CompareTag("Player"))
				{
					Quaternion prevRot = travellerT.rotation;
					Quaternion prevRotCam = cameraPlayer.transform.localRotation;

					//Si un des deux portails est à l'horizontal (au sol ou au plafond)
					if (otherPortalT.eulerAngles.x !=0 || portalT.eulerAngles.x !=0) 
					{
						//Si les deux portails sont sur le sol ou sur le plafond
						if (otherPortalT.eulerAngles.x == 270 && portalT.eulerAngles.x == 270)
						{
							Debug.Log("TP cas 1");
							SetPlayerPosAndRot(travellerT, m.GetColumn(3), otherPortalT, prevRotCam, prevRot);
						}
						//Si un portail est sur le plafond et l'autre sur le sol
						else if(otherPortalT.eulerAngles.x + portalT.eulerAngles.x == 360)
						{
							Debug.Log("TP cas 2");

							Quaternion newCamRot = Quaternion.Euler(otherPortalCamRot.x, 0f, 0f);
							Quaternion newRot = Quaternion.Euler(0f, otherPortalCamRot.y, 0f);
							SetPlayerPosAndRot(travellerT, otherPortalT.position - 0.01f * otherPortalT.forward,otherPortalT, newCamRot,newRot);
						}
						//Autres cas
						else
						{
							Debug.Log("TP cas 3");

							Quaternion newCamRot = Quaternion.Euler(otherPortalCamRot.x, 0f, 0f);
							Quaternion newRot = Quaternion.Euler(0f, otherPortalT.eulerAngles.y, 0f);
							SetPlayerPosAndRot(travellerT, m.GetColumn(3), otherPortalT, newCamRot, newRot);
							if(otherPortalT.eulerAngles.x != 0) bump = 1f;
						}
					}

					//Si l'autre portail est à la verticale
					else
					{
						/*
						Quaternion newCamRot = Quaternion.Euler(otherPortalCamRot.x, 0f, otherPortalCamRot.z);
						float offset = Vector3.SignedAngle(portalT.forward, travellerT.forward, portalT.up);
						Quaternion newRot = Quaternion.Euler(0f, otherPortalT.eulerAngles.y + offset, 0f);
						SetTravellerPosAndRot(travellerT, m.GetColumn(3), otherPortalT, newCamRot, newRot);*/

						travellerT.position = m.GetColumn(3);
						travellerT.RotateAround(otherPortalT.position, otherPortalT.up, 180);
						cameraPlayer.transform.localRotation = Quaternion.Euler(otherPortalCamRot.x, 0f, otherPortalCamRot.z);
						float offset = Vector3.SignedAngle(portalT.forward, travellerT.forward, portalT.up);
						travellerT.rotation = Quaternion.Euler(0f, otherPortalT.eulerAngles.y + offset, 0f);
					}
				}
				else if (traveller.CompareTag("Cube"))
				{
					if (otherPortalT.eulerAngles.x + portalT.eulerAngles.x == 360)
					{
						travellerT.position = otherPortalT.position - otherPortalT.forward * 0.01f;
						travellerT.rotation = m.rotation;
					}
					else
					{
						travellerT.SetPositionAndRotation(m.GetColumn(3), m.rotation);
					}

					travellerT.RotateAround(otherPortalT.position, otherPortalT.up, 180);
				}

				var positionOld = travellerT.position;
				var rotOld = travellerT.rotation;

				try
				{
					traveller.graphicsClone.transform.SetPositionAndRotation(positionOld, rotOld);
					traveller.graphicsClone.transform.RotateAround(otherPortalT.position, otherPortalT.up, 180);
				}
				catch
				{
					continue;
				}


				Debug.Log("New velocity magnitude: " + velocity.magnitude);
				rigidbody.velocity = otherPortalT.forward * velocity.magnitude + otherPortalT.forward * bump;

				if (traveller.CompareTag("Player"))
				{
					for (int j = 0; j < trackedTravellers.Count; j++)
					{
						PortalTraveller travellerChild = trackedTravellers[j];
						if (travellerChild.CompareTag("PickedCube"))
						{
							/*
							Collider childCollider = travellerChild.GetComponent<Collider>();
							Physics.IgnoreCollision(childCollider, wallCollider, false);*/
							ManageCollisions(wall, travellerChild, false);

							travellerChild.ExitPortalThreshold();
							trackedTravellers.Remove(travellerChild);
							i -= 1;
							break;
						}
					}
				}

				/*
				Collider cubeCollider = traveller.GetComponent<Collider>();
				Physics.IgnoreCollision(wallCollider, cubeCollider, false);
				*/
				ManageCollisions(wall, traveller, false);
				AudioManager.instance.PlayAtPoint("portal_enter", otherPortalT.position);
				otherPortal.OnTravellerEnterPortal(traveller);
				trackedTravellers.Remove(traveller);
				i--;

			}
			else
			{
				traveller.graphicsClone.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
				traveller.graphicsClone.transform.RotateAround(otherPortalT.position, otherPortalT.up, 180);
				if (!traveller.CompareTag("PickedCube"))
				{
					traveller.previousOffsetFromPortal = positionDiff;
				}

			}
		}
		
	}

	void SetPlayerPosAndRot(Transform travellerT, Vector3 newPosition, Transform otherPortalT, Quaternion newCamRot, Quaternion newRot)
	{
		travellerT.position = newPosition;
		travellerT.RotateAround(otherPortalT.position, otherPortalT.up, 180);
		cameraPlayer.transform.localRotation = newCamRot;
		travellerT.rotation = newRot;
	}

	void findOtherPortal()
	{
		Portal[] portals = FindObjectsOfType<Portal>();

		if (this.CompareTag("Portal1"))
		{
			foreach (Portal p in portals)
			{
				if (p.CompareTag("Portal2"))
				{
					otherPortal = p;
					return;
				}
			}
			otherPortal = null;

		}

		else if (this.CompareTag("Portal2"))
		{
			foreach (Portal p in portals)
			{
				if (p.CompareTag("Portal1"))
				{
					otherPortal = p;
					return;
				}
			}
			otherPortal = null;
		}
	}



	void OnTravellerEnterPortal(PortalTraveller traveller)
	{
		Debug.Log("Portal: Enter");
		if (!trackedTravellers.Contains(traveller))
		{
			traveller.EnterPortalThreshold();
			traveller.previousOffsetFromPortal = traveller.transform.position - transform.position;
			trackedTravellers.Add(traveller);
			if (otherPortal != null )
			{
				ManageCollisions(wall, traveller, true);
			}
		}
	}

	void OnTravellerExitPortal(PortalTraveller traveller)
	{
		Debug.Log("Portal: Enter");
		ManageCollisions(wall, traveller, false);
		trackedTravellers.Remove(traveller);
	}


	void ManageCollisions(GameObject wall, PortalTraveller traveller, bool enable)
	{
		Collider wallCollider = wall.GetComponent<Collider>();
		Collider travellerCollider = traveller.GetComponent<Collider>();
		Physics.IgnoreCollision(wallCollider, travellerCollider, enable);
	}


	private void OnTriggerEnter(Collider other)
	{

		var traveller = other.GetComponent<PortalTraveller>();
		if (traveller)
		{
			OnTravellerEnterPortal(traveller);
			
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var traveller = other.GetComponent<PortalTraveller>();
		if (traveller && trackedTravellers.Contains(traveller))
		{
			if (traveller.CompareTag("PickedCube"))
			{
				Vector3 positionDiff = traveller.transform.parent.parent.parent.position - transform.position;
				int anglePlayer = System.Math.Sign(Vector3.Dot(positionDiff, transform.forward));
				Vector3 positionDiffPickedCube = traveller.transform.position - transform.position;
				int anglePickedCube = System.Math.Sign(Vector3.Dot(positionDiffPickedCube, transform.forward));

				if (anglePlayer == anglePickedCube)
				{
					traveller.ExitPortalThreshold();
					OnTravellerExitPortal(traveller);
				}
			}
			else if(traveller.CompareTag("Player") || traveller.CompareTag("Cube"))
			{
				traveller.ExitPortalThreshold();
				OnTravellerExitPortal(traveller);
			}
		}
		
	}

	public void Render()
	{
		if (otherPortal != null)
		{
			try {
				screen.material.SetInt("displayMask", 1);
			}
			catch
			{
				return;
			}

			CreateViewTexture();
			screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
			otherPortal.screen.material.SetInt("displayMask", 0);
			Matrix4x4 m = transform.localToWorldMatrix * otherPortal.transform.worldToLocalMatrix * cameraPlayer.transform.localToWorldMatrix;
			portalCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
			portalCam.transform.RotateAround(transform.position, transform.up, 180);

			portalCam.Render();
			otherPortal.screen.material.SetInt("displayMask", 1);

			screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		}
	}

	void CreateViewTexture()
	{
		if (viewTexture != null)
		{
			viewTexture.Release();
		}
		viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
		// Render the view from the portal camera to the view texture
		portalCam.targetTexture = viewTexture;
		// Display the view texture on the screen of the linked portal

		otherPortal.screen.material.SetTexture("_MainTex", viewTexture);
	
		}

	
	private void OnDestroy()
	{
		foreach (PortalTraveller traveller in trackedTravellers)
		{
			Physics.IgnoreCollision(wallCollider, traveller.GetComponent<Collider>(), false);
		}
	}

	private static float WrapAngle(float angle)
	{
		angle %= 360;
		if (angle > 180)
			return angle - 360;

		return angle;
	}

}

