using UnityEngine;
using System.Collections;

public class DestroyPsyParticle : MonoBehaviour {
	private float particleStartTime;
	private ParticleSystem explosionParticle;

	// Use this for initialization
	void Start () {
		//Grab the time of the game when this particle is created.
		explosionParticle = GetComponent<ParticleSystem> ();
		particleStartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		//If current game time is greater than or equal to the Particle's Start Time plus the Particle's Lifetime, then we know we can destroy it.
		if (Time.time >= particleStartTime + explosionParticle.startLifetime)
			Destroy (gameObject);
	}
}
