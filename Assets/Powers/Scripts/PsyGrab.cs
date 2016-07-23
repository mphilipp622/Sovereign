using UnityEngine;
using System.Collections;
using UnityEditor;

public class PsyGrab : MonoBehaviour {

	Ray powerPath;
	RaycastHit grabbedObjectRay, outlineRay;

	[SerializeField]
	Rigidbody powerRB, clone, grabbedObject;

	public Rigidbody GrabbedObject
	{
		get
		{
			return grabbedObject;
		}
		set
		{
			grabbedObject = value;
		}
	}

	[SerializeField]
	Transform spawnpoint, WhereToHoldObject, originalTransform;

	public Transform OriginalTransform
	{
		get
		{
			return originalTransform;
		}
		set
		{
			originalTransform = value;
		}
	}

	[SerializeField]
	float projectileSpeed = 80, maxChargeTime = 2.0f, chargeTime = 0.0f, heldDown = 0.0f;

	[SerializeField]
	bool haveObject = false, hitObject = false, canShoot = true, canThrow = false;

	public bool HitObject
	{
		get
		{
			return hitObject;
		}
		set
		{
			hitObject = value;
		}
	}

	bool psyHolding = false, psyCharging = false;

	[SerializeField]
	float costToShoot = 1.0f, costToThrow = 1.0f, costToPush = 1.0f, currentStamina;

	float angle;

	//These variables are used for particle settings
	ParticleSystem powerParticleMain, grabParticleMain;
	SerializedObject powerParticle;
	Transform chargeParticles;
	float startingParticleShape, startSpeedParticle;

	Quaternion projectileRotation;

	[SerializeField]
	AudioClip powerHoldClip;
	[SerializeField]
	AudioClip powerChargeClip;
	AudioSource powerAudio;

	bool isOutlined;
	GameObject outlinedObject;

	[SerializeField]
	float glowMax = 5f, flickerMinimum = 1f, glowSpeed = 6f;

	void Start () 
	{

		spawnpoint = transform.FindChild("Spawnpoint");

		powerAudio = transform.GetComponent<AudioSource> ();

		chargeParticles = transform.FindChild("ChargeParticles");
		powerParticleMain = chargeParticles.GetComponent<ParticleSystem>();
		grabParticleMain = transform.FindChild("GrabParticles").GetComponent<ParticleSystem>();
		powerParticle = new SerializedObject (powerParticleMain);
		startingParticleShape = powerParticle.FindProperty ("ShapeModule.radius").floatValue = 4.18f;
		powerParticle.ApplyModifiedProperties();
		startSpeedParticle = powerParticleMain.startSpeed;
		chargeParticles.position = spawnpoint.position;
		chargeParticles.gameObject.SetActive(false);
		isOutlined = false;
		outlinedObject = null;
		//powerParticleMain = transform.FindChild(

	}

