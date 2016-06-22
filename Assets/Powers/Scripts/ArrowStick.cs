using UnityEngine;
using System.Collections;

public class ArrowStick : MonoBehaviour {

	private Rigidbody arrow;
	private Transform newParent;
	private float arrowStartTime;
	// Use this for initialization
	void Start () {
		arrow = GetComponent<Rigidbody> ();
		arrowStartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= arrowStartTime + 10)
			Destroy (gameObject);
	}

	void OnCollisionEnter(Collision collision){
		//make arrow stop
		/*

		arrow.freezeRotation = true;

		*/

		//newParent = collision.gameObject.GetComponentInParent <Transform>();
		arrow.isKinematic=true; // stop physics
		arrow.velocity = new Vector3 (0, 0, 0);
		arrow.freezeRotation = true;

		//arrow.useGravity = false;
		transform.parent = collision.transform.root;
		//transform.parent = collision.transform;
		//transform.SetParent (collision.transform);
		Debug.Log ("Arrow Hit");
		//Debug.Log (newParent);

	}
}
