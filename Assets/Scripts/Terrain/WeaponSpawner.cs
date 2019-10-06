using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject[]	spawnLocations;
    public GameObject[]	weaponPrefabs;

	public int MaxWeaponCount = 10;

	public float ThresholdNewWeapon = 5.0f;

	private float fDurationNewWeapon = 0.0f;
	private int iCurrentWeaponCount = 0;

	void Update()
	{
		fDurationNewWeapon += Time.deltaTime;
		if(fDurationNewWeapon >= ThresholdNewWeapon)
		{
			if(iCurrentWeaponCount < MaxWeaponCount)
			{
				SpawnRandomWeapon();

				fDurationNewWeapon = 0.0f;
			}
		}
	}

	public void SpawnRandomWeapon()
	{
		GameObject spawnLocation = spawnLocations[(int)(Random.value * (spawnLocations.Length - 1))];
		GameObject weaponPrefab = weaponPrefabs[(int)(Random.value * (weaponPrefabs.Length - 1))];

		SpawnWeapon(spawnLocation, weaponPrefab);
	}

	public void SpawnWeapon(GameObject spawnLocation, GameObject weaponPrefab)
	{
		GameObject newWeapon = Instantiate<GameObject>(weaponPrefab, spawnLocation.transform.position, spawnLocation.transform.localRotation);
		newWeapon.GetComponent<Rigidbody>().velocity = spawnLocation.transform.right * spawnLocation.GetComponent<WeaponSpawnLocation>().initialVelocity;

		++iCurrentWeaponCount;
	}

	public void OnWeaponDisappeared()
	{
		--iCurrentWeaponCount;
	}
}
