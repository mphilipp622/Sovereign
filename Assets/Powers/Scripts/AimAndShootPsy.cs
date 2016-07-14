using UnityEngine;
using System.Collections;
using UnityEditor;

public class AimAndShootPsy : MonoBehaviour {

	public Rigidbody projectile, clone, grabbedObject, pushProjectile, pushClone; //grabbedObject is assigned in the EnableManipulation Script
	public float speed = 100, pushSpeed = 50, projectileSpeed;
	public float maxChargeTime = 2.0f, chargeTime = 0.0f, heldDown = 0.0f;
	public Transform spawnpoint, camera, WhereToHoldObject, MaxObjectDistance, originalTransform;
	private Quaternion projectileRotation, pushRotation;
	private Quaternion bowRotation;
	private Vector3 targetLocation;
	RaycastHit whereToShoot;
	public bool haveObject = false, hitObject = false, canShoot = true, canThrow = false;
	private float angle;
	RaycastHit grabbedObjectRay;

	public float costToShoot = 1.0f, costToThrow = 1.0f, costToPush = 1.0f, currentStamina;
	private Bounds grabbedObjectBounds, clippingBounds;


	//These variables are used for particle settings
	private ParticleSystem powerParticleMain;
	private SerializedObject powerParticle;
	private Transform particles;
	private float startingParticleShape, startSpeedParticle;

//	private GameObject cameraParent;
	private Camera cam;

	[SerializeField]
	private AudioClip powerHoldClip;
	[SerializeField]
	private AudioClip powerChargeClip;
	private AudioSource powerAudio;

	RaycastHit pushRaycast;
	Ray pushRay;
	LineRenderer pushLine;
	float rayLength = 10.0f;
	float nextCanPushTime = 0.0f, pushPenaltyTime = 1.0f;
	Rigidbody pushedObject;
	GameObject pushParticle;
	Transform currentObject, lastObject;
	
	void Start () 
	{

		camera = Camera.main.transform;
		cam = Camera.main;
//		cameraParent = GameObject.Find ("Offset");
		powerAudio = transform.GetComponent<AudioSource> ();
		whereToShoot = new RaycastHit (); //Will be used for determining where to shoot object
		powerParticleMain = transform.GetComponentInChildren<ParticleSystem>();
		powerParticle = new SerializedObject (GetComponentInChildren<ParticleSystem> ());
		startingParticleShape = powerParticle.FindProperty ("ShapeModule.radius").floatValue = 4.18f;
		powerParticle.ApplyModifiedProperties();
		particles = transform.Find ("ChargeParticles");
		particles.gameObject.SetActive (false);
		startSpeedParticle = powerParticleMain.startSpeed;
		pushLine = GetComponentInChildren<LineRenderer>();
		pushRaycast = new RaycastHit();
		pushParticle = GameObject.Find("PushParticles");

	}

