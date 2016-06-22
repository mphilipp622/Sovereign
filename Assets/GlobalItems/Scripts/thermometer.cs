using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class thermometer : MonoBehaviour {


	public float CurrentTemp;
	public float TempDecriment=25.0f;
	public bool scorching= false;
	public bool freezing= true;
	public bool ideal= false;
	Text text;
	void Awake () {
		text = GetComponent <Text> ();
		CurrentTemp = -90.0f;
	}
	// Update is called once per frame
	void Update () {

		if(CurrentTemp > -100){
			CurrentTemp -= Time.deltaTime;
		}
		if(CurrentTemp <= -100){
			//Debug.Log("GAME OVER");
		}

		text.text = "Temp: " + CurrentTemp;
	}

}
