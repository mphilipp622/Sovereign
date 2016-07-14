using UnityEngine;
using System.Collections;

public class HideAndShowPanels : MonoBehaviour {

	public GameObject LoadoutPanel = null;
	public GameObject Hero = null;
	public GameObject Camera = null;
	public Transform HeroSpawnPoint = null;

		public void ShowTheLoadoutPanel()
	{
		LoadoutPanel.SetActive(true);
		Camera.SetActive (true);
	}

		public void HideTheLoadoutPanel()
	{
		LoadoutPanel.SetActive (false);
		Camera.SetActive (false);
		Instantiate(Hero, HeroSpawnPoint.position, HeroSpawnPoint.rotation);
	}
}