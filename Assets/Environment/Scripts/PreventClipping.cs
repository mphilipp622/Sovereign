using UnityEngine;
using System.Collections;

public class PreventClipping : MonoBehaviour {
	private Collider objectCollider;

	void Start () {
		objectCollider = GetComponent<Collider> ();
	}

	void Update () {
		//if (gameObject.transform.parent == null)
		//	objectCollider.isTrigger = false;
		if (gameObject.transform.parent != null) {
		//	objectCollider.isTrigger = true;
			gameObject.SendMessageUpwards("SetGrabbedBounds", objectCollider.bounds, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnCollisionEnter(Collision collision){
		if (gameObject.transform.parent != null) {
			gameObject.SendMessageUpwards ("SetClippingBounds", collision.collider.bounds, SendMessageOptions.DontRequireReceiver);

			Debug.Log("Colliding");
			
		}
	}
	void OnTriggerEnter(Collider collider){
		/*if (gameObject.transform.parent != null) {
			gameObject.SendMessageUpwards ("SetClippingBounds", collider.bounds, SendMessageOptions.DontRequireReceiver);
			gameObject.SendMessageUpwards("SetHitObjectBool", true, SendMessageOptions.RequireReceiver);
			Debug.Log("Trigger Colliding");
	
		}*/
	}
}
