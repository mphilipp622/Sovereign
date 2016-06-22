using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture2D crosshair;
	public float crosshairScale = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.DrawTexture(new Rect((Screen.width-crosshair.width*crosshairScale)/2 ,(Screen.height-crosshair.height*crosshairScale)/2, crosshair.width*crosshairScale, crosshair.height*crosshairScale),crosshair);
	}
}
