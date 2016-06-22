using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ingredients : MonoBehaviour {
	public bool wood= false;
	public int woodquantity= 1;
	public bool ingredientbool;
	public string ingredientname=" ";
	public int ingredientquantity= 0;
	
	GameObject updateGobject;
	
	private int objectQuantity;
	private string Gobjectname;
	Text text;
	void Awake () {
		objectQuantity=ingredientquantity;
		Gobjectname=ingredientname;
		text = GetComponent <Text> ();

	}
	public int increasequantity ( int x){
			woodquantity+=x;
			wood=true;
		text.text = "Lumber: " + woodquantity;

		return woodquantity;
	}
	void Update () {
		//
	

	
	}
	
}