	void Update () 
	{
		powerPath = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

		if(Physics.Raycast(powerPath, out outlineRay, 20f))
		{
			if(outlineRay.collider.tag == "CanManipulate" && !isOutlined && !haveObject)
			{
				outlinedObject = outlineRay.collider.gameObject;
				StartCoroutine(OutlineObject(outlineRay.collider.gameObject.GetComponent<Renderer>()));
//				outlineRay.collider.gameObject.GetComponent<Renderer>().material.SetFloat("_MKGlowTexStrength", );
			}
			else if(outlineRay.collider.tag == "CanManipulate" && isOutlined)
			{
				if(Input.GetButtonDown("Fire1") && canShoot && StaminaManager.SM.stamina > costToShoot)
				{
					canShoot = false;
					psyHolding = true;
					StaminaManager.SM.CanCharge(false);
					grabParticleMain.Play();

					OriginalTransform = outlineRay.collider.gameObject.transform.parent;
					outlineRay.rigidbody.isKinematic = true;
					outlineRay.collider.gameObject.transform.SetParent (transform);

					HitObject = true;
					GrabbedObject = outlineRay.rigidbody;

//					grabbedObject.gameObject.layer = 2; //This is necessary to make sure our raycast goes through our object that we are holding.

					//Interpolate the position and rotation of the grabbed object to the holding area's position and rotation.
					grabbedObject.transform.position = Vector3.Lerp (grabbedObject.transform.position, spawnpoint.position, Time.deltaTime * 5);
					grabbedObject.transform.rotation = Quaternion.Slerp (grabbedObject.transform.rotation, spawnpoint.rotation, Time.deltaTime * 5);

					//Setup an Angle variable that will determine the angle between our grabbed object's rotation and our holding area's rotation.
					//This is used to determine when to stop our interpolation and allow the player to manipulate the object
					angle = Quaternion.Angle (grabbedObject.transform.rotation, spawnpoint.rotation);

					//if the angle between our two rotations is less than 1, then we know we are OK to let the player manipulate the object
					if (angle < 1)
					{
						//change the state of our game. HaveObject is used to allow the user to manipulate objects.
						haveObject = true;
					}
				}
			}
			if(outlinedObject != outlineRay.collider.gameObject && canShoot)
			{
				isOutlined = false;
			}
		}

//		Debug.DrawRay(powerPath.origin, powerPath.direction * 20f, Color.red, 1f);

		//If user Presses Left Mouse and They are able to shoot
		if (Input.GetButton ("Fire1") && canShoot && StaminaManager.SM.stamina >= 0) 
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
			psyCharging = true;
			StaminaManager.SM.CanCharge(false);
			//gameObject.SendMessageUpwards("PsyCharging", true); //tell our Character controller that we are Charging our power
			//find out if we have not yet reached our maximum charge time so that we can calculate the stamina reduction
			if(chargeTime < maxChargeTime)
			{
				if(chargeTime < 1.0f)
					Camera.main.fieldOfView = Mathf.Lerp (60, 70, chargeTime / maxChargeTime);
				else if(chargeTime > 1.0f)
					Camera.main.fieldOfView = Mathf.Lerp (70, 60, chargeTime / maxChargeTime);
				chargeParticles.gameObject.SetActive(true);
				powerParticle.FindProperty("ShapeModule.radius").floatValue = Mathf.Lerp (startingParticleShape, 1.0f, chargeTime / maxChargeTime);
				powerParticle.ApplyModifiedProperties();
				powerParticleMain.startSpeed = Mathf.Lerp (startSpeedParticle, -0.1f, chargeTime / maxChargeTime);

				//gameObject.SendMessageUpwards("Charging", true, SendMessageOptions.RequireReceiver);
				//gameObject.SendMessageUpwards ("ChargeTime", chargeTime, SendMessageOptions.RequireReceiver);

				//tell Character Controller to reduce stamina from 0 - the cost of the power over the course of how long it takes us to fully charge an attack
				//gameObject.SendMessageUpwards("PsyStamina", Mathf.Lerp (0, costToShoot, Time.deltaTime / maxChargeTime)); 
				StaminaManager.SM.PsyStamina(Mathf.Lerp (0, costToShoot, Time.deltaTime / maxChargeTime));
			}
			else if(chargeTime > maxChargeTime)
			{
				if(powerAudio.clip != powerHoldClip)
					powerAudio.clip = powerHoldClip;
				if(!powerAudio.isPlaying)
				{
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
			chargeParticles.gameObject.SetActive(false);
			chargeTime = 0; //this is necessary so the above logic for charging works properly.
			psyCharging = false;
			StaminaManager.SM.SetRechargeTime();
			StaminaManager.SM.CanCharge(true);
			//gameObject.SendMessageUpwards("PsyCharging", false); //tell our Character Controller that we are no longer charging our power.
		} 
		//is player letting up on mouse and have we NOT reached the required charge time? We will NOT fire a PsyBullet
		else if (Input.GetButtonUp ("Fire1") && chargeTime < maxChargeTime)
		{
			//cam.fieldOfView = Mathf.Lerp (70, 60, chargeTime);
			powerAudio.Stop ();
			powerAudio.clip = powerChargeClip;
			chargeParticles.gameObject.SetActive(false);
			chargeTime = 0; //reset chargeTime
			psyCharging = false;
			StaminaManager.SM.SetRechargeTime();
			StaminaManager.SM.CanCharge(true);
			//gameObject.SendMessageUpwards("PsyCharging", false); //Tell Character Controller we're no longer charging
			//			SendMessageUpwards ("ChangeDamping", 0f);
		}
		//is player letting go of mouse button and do they have an object currently?
		else if (Input.GetButtonDown ("Fire1") && haveObject)
			ShootWeapon (); //fire the object

		//make sure camera is returned to original position when user is not firing
//		if (chargeTime == 0) 
//		{
//			//			cameraParent.transform.localPosition = new Vector3 (0, 0, 0);
//		}
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
			psyHolding = true;
			StaminaManager.SM.CanCharge(false);
//			gameObject.SendMessageUpwards("PsyHolding", true);

			grabbedObject.gameObject.layer = 2; //This is necessary to make sure our raycast goes through our object that we are holding.

			//Interpolate the position and rotation of the grabbed object to the holding area's position and rotation.
//			grabbedObject.transform.position = Vector3.Lerp (grabbedObject.transform.position, WhereToHoldObject.position, Time.deltaTime * 5);
			grabbedObject.transform.position = Vector3.Lerp (grabbedObject.transform.position, spawnpoint.position, Time.deltaTime * 5);
//			grabbedObject.transform.rotation = Quaternion.Slerp (grabbedObject.transform.rotation, WhereToHoldObject.rotation, Time.deltaTime * 5);
			grabbedObject.transform.rotation = Quaternion.Slerp (grabbedObject.transform.rotation, spawnpoint.rotation, Time.deltaTime * 5);

			//Setup an Angle variable that will determine the angle between our grabbed object's rotation and our holding area's rotation.
			//This is used to determine when to stop our interpolation and allow the player to manipulate the object
//			angle = Quaternion.Angle (grabbedObject.transform.rotation, WhereToHoldObject.rotation);
			angle = Quaternion.Angle (grabbedObject.transform.rotation, spawnpoint.rotation);


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
//				grabbedObject.transform.position = Vector3.Lerp (grabbedObjectRay.point, WhereToHoldObject.position, Time.deltaTime * 3);
				grabbedObject.transform.position = Vector3.Lerp (grabbedObjectRay.point, spawnpoint.position, Time.deltaTime * 3);

				//if the grabbed object is getting pushed too far ahead of the holding area, then force its position back onto the holding area.
//				if(grabbedObject.transform.position.z < WhereToHoldObject.position.z){
//					grabbedObject.transform.position = WhereToHoldObject.position;
//				}
				if(grabbedObject.transform.position.z < spawnpoint.position.z)
					grabbedObject.transform.position = spawnpoint.position;
				
			}
			/*if(grabbedObjectBounds.Intersects(clippingBounds)){
				Debug.LogWarning ("BOUNDS INTERSECT WITH" + clippingBounds.extents);
				//grabbedObject.transform.position = Vector3.Lerp (grabbedObjectRay.point, WhereToHoldObject.position, Time.deltaTime * 3);
				grabbedObject.transform.position = WhereToHoldObject.position;
			}*/


			//When we are in posession of an object, allow the player to Manipulate Objects.
//			ManipulateObject ();
		}

