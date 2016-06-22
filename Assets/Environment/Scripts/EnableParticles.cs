using UnityEngine;
using System.Collections;

public class EnableParticles : MonoBehaviour {

	private ParticleSystem psyControlParticle;
	bool isPushed = false;
	Transform originalParent;

	void Start () 
	{
		psyControlParticle = GetComponentInChildren<ParticleSystem> ();
		originalParent = transform.parent;
		psyControlParticle.gameObject.SetActive (false);
	}

	void Update () 
	{
		Debug.Log(transform.parent);
		if(transform.parent.tag == "MainCamera" || isPushed)
			psyControlParticle.gameObject.SetActive (true);
		else if ( transform.parent == originalParent || !isPushed )
			psyControlParticle.gameObject.SetActive (false);
	}

	public void BeingPushed( bool pushed )
	{
		isPushed = pushed;
	}
}
