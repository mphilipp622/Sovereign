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
	float projectileSpeed = 80;

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

	public bool CanShoot
	{
		get
		{
			return canShoot;
		}
	}

	bool psyHolding = false;

	[SerializeField]
	float costToShoot = 1.0f, costToThrow = 1.0f, currentStamina;

	float angle;

	//These variables are used for particle settings
	ParticleSystem grabParticleMain;
	Transform chargeParticles;

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

	PsyPush pushScript;

	void Start () 
	{
		pushScript = GetComponent<PsyPush>();
		spawnpoint = transform.FindChild("Spawnpoint");

		powerAudio = transform.GetComponent<AudioSource> ();

		chargeParticles = transform.FindChild("ChargeParticles");
		grabParticleMain = transform.FindChild("GrabParticles").GetComponent<ParticleSystem>();
		chargeParticles.position = spawnpoint.position;
		chargeParticles.gameObject.SetActive(false);
		isOutlined = false;
		outlinedObject = null;

	}

	void Update () 
	{
		powerPath = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

		if(pushScript.CanShoot)
		{
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
		}
		else
			isOutlined = false;

		if (Input.GetButtonDown ("Fire1") && haveObject)
			ShootWeapon (); //fire the object

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

			grabbedObject.gameObject.layer = 2; //This is necessary to make sure our raycast goes through our object that we are holding.

			//Interpolate the position and rotation of the grabbed object to the holding area's position and rotation.
			grabbedObject.transform.position = Vector3.Lerp (grabbedObject.transform.position, spawnpoint.position, Time.deltaTime * 5);
			grabbedObject.transform.rotation = Quaternion.Slerp (grabbedObject.transform.rotation, spawnpoint.rotation, Time.deltaTime * 5);

			//Setup an Angle variable that will determine the angle between our grabbed object's rotation and our holding area's rotation.
			//This is used to determine when to stop our interpolation and allow the player to manipulate the object
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
			
	}
	void ShootWeapon()
	{

		//Logic for letting the player shoot. If they don't have an object, we need to let them shoot the PsyBullet
//		if (!haveObject) 
//		{
//			//Setup the projectile's rotation so it matches the camera's forward and upward vectors.
//			projectileRotation = Quaternion.LookRotation (Camera.main.transform.forward, Camera.main.transform.up);
//
//			//Create a PsyBullet Object at the spawnpoint's position using the projectileRotation variable.
//			clone = (Rigidbody)Instantiate (powerRB, spawnpoint.position, projectileRotation); 
//
//			//set the clone's velocity to wherever our target location is minus the position of our spawnpoint.
//			//This should send our projectile to wherever our player is looking.
//			clone.velocity = powerPath.direction * projectileSpeed;
//
//		} 
		//If our player HAS an object, we need to setup different behavior when player fires.
		if(haveObject) 
		{
			//Set the grabbed object non-kinematic. This is necessary because we force the object to kinematic in the EnableManipulation
			//script. By setting it back to non-kinematic, we ensure we can utilize physics and collision with the grabbed object.
			grabbedObject.isKinematic = false;

			//set the grabbedObject's velocity to wherever we're looking.
			grabbedObject.velocity = powerPath.direction * projectileSpeed;

			StaminaManager.SM.PsyStamina(costToThrow);
			psyHolding = false;
			StaminaManager.SM.CanCharge(true);


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