		//Logic for Push Power
//		if(Input.GetKey(KeyCode.Mouse1) && currentStamina >= 0.0f && Time.time >= nextCanPushTime)
//		{
//			PushObject ();
//		}
//
//		else if(Input.GetKeyUp(KeyCode.Mouse1))
//		{
//			pushParticle.SetActive(false);
//			if(pushedObject != null)
//			{
//				if(pushedObject.constraints != RigidbodyConstraints.None)
//					pushedObject.constraints = RigidbodyConstraints.None;
//			}
//
//			if(currentObject != null)
//				currentObject.SendMessage("BeingPushed", false);
//		}
//
//		else if(currentStamina <= 0.0f)
//		{
//			nextCanPushTime = Time.time + pushPenaltyTime;
//			pushParticle.SetActive(false);
//			if(pushedObject != null)
//			{
//				if(pushedObject.constraints != RigidbodyConstraints.None)
//					pushedObject.constraints = RigidbodyConstraints.None;
//			}
//
//			if(currentObject != null)
//				currentObject.SendMessage("BeingPushed", false);
//		}
	}
	void ShootWeapon()
	{

		//Logic for letting the player shoot. If they don't have an object, we need to let them shoot the PsyBullet
		if (!haveObject) 
		{

			//Setup the projectile's rotation so it matches the camera's forward and upward vectors.
			projectileRotation = Quaternion.LookRotation (Camera.main.transform.forward, Camera.main.transform.up);

			//Create a PsyBullet Object at the spawnpoint's position using the projectileRotation variable.
			clone = (Rigidbody)Instantiate (powerRB, spawnpoint.position, projectileRotation); 

			//set the clone's velocity to wherever our target location is minus the position of our spawnpoint.
			//This should send our projectile to wherever our player is looking.
//			clone.velocity = (powerRB - spawnpoint.position).normalized * projectileSpeed;
			clone.velocity = powerPath.direction * projectileSpeed;

			//gameObject.SendMessageUpwards("PsyStamina", costToShoot);

		} 
		//If our player HAS an object, we need to setup different behavior when player fires.
		else 
		{
			//Set the grabbed object non-kinematic. This is necessary because we force the object to kinematic in the EnableManipulation
			//script. By setting it back to non-kinematic, we ensure we can utilize physics and collision with the grabbed object.
			grabbedObject.isKinematic = false;

			//set the grabbedObject's velocity to wherever we're looking.
//			grabbedObject.velocity = (targetLocation - WhereToHoldObject.position ).normalized * speed;
			grabbedObject.velocity = powerPath.direction * projectileSpeed;

			StaminaManager.SM.PsyStamina(costToThrow);
			//gameObject.SendMessageUpwards ("PsyStamina", costToThrow);
			psyHolding = false;
			StaminaManager.SM.CanCharge(true);
			//gameObject.SendMessageUpwards ("PsyHolding", false);


			grabbedObject.gameObject.layer = 0; //need to set our layer back to normal so raycasts can hit it

			//Force the grabbed Object back to the root of the heirarchy so it is not a child of anything.
			grabbedObject.transform.parent = originalTransform;

			//set our state back to not having an object.
			haveObject = false;
			canShoot = true;
		}
	}

	IEnumerator OutlineObject(Renderer _rend)
	{
		isOutlined = true;

		float glowStrength = 0;
		while(glowStrength < glowMax)
		{
			_rend.material.SetFloat("_MKGlowTexStrength", glowStrength);
			glowStrength += Time.deltaTime * glowSpeed;
			yield return null;
		}

		//Execute while object is outlined
		while(isOutlined)
		{
			
			while(glowStrength > flickerMinimum)
			{
				if(!isOutlined)
					break;
				
				_rend.material.SetFloat("_MKGlowTexStrength", glowStrength);
				glowStrength -= Time.deltaTime * glowSpeed;
				yield return null;
			}

			while(glowStrength < glowMax)
			{
				if(!isOutlined)
					break;
				
				_rend.material.SetFloat("_MKGlowTexStrength", glowStrength);
				glowStrength += Time.deltaTime * glowSpeed;
				yield return null;
			}

			yield return null;
		}
			
		while(glowStrength > 0f)
		{
			_rend.material.SetFloat("_MKGlowTexStrength", glowStrength);
			glowStrength -= Time.deltaTime * glowSpeed;
			yield return null;
		}

	}
}
