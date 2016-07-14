using UnityEngine;
using System.Collections;

public class HeartRate : MonoBehaviour {

	public static HeartRate HR;

	vp_FPPlayerEventHandler eventHandler;

	float _bpm;

	public float bpm
	{
		get
		{
			return _bpm;
		}
		set
		{
			_bpm = value;
		}
	}

	void Awake()
	{
		if (HR == null)
			HR = this;

		else if (HR != this)
			Destroy(gameObject);
	}

	void Start () 
	{
		eventHandler = GetComponent<vp_FPPlayerEventHandler>();

	}

	void Update () 
	{
		Debug.Log(eventHandler.Run.Active); // This is how you call the state.
	}
}
