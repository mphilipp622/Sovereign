using UnityEngine;
using System.Collections;

public class AimAndShoot : MonoBehaviour {

	public Rigidbody projectile, clone;
	public float tension, maxTension = 100.0f, projectileSpeed;
	public float maxDrawTime = 3.0f, drawTime = 0.0f;
	public Transform spawnpoint, camera;
	private Quaternion projectileRotation;
	private Quaternion bowRotation;
	private Vector3 targetLocation;
	RaycastHit whereToShoot;

	// Use this for initialization
	void Start () {

		whereToShoot = new RaycastHit ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Physics.Raycast (camera.position, camera.forward, out whereToShoot)) {
			targetLocation = whereToShoot.point;
			Debug.Log ("Where to Shoot" + whereToShoot);
		}
		//Debug.DrawRay (camera.position, camera.forward, Color.blue);
		if (Input.GetButton ("Fire1")) {
			drawTime += Time.deltaTime / maxDrawTime;
			tension = Mathf.Lerp (0, maxTension, drawTime);
		} else if (Input.GetButtonUp ("Fire1")) {
			ShootWeapon ();
			tension = 0;
			drawTime = 0;
		}
	}
	void ShootWeapon(){

		projectileRotation = Quaternion.LookRotation (camera.forward, camera.up) * projectile.rotation;
		clone = (Rigidbody) Instantiate (projectile, spawnpoint.position, projectileRotation);

		clone.velocity = (targetLocation - spawnpoint.position).normalized * tension;

	}

	void ChangeWeapon(){

	}
}