	void Update () 
	{


		//create the raycast
		if (Physics.Raycast (camera.position, camera.forward, out whereToShoot)) 
		{
			//set the target location to the Vector3 location that our raycast is hitting
			targetLocation = whereToShoot.point;
			//Debug.Log ("Where to Shoot" + whereToShoot.collider); //used for testing to know what object we're hitting with the raycast
		}

		if (currentStamina <= 0) 
		{
			particles.gameObject.SetActive (false);
		}

		//If user Presses Left Mouse and They are able to shoot
		if (Input.GetButton ("Fire1") && canShoot && currentStamina >= 0) 
		{
			if(Input.GetButtonDown("Fire1"))
			{
//				SendMessageUpwards ("ChangeDamping", 0.96f);
//				SendMessageUpwards("doShake"); // Start shaking camera when the player presses the button down.
				if(powerAudio.clip != powerChargeClip)
					powerAudio.clip = powerChargeClip;
				powerAudio.Play();
			}
			chargeTime += Time.deltaTime;//Set chargeTime to equal one second of game time. This executes continuously as long as user is holding down mouse button
			gameObject.SendMessageUpwards("PsyCharging", true); //tell our Character controller that we are Charging our power
			//find out if we have not yet reached our maximum charge time so that we can calculate the stamina reduction
			if(chargeTime < maxChargeTime)
			{
				if(chargeTime < 1.0f)
					cam.fieldOfView = Mathf.Lerp (60, 70, chargeTime / maxChargeTime);
				else if(chargeTime > 1.0f)
					cam.fieldOfView = Mathf.Lerp (70, 60, chargeTime / maxChargeTime);
				particles.gameObject.SetActive(true);
				powerParticle.FindProperty("ShapeModule.radius").floatValue = Mathf.Lerp (startingParticleShape, 1.0f, chargeTime / maxChargeTime);
				powerParticle.ApplyModifiedProperties();
				powerParticleMain.startSpeed = Mathf.Lerp (startSpeedParticle, -0.1f, chargeTime / maxChargeTime);

				//gameObject.SendMessageUpwards("Charging", true, SendMessageOptions.RequireReceiver);
				//gameObject.SendMessageUpwards ("ChargeTime", chargeTime, SendMessageOptions.RequireReceiver);

				//tell Character Controller to reduce stamina from 0 - the cost of the power over the course of how long it takes us to fully charge an attack
				gameObject.SendMessageUpwards("PsyStamina", Mathf.Lerp (0, costToShoot, Time.deltaTime / maxChargeTime)); 
			}
			else if(chargeTime > maxChargeTime)
			{
				if(powerAudio.clip != powerHoldClip)
					powerAudio.clip = powerHoldClip;
				if(!powerAudio.isPlaying){
					powerAudio.loop = true;
					powerAudio.Play();
				}
			}
			//else if(chargeTime >= maxChargeTime)
				//powerParticleMain.startSpeed = -0.1f;
		}
		//is player letting up on the mouse and have we exceeded the amount of time that's required for a fully charged attack and can we currently shoot?
		else if (Input.GetButtonUp ("Fire1") && chargeTime >= maxChargeTime && canShoot)
		{
			//cam.fieldOfView = Mathf.Lerp (70, 60, chargeTime);
			ShootWeapon ();
			powerAudio.Pause ();
			//powerAudio.clip = powerChargeClip;
			particles.gameObject.SetActive(false);
			chargeTime = 0; //this is necessary so the above logic for charging works properly.
			gameObject.SendMessageUpwards("PsyCharging", false); //tell our Character Controller that we are no longer charging our power.
		} 
		//is player letting up on mouse and have we NOT reached the required charge time? We will NOT fire a PsyBullet
		else if (Input.GetButtonUp ("Fire1") && chargeTime < maxChargeTime)
		{
			//cam.fieldOfView = Mathf.Lerp (70, 60, chargeTime);
			powerAudio.Stop ();
			powerAudio.clip = powerChargeClip;
			particles.gameObject.SetActive(false);
			chargeTime = 0; //reset chargeTime
			gameObject.SendMessageUpwards("PsyCharging", false); //Tell Character Controller we're no longer charging
//			SendMessageUpwards ("ChangeDamping", 0f);
		}
		//is player letting go of mouse button and do they have an object currently?
		else if (Input.GetButtonDown ("Fire1") && haveObject)
			ShootWeapon (); //fire the object

		//make sure camera is returned to original position when user is not firing
		if (chargeTime == 0) 
		{
//			cameraParent.transform.localPosition = new Vector3 (0, 0, 0);
		}
		//Setup the behavior for when we hit an object with our psychic power.
		if (hitObject) 
		{

			if(powerAudio.clip != powerHoldClip)
				powerAudio.clip = powerHoldClip;
			if(!powerAudio.isPlaying)
			{
				powerAudio.UnPause();
			}
			canShoot = false;
			gameObject.SendMessageUpwards("PsyHolding", true);

			grabbedObject.gameObject.layer = 2; //This is necessary to make sure our raycast goes through our object that we are holding.

			//Interpolate the position and rotation of the grabbed object to the holding area's position and rotation.
			grabbedObject.transform.position = Vector3.Lerp (grabbedObject.transform.position, WhereToHoldObject.position, Time.deltaTime * 5);
			grabbedObject.transform.rotation = Quaternion.Slerp (grabbedObject.transform.rotation, WhereToHoldObject.rotation, Time.deltaTime * 5);

			//Setup an Angle variable that will determine the angle between our grabbed object's rotation and our holding area's rotation.
			//This is used to determine when to stop our interpolation and allow the player to manipulate the object
			angle = Quaternion.Angle (grabbedObject.transform.rotation, WhereToHoldObject.rotation);

			//if the angle between our two rotations is less than 1, then we know we are OK to let the player manipulate the object
			if (angle < 1)
			{
				//grabbedObject.transform.position = WhereToHoldObject.position;
				//change the state of our game. HaveObject is used to allow the user to manipulate objects.
				hitObject = false;
				haveObject = true;
			}
		} 
		//Setup the logic for when our player is in posession of an object. This only happens after the HitObject functionality runs.
		else if (haveObject) 
		{
			//Once we have an object, we need that object to send out its own raycast. This is used to try and prevent clipping.
			//This still needs some extra work since there is still some clipping happening. Not sure this is the best way to handle clipping.
			if(Physics.Raycast (grabbedObject.transform.position, grabbedObject.transform.forward, out grabbedObjectRay))
			{
				Debug.DrawRay(grabbedObject.transform.position, grabbedObject.transform.forward, Color.red);
			}

			//The following logic is used to try and prevent clipping. Again, not working ideally.
			/*Determine if the grabbed object's forward position is greater than the point that our raycast is hitting.
			 * The raycast will only be hitting a point if it's hitting a collider so the below if statement should only execute
			 * if our grabbed object is passing through a collider.
			 */
			if (grabbedObject.transform.position.z > grabbedObjectRay.point.z) 
			{
				//if we are clipping, force the grabbed object to interpolate back to our holding area.
				grabbedObject.transform.position = Vector3.Lerp (grabbedObjectRay.point, WhereToHoldObject.position, Time.deltaTime * 3);
				//if the grabbed object is getting pushed too far ahead of the holding area, then force its position back onto the holding area.
				if(grabbedObject.transform.position.z < WhereToHoldObject.position.z){
					grabbedObject.transform.position = WhereToHoldObject.position;
				}
			}
			/*if(grabbedObjectBounds.Intersects(clippingBounds)){
				Debug.LogWarning ("BOUNDS INTERSECT WITH" + clippingBounds.extents);
				//grabbedObject.transform.position = Vector3.Lerp (grabbedObjectRay.point, WhereToHoldObject.position, Time.deltaTime * 3);
				grabbedObject.transform.position = WhereToHoldObject.position;
			}*/


			//When we are in posession of an object, allow the player to Manipulate Objects.
			ManipulateObject ();
		}

		//Logic for Push Power
		if(Input.GetKey(KeyCode.Mouse1) && currentStamina >= 0.0f && Time.time >= nextCanPushTime)
		{
			PushObject ();
		}

		else if(Input.GetKeyUp(KeyCode.Mouse1))
		{
			pushParticle.SetActive(false);
			if(pushedObject != null)
			{
				if(pushedObject.constraints != RigidbodyConstraints.None)
					pushedObject.constraints = RigidbodyConstraints.None;
			}

			if(currentObject != null)
				currentObject.SendMessage("BeingPushed", false);
		}

		else if(currentStamina <= 0.0f)
		{
			nextCanPushTime = Time.time + pushPenaltyTime;
			pushParticle.SetActive(false);
			if(pushedObject != null)
			{
				if(pushedObject.constraints != RigidbodyConstraints.None)
					pushedObject.constraints = RigidbodyConstraints.None;
			}

			if(currentObject != null)
				currentObject.SendMessage("BeingPushed", false);
		}
	}
	void ShootWeapon()
	{

		//Logic for letting the player shoot. If they don't have an object, we need to let them shoot the PsyBullet
		if (!haveObject) 
		{

			//Setup the projectile's rotation so it matches the camera's forward and upward vectors.
			projectileRotation = Quaternion.LookRotation (camera.forward, camera.up);

			//Create a PsyBullet Object at the spawnpoint's position using the projectileRotation variable.
			clone = (Rigidbody)Instantiate (projectile, spawnpoint.position, projectileRotation); 

			//set the clone's velocity to wherever our target location is minus the position of our spawnpoint.
			//This should send our projectile to wherever our player is looking.
			clone.velocity = (targetLocation - spawnpoint.position).normalized * speed;

			//gameObject.SendMessageUpwards("PsyStamina", costToShoot);

		} 
		//If our player HAS an object, we need to setup different behavior when player fires.
		else 
		{
			//Set the grabbed object non-kinematic. This is necessary because we force the object to kinematic in the EnableManipulation
			//script. By setting it back to non-kinematic, we ensure we can utilize physics and collision with the grabbed object.
			grabbedObject.isKinematic = false;

			//set the grabbedObject's velocity to wherever we're looking.
			grabbedObject.velocity = (targetLocation - WhereToHoldObject.position ).normalized * speed;

			gameObject.SendMessageUpwards ("PsyStamina", costToThrow);
			gameObject.SendMessageUpwards ("PsyHolding", false);


			grabbedObject.gameObject.layer = 0; //need to set our layer back to normal so raycasts can hit it

			//Force the grabbed Object back to the root of the heirarchy so it is not a child of anything.
			grabbedObject.transform.parent = originalTransform;

			//set our state back to not having an object.
			haveObject = false;
			canShoot = true;
		}
	}

