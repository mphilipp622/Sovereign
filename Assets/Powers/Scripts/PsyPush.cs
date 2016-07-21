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

	Transform pushCollider, maxPushTransform;
//	Collider pushCollider;

	void Start () 
	{
		spawnpoint = transform.FindChild("Spawnpoint");
		pushParticles = transform.FindChild("PushParticleNew").GetComponent<ParticleSystem>();
		controller = GameObject.FindWithTag("Player").GetComponent<vp_FPController>();
		camera = GameObject.FindWithTag("MainCamera").GetComponent<vp_FPCamera>();
		pushCollider = transform.FindChild("PushCollider");
		pushCollider.gameObject.SetActive(false);
		maxPushTransform = GameObject.Find("MaxPushDistance").transform;
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
			StartCoroutine(Push());
		}
	}

	IEnumerator Push()
	{
		canShoot = false;
		projectileRotation = Quaternion.LookRotation (Camera.main.transform.forward, Camera.main.transform.up);
//		pushParticles.Stop();
		pushParticles.Play();
//		pushCollider.transform.localScale = new Vector3(pushCollider.transform.localScale.x, 0, pushCollider.transform.localScale.z);
//		pushCollider.gameObject.SetActive(true);
		StaminaManager.SM.PsyStamina(costToShoot);
		yield return null;
//		while(pushCollider.transform.localScale.y < 4f)
//		{
//			pushCollider.transform.localScale = new Vector3(pushCollider.transform.localScale.x, pushCollider.transform.localScale.y + Time.deltaTime * 4, pushCollider.transform.localScale.z);
//			yield return null;
//		}
		pushCollider.gameObject.SetActive(false);
		//particleClone = (Transform)Instantiate(particleTransform, spawnpoint.position, Quaternion.identity);
		clone = (Rigidbody)Instantiate (powerRB, spawnpoint.position, projectileRotation); 
		while(clone.transform.position.z < maxPushTransform.position.z)
		{
			clone.transform.position += powerPath.direction * Time.deltaTime * projectileSpeed;
//			clone.transform.position = Vector3.Lerp(clone.transform.position, powerPath.direction, Time.deltaTime);
			yield return null;
		}
		Destroy(clone.gameObject);
//		clone.velocity = powerPath.direction * projectileSpeed;
		canShoot = true;
	}
}
