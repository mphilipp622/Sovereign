using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWeapon : MonoBehaviour {

    [SerializeField]
    int ammo, maxAmmo, damage;

	[SerializeField]
	float fireRate;

	[SerializeField]
	float raycastLength = 20f;

    RaycastHit bulletPath;


    // These variables will be used for psychic sentry gun
	[SerializeField]
	Transform lineOfSight;
    List<Transform> transformsInTrigger, transformsInSight;

	Collider sentryRadiusCol;

	public GameObject weapon
	{
		get
		{
			return gameObject;
		}
	}

	bool _isControlled = false;
	public bool isControlled
	{
		get
		{
			return _isControlled;
		}
		set
		{
			_isControlled = value;
		}
	}

	bool _hasTarget = false;
	public bool hasTarget
	{
		get
		{
			return _hasTarget;
		}
		set
		{
			_hasTarget = value;
		}
	}
	float maxDistance;

	bool startSentry = false;

	void Start ()
    {
        transformsInTrigger = new List<Transform>();
        transformsInSight = new List<Transform>();

		transformsInTrigger.Add(transform.parent);
		transformsInSight.Add(transform.parent);


        ammo = maxAmmo;
		lineOfSight = transform.FindChild("LineOfSight");
		sentryRadiusCol = GetComponent<Collider>();
		maxDistance = Vector3.Distance(transform.position, sentryRadiusCol.bounds.max);
		sentryRadiusCol.enabled = false;

	}
	
	void Update ()
    {
		if(isControlled && ammo > 0)
			sentryRadiusCol.enabled = true;
		else
			sentryRadiusCol.enabled = false;

		if(isControlled && !startSentry)
		{
			StartCoroutine(LineOfSight());
			StartCoroutine(ShootAt(GetClosestTarget()));
			startSentry = true;
		}

//		Debug.Log(bulletPath.collider.name);
	}

	IEnumerator LineOfSight()
	{
		/*
		 * This coroutine will point an invisible transform at every nearby enemy and send out a Raycast.
		 * The Raycast will be used to determine if there's a clear line of sight from the gun's position
		 * to the enemy. It will run as long as our gun is controlled telepathically and the gun has more 
		 * than 0 ammo.
		 */

		if(transformsInTrigger.Count > 0)
		{
	//		while(_isControlled && ammo > 0)
	//		{
		        foreach (Transform enemy in transformsInTrigger)
		        {
					lineOfSight.LookAt(enemy.position);
					if(Physics.Raycast(lineOfSight.position, lineOfSight.forward, out bulletPath, raycastLength))
					{
						if(bulletPath.collider.tag == "Enemy")
						{
						if(!transformsInSight.Contains(bulletPath.collider.transform) && enemy.GetComponent<EnemyStats>().hitPoints > 0)
								transformsInSight.Add(enemy);
						}
						else
						{
							if(transformsInSight.Contains(bulletPath.collider.transform))
								transformsInSight.Remove(enemy);
						}
					}
		        }

	//			yield return null;
	//		}

			lineOfSight.rotation = transform.rotation;
		}
		else
		{
			while(_isControlled && ammo > 0 && transformsInSight.Count == 0)
			{
				foreach (Transform enemy in transformsInTrigger)
				{
					lineOfSight.LookAt(enemy.position);
					if(Physics.Raycast(lineOfSight.position, lineOfSight.forward, out bulletPath, raycastLength))
					{
						if(bulletPath.collider.tag == "Enemy")
						{
							if(!transformsInSight.Contains(bulletPath.collider.transform) && transform.GetComponent<EnemyStats>().hitPoints > 0)
								transformsInSight.Add(enemy);
						}
						else
						{
							if(transformsInSight.Contains(bulletPath.collider.transform))
								transformsInSight.Remove(enemy);
						}
					}
				}

				yield return null;
			}

			lineOfSight.rotation = transform.rotation;
		}

		yield return null;
	}

    void AddNearbyEnemies(Transform enemy)
    {
        if (!transformsInTrigger.Contains(enemy))
            transformsInTrigger.Add(enemy);
        else
            return;
    }

    void RemoveNearbyEnemies(Transform enemy)
    {
		if (transformsInTrigger.Contains(enemy))
		{
			transformsInTrigger.Remove(enemy);
			transformsInSight.Remove(enemy); // If the enemy is no longer nearby, then they can't be in line of sight either.
		}
		else
			return;
    }

	Transform GetClosestTarget()
	{
		Transform newTarget = null;

		float distance = maxDistance;

		if(transformsInSight.Count > 0)
		{
			foreach(Transform enemy in transformsInSight)
			{
				if(Vector3.Distance(transform.position, enemy.position) < distance)
				{
					// In here we essentially sort through all the nearby enemies. If they are closer
					// than the last enemy, then we set distance to be relative to the closer
					// enemy.
					newTarget = enemy;
					distance = Vector3.Distance(transform.position, enemy.position);
				}
			}

			hasTarget = true;
			return newTarget;
		}
		else
			return null;
//		distance = maxDistance; // set distance back to maxDistance so next time we run coroutine, we start from farthest distance and move inward.
//		Debug.Log(newTarget.name);
		
	}

	IEnumerator ShootAt(Transform target)
	{
		while(_isControlled && ammo > 0)
		{
			if(hasTarget)
			{
				if(target.GetComponent<EnemyStats>().hitPoints > 0)
				{
					transform.LookAt(target.position);
					Fire();
				}
				else
				{
					RemoveNearbyEnemies(target);
					hasTarget = false;
				}
					
			}
			else
			{
				Debug.Log("Searching");

				StartCoroutine(LineOfSight());
				target = GetClosestTarget();
			}

			yield return new WaitForSeconds(fireRate);
		}

		startSentry = false;
	}

	void Fire()
	{
		if(bulletPath.collider.tag == "Enemy")
		{
			if(bulletPath.collider.transform.GetComponent<EnemyStats>().hitPoints > 0)
			{
				Debug.Log("Damage " + bulletPath.collider.name);
				bulletPath.collider.transform.GetComponent<EnemyStats>().hitPoints -= damage;
				ammo--;
			}
			// Apply Damage Here
			// Apply recoil
			// Apply sound effects and muzzle flash


		}
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
//			Debug.Log("True");
            AddNearbyEnemies(col.transform);
        }
    }

    void OnTriggerExit(Collider col)
    {
		if (col.tag == "Enemy")
		{
			RemoveNearbyEnemies(col.transform);
		}
    }
}
