using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaminaManager : MonoBehaviour {

	public static StaminaManager SM;

	float _stamina = 5f;

	public float stamina
	{
		get
		{
			return _stamina;
		}
		set
		{
			_stamina = value;
		}
	}

	private float maxStamina = 5.0f;
	private float staminaRechargeDelay = 1.0f;
	private float staminaPenaltyTime = 2.0f;
	private float nextCanRunTime, nextStaminaRechargeTime;

	vp_FPPlayerEventHandler eventHandler;
	bool isRunning;

	bool canCharge;

	[SerializeField]
	Slider staminaBar;

	void Awake()
	{
		if (SM == null)
			SM = this;
		else if (SM != this)
			Destroy(gameObject);    
	}

	void Start () 
	{
		canCharge = true;
		isRunning = false;
		staminaBar = GameObject.Find("StaminaSlider").GetComponent<Slider>();
		eventHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<vp_FPPlayerEventHandler>();
	}

	void Update () 
	{
		staminaBar.minValue = 0;
		staminaBar.maxValue = maxStamina;
		staminaBar.value = stamina;

		//Debug.Log(eventHandler.Run.Active);

		if(eventHandler.Run.Active && !isRunning)
		{
			StartCoroutine(Running());
			isRunning = true;
		}

		if (Time.time > nextStaminaRechargeTime && canCharge) 
		{
			//start to regen stamina. Mathf.Min grabs the lowest value between stamina + 1 per second and Maximum stamina, which is set to 5.
			stamina = Mathf.Min (stamina + Time.deltaTime * 1, maxStamina);
		} 
	}

	public void PsyStamina(float psyStamina)
	{
		//if(!isHolding)
		stamina -= psyStamina;
		//nextStaminaRechargeTime = Time.time + staminaRechargeDelay;
	}

	public void SetRechargeTime()
	{
		nextStaminaRechargeTime = Time.time + staminaRechargeDelay;
	}

	public void CanCharge(bool _canCharge)
	{
		canCharge = _canCharge;
	}

	IEnumerator Running()
	{
		while(eventHandler.Run.Active)
		{
			stamina -= .1f;
			yield return null;
		}

		SetRechargeTime();
		isRunning = false;
	}
}
