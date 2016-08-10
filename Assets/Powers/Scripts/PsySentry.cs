using UnityEngine;
using System.Collections;

public class PsySentry : MonoBehaviour {

	Ray powerPath;
	RaycastHit outlineRay;

	[SerializeField]
	float costToUse = 50f, cooldownTime = 20f, powerDistance = 20f;

	float timePlusCooldown = 0f;

	bool canCast = true;

	bool isOutlined;

	GameObject outlinedObject;

	[SerializeField]
	float glowMax = 5f, flickerMinimum = 1f, glowSpeed = 6f;

	void Start () 
	{
		outlinedObject = null;
		isOutlined = false;
	}

	void Update () 
	{
		powerPath = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

		if(Time.time > timePlusCooldown && StaminaManager.SM.stamina > costToUse)
			canCast = true;
		else
			canCast = false;
		
		if(canCast)
		{
			if(Physics.Raycast(powerPath, out outlineRay, powerDistance))
			{
				if(outlineRay.collider.tag == "Enemy" && !isOutlined)
				{
					if(outlineRay.collider.transform.childCount > 0)
					{
						outlinedObject = outlineRay.collider.GetComponentInChildren<EnemyWeapon>().weapon;
						StartCoroutine(OutlineObject(outlineRay.collider.GetComponentInChildren<EnemyWeapon>().weapon.GetComponent<Renderer>()));
					}
//					Debug.Log("Test");
				}
				else if(outlineRay.collider.tag == "Enemy" && isOutlined)
				{
					if(Input.GetButtonDown("Fire3") && canCast && StaminaManager.SM.stamina > costToUse)
					{
						canCast = false;
						outlinedObject.transform.parent = null;
//						grabParticleMain.Play(); // If I want particles, put them here.
						outlinedObject.transform.position += Vector3.forward * 2;
						outlinedObject.GetComponent<EnemyWeapon>().isControlled = true;

					}
				}

				if(outlinedObject.transform.parent != outlineRay.collider.transform && canCast)
				{
					isOutlined = false;
				}
			}
			else
				isOutlined = false;
		}
		else
			isOutlined = false;




	}

	IEnumerator OutlineObject(Renderer _rend)
	{
		isOutlined = true;

		float glowStrength = 0;
		while(glowStrength < glowMax)
		{
			_rend.material.SetFloat("_MKGlowTexStrength", glowStrength);
			glowStrength += Time.deltaTime * glowSpeed;
			yield return null;
		}

		//Execute while object is outlined
		while(isOutlined)
		{

			while(glowStrength > flickerMinimum)
			{
				if(!isOutlined)
					break;

				_rend.material.SetFloat("_MKGlowTexStrength", glowStrength);
				glowStrength -= Time.deltaTime * glowSpeed;
				yield return null;
			}

			while(glowStrength < glowMax)
			{
				if(!isOutlined)
					break;

				_rend.material.SetFloat("_MKGlowTexStrength", glowStrength);
				glowStrength += Time.deltaTime * glowSpeed;
				yield return null;
			}

			yield return null;
		}

		while(glowStrength > 0f)
		{
			_rend.material.SetFloat("_MKGlowTexStrength", glowStrength);
			glowStrength -= Time.deltaTime * glowSpeed;
			yield return null;
		}

	}
}
