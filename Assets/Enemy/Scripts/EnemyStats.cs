using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour {

	[SerializeField]
	int hp;

	public int hitPoints
	{
		get
		{
			return hp;
		}
		set
		{
			hp = value;
		}
	}

	void Start () 
	{
	
	}

	void Update () 
	{
	
	}
}
