using UnityEngine;
using System.Collections;

public class PsyPush : MonoBehaviour {

	Ray powerPath;

	[SerializeField]
	Rigidbody powerRB, clone;

	[SerializeField]
	Transform spawnpoint, particleTransform, particleClone;

	[SerializeField]
	float projectileSpeed = 80;

	[SerializeField]
	bool canShoot = true;

	[SerializeField]
	float costToShoot = 1.0f;

	[SerializeField]
	ParticleSystem pushParticles;

	vp_FPController controller;
	vp_FPCamera camera;

	Quaternion projectileRotation;

	void Start () 
	{
		spawnpoint = transform.FindChild("Spawnpoint");
		pushParticles = transform.FindChild("PushParticleNew").GetComponent<ParticleSystem>();
		controller = GameObject.FindWithTag("Player").GetComponent<vp_FPController>();
		camera = GameObject.FindWithTag("MainCamera").GetComponent<vp_FPCamera>();

//		pushParticles.transform.position = spawnpoint.position;
//		pushParticles.Stop();
	}

	void Update () 
	{
		powerPath = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

		if(Input.GetButtonDown("Fire2") && canShoot)
		{
			controller.ResetState(); // Make sure speed doesn't slow down when right-clicking
			camera.ResetState(); // Make sure camera doesn't zoom when right-clicking.
			Push();
		}
	}

	void Push()
	{
		canShoot = false;
		projectileRotation = Quaternion.LookRotation (Camera.main.transform.forward, Camera.main.transform.up);
//		pushParticles.Stop();
		pushParticles.Play();
		StaminaManager.SM.PsyStamina(costToShoot);
		//particleClone = (Transform)Instantiate(particleTransform, spawnpoint.position, Quaternion.identity);
		//clone = (Rigidbody)Instantiate (powerRB, spawnpoint.position, projectileRotation); 
		//clone.velocity = powerPath.direction * projectileSpeed;
		canShoot = true;
	}
}
