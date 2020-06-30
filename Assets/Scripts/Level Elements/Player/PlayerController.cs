using UnityEditor;
using UnityEngine;

public class PlayerController : PortalTraveller
{

	public Transform camera;
	public float camSensi;
	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public float maxYVelocity = -15f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	public float midAirControl = 0.1f;
	public float slowMotion = 0.1f;
	
	private Rigidbody rigidbody;
	private CapsuleCollider CapsuleCollider;
	private bool grounded = false;
	public bool isSlowMotion = false;
	public bool isHolding;

	private PortalGunController portalGun;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		rigidbody = GetComponent<Rigidbody>();
		CapsuleCollider = GetComponent<CapsuleCollider>();
		portalGun = FindObjectOfType<PortalGunController>();
	}

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		CapsuleCollider = GetComponent<CapsuleCollider>();
		rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			if (!isSlowMotion)
			{
				Time.timeScale = slowMotion;
			}
			else
			{
				Time.timeScale = 1;
			}
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
			isSlowMotion = !isSlowMotion;
		}

		float yRot = Input.GetAxis("Mouse X");
		float xRot = Input.GetAxis("Mouse Y");

		Vector3 rotateY = new Vector3(0, yRot, 0) * camSensi;
		Vector3 rotateX = new Vector3(xRot, 0, 0) * camSensi;

		rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(rotateY));
		camera.transform.Rotate(-rotateX);
	}

	void FixedUpdate()
	{
		
		Vector3 velocity = rigidbody.velocity;
		Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		targetVelocity = transform.TransformDirection(targetVelocity);
		if (grounded)
		{
			targetVelocity *= speed;
			portalGun.SetMoveSpeed(targetVelocity.magnitude);
			if(targetVelocity != Vector3.zero && grounded)
			{
				AudioManager.instance.Play("footSteps");
			}
			else
			{
				AudioManager.instance.Stop("footSteps");
			}

			// On ajoute de la force à notre rigidbody afin d'atteindre la vélocité souhaitée
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0;
			rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
		
			// Jump
			if (canJump && Input.GetButton("Jump"))
			{
				rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
			}
			if (velocity.magnitude <= -(maxYVelocity * 0.7f))
			{
				AudioManager.instance.Stop("wind_sound");
			}
		}
		else
		{
			//On permet au joueur de se controler dans les airs
			Vector3 velocityChange;
			if(isSlowMotion)
			{
				velocityChange = targetVelocity * midAirControl * slowMotion;
			}
			else
			{
				velocityChange = targetVelocity * midAirControl;
			}

			//Empêche la velocité en y (verticale) d'être supérieure à maxYVelocity (problème de téléportation si le joueur va trop vite)
			if (velocity.y <= maxYVelocity)
			{
				velocityChange.y = -velocity.y + maxYVelocity;
			}
			if (velocity.magnitude >= -(maxYVelocity * 0.7f))
			{
				AudioManager.instance.Play("wind_sound");
			}
			rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
			portalGun.SetMoveSpeed(0f);

		}

		// We apply gravity manually for more tuning control
		rigidbody.AddForce(new Vector3(0, -gravity * rigidbody.mass, 0));
		
		grounded = false;
	}


	private void OnCollisionStay(Collision collision)
	{
		/*
		if (collision.collider.transform.eulerAngles.x == 270f)
		{
			grounded = true;
		}*/

		grounded = true;
	}

	private void OnCollisionExit(Collision collision)
	{
		grounded = false;
	}

	float CalculateJumpVerticalSpeed()
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}



	public bool isOnPickUpAble()
	{
		float raycastRadius = 0.6f;
		int PickUpAbleLayerMask = 9;
		RaycastHit hitInfo;
		if (Physics.SphereCast(rigidbody.position, raycastRadius, rigidbody.transform.up * -1, out hitInfo, 1.3f))
		{
			if (hitInfo.collider.gameObject.layer == PickUpAbleLayerMask)
			{
				return true;
			}

		}
		return false;
	}


	/*
	public bool isGrounded()
	{
		RaycastHit hitInfo;
		float raycastDist = transform.localScale.y / 2 + 0.1f;
		if (Physics.SphereCast(rigidbody.position, 0.25f, rigidbody.transform.up * -1, out hitInfo, raycastDist))
		{
			Debug.Log("grounded");
			return true;

		}
		return false;
	}*/
}