	void ManipulateObject()
	{
		//This method is used to allow the player to use the Q and E keys to move the grabbed object's position along the z-axis while
		//they are in posesssion of an object.

		//If the user presses E and the grabbed object's position is not equal to the closest point we will allow the object to be
		if (Input.GetKey (KeyCode.E) & grabbedObject.transform.position != WhereToHoldObject.position) 
		{
			//interpolate Grabbed Object's position towards the closest position
			grabbedObject.transform.position = Vector3.Lerp (grabbedObject.transform.position, WhereToHoldObject.position, Time.deltaTime * 2);
		} 
		//If player presses Q and the grabbed object's position doesn't equal the farthest distance we will allow the object to be
		else if (Input.GetKey (KeyCode.Q) & grabbedObject.transform.position != MaxObjectDistance.position) 
		{
			//Interpolate grabbed object's position towards the max distance
			grabbedObject.transform.position = Vector3.Lerp (grabbedObject.transform.position, MaxObjectDistance.position, Time.deltaTime * 2);
		}
	}

	void PushObject()
	{

		pushParticle.SetActive(true);
		pushParticle.transform.LookAt(targetLocation);
		//pushLine.enabled = true;
		//pushLine.material.mainTextureOffset = new Vector2(0, Time.time);
		
		//pushRay = new Ray(spawnpoint.position, targetLocation - spawnpoint.position);
		Debug.DrawRay(spawnpoint.position, (targetLocation - spawnpoint.position) * rayLength, Color.red);
		
		//pushLine.SetPosition(0, spawnpoint.position); 
		//pushLine.SetPosition(1, pushRay.GetPoint (rayLength));
		
		if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out pushRaycast, rayLength))
		{


			if(pushRaycast.collider.tag == "CanManipulate")
			{
				//Debug.Log ("TRUE");
				currentObject = pushRaycast.transform;
				currentObject.SendMessage("BeingPushed", true);
				pushedObject = pushRaycast.collider.attachedRigidbody;
				pushedObject.constraints = RigidbodyConstraints.FreezeRotation;
				pushRaycast.collider.attachedRigidbody.AddForce(Camera.main.transform.forward * pushSpeed);
			}


		}

		gameObject.SendMessageUpwards("PsyStamina", 2 * Time.deltaTime);


		//pushRotation = Quaternion.LookRotation (camera.forward, camera.up);


		//pushClone = (Rigidbody)Instantiate (pushProjectile, spawnpoint.position, pushRotation); 
		
		//set the clone's velocity to wherever our target location is minus the position of our spawnpoint.
		//This should send our projectile to wherever our player is looking.
		//pushClone.velocity = (targetLocation - spawnpoint.position).normalized * pushSpeed;
	}

	void SetHitObjectBool(bool objectHit)
	{
		hitObject = objectHit;
	}
	void SetGrabbedObject (Rigidbody objectGrabbed)
	{
		grabbedObject = objectGrabbed;
	}
	void SetGrabbedBounds(Bounds objectBounds)
	{
		grabbedObjectBounds = objectBounds;
	}
	void SetClippingBounds(Bounds clipBounds)
	{
		clippingBounds = clipBounds;
		Debug.Log (clippingBounds.extents);
	}

	void GetStamina(float stamina)
	{
		currentStamina = stamina;
	}
}

