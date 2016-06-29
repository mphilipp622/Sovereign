using UnityEngine;
using System.Collections;

public class DestroyPlayerButton : MonoBehaviour {

	public GameObject Camera;

	void update(){
		if (Input.GetKeyDown("k"))
			
		//	Destroy (gameObject, 1.5f);
		Camera.SetActive (true);	
	}
}