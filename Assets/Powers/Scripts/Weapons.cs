using UnityEngine;
using System.Collections;

public class Weapons : MonoBehaviour {
	public int currentWeapon;
	public GameObject[] weapons;

	// Use this for initialization
	void Start () {

		//set default weapon to weapon 0 of the array
		ChangeWeapon (0);
	}
	
	// Update is called once per frame
	void Update () {

		//Logic for switching weapons based on what key is pressed. Hope to expand on this in the future to include mouse wheel up and down
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			ChangeWeapon(0);
		}
		else if(Input.GetKeyDown (KeyCode.Alpha2)){
			ChangeWeapon(1);
		}
	}

	void ChangeWeapon(int num){

		//set current weapon to whatever weapon is assigned to the value of the weapons array.
		currentWeapon = num;

		//Loop will run through the number of weapons user may have. If i equals the value of the number that the user presses
		//then we set the weapon that belongs to that array index to active and set the other weapons in the array to false.
		for (int i = 0; i < weapons.Length; i++) {
			if(i == num)
				weapons[i].gameObject.SetActive(true);
			else
				weapons[i].gameObject.SetActive (false);
			}
	}
}
