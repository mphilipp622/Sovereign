using UnityEngine;
using System.Collections;

public class EnableManipulation : MonoBehaviour {

	public Transform playerTransform;
	public Transform Spawnpoint, WhereToHoldObject;
	public ParticleSystem psyParticles;
	private Transform parentTransform;
	// Use this for initialization
	void Start () {
		//parentTransform = Camera.main.transform.FindChild ("PsyPower").transform;
		parentTransform = Camera.main.transform;
		//parentTransform = Camera.main.transform.FindChild ("PsyPower").FindChild("EmptyParent").transform;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "CanManipulate") {

			GameObject.Find ("PsyPower").GetComponent<AimAndShootPsy>().originalTransform = collision.gameObject.transform.parent;
			collision.rigidbody.isKinematic = true;
			collision.gameObject.transform.SetParent (parentTransform);

			//collision.gameObject.SendMessageUpwards("SetGrabbedObject", collision.rigidbody, SendMessageOptions.RequireReceiver);
			//collision.gameObject.SendMessageUpwards("SetHitObjectBool", true, SendMessageOptions.RequireReceiver);
			GameObject.Find ("PsyPower").GetComponent<AimAndShootPsy>().hitObject = true;
			GameObject.Find ("PsyPower").GetComponent<AimAndShootPsy>().grabbedObject = collision.rigidbody;
			//collision.gameObject.SendMessageUpwards("SetGrabbedBounds", collision.collider.bounds, SendMessageOptions.RequireReceiver);
		}
		ExplodeParticle ();
		Destroy(gameObject);


	}

	void ExplodeParticle(){
		//Will be used for making the object look like it explodes
		Instantiate (psyParticles, gameObject.transform.position, Quaternion.identity);

		//psyParticles.transform.position = gameObject.transform.position;
		psyParticles.Play ();
		//Debug.Log ("System Playing");
	}
}
