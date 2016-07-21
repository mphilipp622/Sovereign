using UnityEngine;
using System.Collections;

public class PushCollider : MonoBehaviour {

	[SerializeField]
	float pushForce = 35f;

	void Start () 
	{
		
	}

	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "CanManipulate")
		{
			col.attachedRigidbody.AddForce(Camera.main.transform.forward * pushForce, ForceMode.Impulse);
		}
	}
//
//	void OnCollisionEnter(Collision col)
//	{
//		if(col.gameObject.tag == "CanManipulate")
//		{
//			col.collider.attachedRigidbody.AddForce(transform.parent.forward * 10f, ForceMode.Impulse);
//		}
//	}
}
