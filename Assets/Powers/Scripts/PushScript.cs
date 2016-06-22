using UnityEngine;
using System.Collections;

public class PushScript : MonoBehaviour {

	Rigidbody projectile;
	float startTime;
	Vector3 startPos;

	void Start () 
	{
		projectile = GetComponent<Rigidbody>();
		startTime = Time.time;
		startPos = transform.position;
	}

	void Update () 
	{
		if(Time.time >= startTime + 1.0f)
			Destroy(gameObject);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "CanManipulate")
		{
			collision.rigidbody.velocity = projectile.velocity;
		}
		Destroy (gameObject);
	}
}
