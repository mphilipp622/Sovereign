using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadoutButtons : MonoBehaviour {
	
	public int GunType = 0;
	public int minGunType= 0;
	public int maxGunType = 2;
	public Text Gun1Text = null;
	public GameObject M4A1 = null;
	public GameObject Shotgun = null;
	public Transform player = null;
	
	public void NextGun() {
		if (GunType < maxGunType) {
			GunType = GunType +1;
		}
	}
	
	public void PreviousGun() {
		if (GunType > minGunType) {
			GunType = GunType - 1;
		}
	}

	public void StartGamePrimaryGun(){

		if (GunType == 1) {
			Instantiate(M4A1, player.position, player.rotation);
		}

		if (GunType == 2) {
			Instantiate(Shotgun, player.position, player.rotation);
		}
	}
	void Update (){
		if (GunType == 1)
			Gun1Text.text = "M4A1";
		//Debug.Log ("Changed Text to M4A1");

		if (GunType == 0)
			Gun1Text.text = "None";
		//Debug.Log ("Changed Text to None");

		if (GunType == 2)
			Gun1Text.text = "Shotgun";
		//Debug.Log ("Changed Text to Shotgun");
	}
}
