using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour {
	
	private float stamina;
	private Slider staminaBar;
	private Material progress;

	void Start () {
		progress = gameObject.GetComponent<Image> ().material;
	}

	void Update () {
		//progress.mainTextureOffset = new Vector2(2, 0);
	}
}