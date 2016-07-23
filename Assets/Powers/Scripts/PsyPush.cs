using UnityEngine;
using System.Collections;
using UnityEditor;

public class PsyPush : MonoBehaviour {

	Ray powerPath;

	[SerializeField]
	Rigidbody powerRB, clone;

	[SerializeField]
	Transform spawnpoint, particleTransform, particleClone;

	[SerializeField]
	float projectileSpeed = 80, maxChargeTime = 1.0f, chargeTime = 0.0f, heldDown = 0.0f;

	[SerializeField]
	bool canShoot = true;
	bool psyCharging = false;

	PsyGrab grabScript;

	public bool CanShoot
	{
		get
		{
			return canShoot;
		}
	}

	[SerializeField]
	float costToShoot = 1.0f;

	[SerializeField]
	ParticleSystem pushParticles;

	[SerializeField]
	AudioClip powerHoldClip;
	[SerializeField]
	AudioClip powerChargeClip;
	AudioSource powerAudio;

	vp_FPController controller;
	vp_FPCamera camera;
	vp_FPInput mouseLook;

	Quaternion projectileRotation;

	Transform maxPushTransform;

	ParticleSystem powerParticleMain;
	SerializedObject powerParticle;
	Transform chargeParticles;
	float startingParticleShape, startSpeedParticle;

	void Start () 
	{
		grabScript = GetComponent<PsyGrab>();
		spawnpoint = transform.FindChild("Spawnpoint");
		powerAudio = transform.GetComponent<AudioSource> ();
		chargeParticles = transform.FindChild("ChargeParticles");
		powerParticleMain = chargeParticles.GetComponent<ParticleSystem>();
		pushParticles = transform.FindChild("PushParticleNew").GetComponent<ParticleSystem>();
		powerParticle = new SerializedObject (powerParticleMain);
		powerParticle.ApplyModifiedProperties();
		startingParticleShape = powerParticle.FindProperty ("ShapeModule.radius").floatValue = 4.18f;
		startSpeedParticle = powerParticleMain.startSpeed;
		chargeParticles.position = spawnpoint.position;
		chargeParticles.gameObject.SetActive(false);
		controller = GameObject.FindWithTag("Player").GetComponent<vp_FPController>();
		camera = GameObject.FindWithTag("MainCamera").GetComponent<vp_FPCamera>();
		mouseLook = GameObject.FindWithTag("Player").GetComponent<vp_FPInput>();

		maxPushTransform = GameObject.Find("MaxPushDistance").transform;

	}

	void Update () 
	{
		powerPath = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

		if(Input.GetButtonDown("Fire2") && canShoot && StaminaManager.SM.stamina >= costToShoot && grabScript.CanShoot)
		{
			controller.ResetState(); // Make sure speed doesn't slow down when right-clicking
			mouseLook.ResetState();
			camera.ResetState(); // Make sure camera doesn't zoom when right-clicking.
			StartCoroutine(Push());
		}


	}

	IEnumerator Push()
	{
		canShoot = false;
		while(Input.GetButton("Fire2") && chargeTime < maxChargeTime)
		{
			
			controller.ResetState(); // Make sure speed doesn't slow down when right-clicking
			mouseLook.ResetState();
			camera.ResetState(); // Make sure camera doesn't zoom when right-clicking.
			chargeTime += Time.deltaTime;//Set chargeTime to equal one second of game time. This executes continuously as long as user is holding down mouse button
			psyCharging = true;
			StaminaManager.SM.CanCharge(false);
//			if(powerAudio.clip != powerChargeClip)
//				powerAudio.clip = powerChargeClip;
//			powerAudio.Play();

			if(chargeTime < maxChargeTime / 2f)
				Camera.main.fieldOfView = Mathf.Lerp (60, 70, chargeTime / maxChargeTime);
			else if(chargeTime > maxChargeTime / 2)
				Camera.main.fieldOfView = Mathf.Lerp (70, 60, chargeTime / maxChargeTime);
			chargeParticles.gameObject.SetActive(true);
			powerParticle.FindProperty("ShapeModule.radius").floatValue = Mathf.Lerp (startingParticleShape, 1.0f, chargeTime / maxChargeTime);
			powerParticle.ApplyModifiedProperties();
			powerParticleMain.startSpeed = Mathf.Lerp (startSpeedParticle, -0.1f, chargeTime / maxChargeTime);

			//tell Character Controller to reduce stamina from 0 - the cost of the power over the course of how long it takes us to fully charge an attack
			StaminaManager.SM.PsyStamina(Mathf.Lerp (0, costToShoot, Time.deltaTime / maxChargeTime));

//			else if(chargeTime > maxChargeTime)
//			{
//				if(powerAudio.clip != powerHoldClip)
//					powerAudio.clip = powerHoldClip;
//				if(!powerAudio.isPlaying)
//				{
//					powerAudio.loop = true;
//					powerAudio.Play();
//				}
//			}

			yield return null;
		}

		if (chargeTime < maxChargeTime)
		{
			powerAudio.Stop ();
			powerAudio.clip = powerChargeClip;
			chargeParticles.gameObject.SetActive(false);
			chargeTime = 0; //reset chargeTime
			psyCharging = false;
			canShoot = true;
			StaminaManager.SM.SetRechargeTime();
			StaminaManager.SM.CanCharge(true);
		}


		while(chargeTime >= maxChargeTime)
		{
			controller.ResetState(); // Make sure speed doesn't slow down when right-clicking
			camera.ResetState(); // Make sure camera doesn't zoom when right-clicking.
			mouseLook.ResetState();

			if (Input.GetButtonUp ("Fire2"))
			{
				pushParticles.Play();
				StaminaManager.SM.PsyStamina(costToShoot);
				yield return null;
				clone = (Rigidbody)Instantiate (powerRB, spawnpoint.position, projectileRotation); 
				while(clone.transform.position.z < maxPushTransform.position.z)
				{
					clone.transform.position += powerPath.direction * Time.deltaTime * projectileSpeed;
					yield return null;
				}
				Destroy(clone.gameObject);

				canShoot = true;

				powerAudio.Pause ();

				chargeParticles.gameObject.SetActive(false);
				chargeTime = 0; //this is necessary so the above logic for charging works properly.
				psyCharging = false;
				StaminaManager.SM.SetRechargeTime();
				StaminaManager.SM.CanCharge(true);

			}
			yield return null;
		}
		yield return null;
	}
}
