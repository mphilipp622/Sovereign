using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadoutButtonsSecondaryGun : MonoBehaviour {
	
	public int SecondaryGunType = 0;
	public int minGunType= 0;
	public int maxGunType = 2;
	public Text Gun2Text = null;
	public GameObject PickUpPistolPrefab;
	public GameObject PickUpKnifePrefab;
	public Transform Player;
	
	public void NextSecondaryGun() {
		if (SecondaryGunType < maxGunType) {
			SecondaryGunType = SecondaryGunType +1;
		}
	}
	
	public void PreviousSecondaryGun() {
		if (SecondaryGunType > minGunType) {
			SecondaryGunType = SecondaryGunType - 1;
		}
	}

	public void StartGameSecondary() {

		if (SecondaryGunType == 1) {
			Instantiate(PickUpKnifePrefab, Player.position, Player.rotation);

		}

		if (SecondaryGunType == 2) {
			Instantiate(PickUpPistolPrefab, Player.position, Player.rotation);
		}
	}

	void Update (){ //Update is called once per frame
		if (SecondaryGunType == 1)
			Gun2Text.text = "Knife";
		//Debug.Log ("Changed Text to Knife");

		if (SecondaryGunType == 0)
			Gun2Text.text = "None";
		//Debug.Log ("Changed Text to None");

		if (SecondaryGunType == 2)
			Gun2Text.text = "Pistol";
		//Debug.Log ("Changed Text to Pistol");
	}
}
