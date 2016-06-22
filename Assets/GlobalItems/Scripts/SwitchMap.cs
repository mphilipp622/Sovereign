using UnityEngine;
using System.Collections;

public class SwitchMap : MonoBehaviour 
{

	GameObject[] maps;

	void Start () 
	{
		maps = new GameObject[2];
		maps [0] = GameObject.Find ("Map1");
		maps [1] = GameObject.Find ("Map2");
		maps [1].SetActive (false);
	}

	void Update () 
	{
		if(Input.GetKeyDown (KeyCode.LeftAlt))
			ChangeMap ();
	}

	void ChangeMap()
	{
		if (maps [0].activeSelf) 
		{
			maps [1].SetActive (true);
			maps [0].SetActive (false);
		} 
		else if (maps [1].activeSelf) 
		{
			maps[0].SetActive(true);
			maps[1].SetActive (false);
		}
	}
}