using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    //
    // Spwan locations
    [Header("Spawn Locations")]
    public WeaponSpawnLocation[] m_aWeaponSpawnLocations;

    //
    // Weapons
    [Header("Weapons prefabs")]
    public GameObject[] m_aWeaponPrefabs;

    //
    //
    [Header("Spawn Parameters")]
    public int m_iMaxWeaponCount;
    public float m_fThresholdNewWeapon;

	//
	//
	private float fDurationNewWeapon = 0.0f;
    private int iCurrentWeaponCount = 0;

    void Update()
	{
		fDurationNewWeapon += Time.deltaTime;

		if(fDurationNewWeapon >= m_fThresholdNewWeapon)
		{
			if(iCurrentWeaponCount < m_iMaxWeaponCount)
			{
				SpawnRandomWeapon();

				fDurationNewWeapon = 0.0f;
			}
		}
	}

	public Transform GetSpawnLocation()
	{
		GameObject spawnLocation = m_aWeaponSpawnLocations[Random.Range(0, m_aWeaponSpawnLocations.Length)].transform.gameObject;
		return spawnLocation.transform;
	}

	public void SpawnRandomWeapon()
	{
		Transform newTransform = GetSpawnLocation();
		float fInitialVelocity = newTransform.gameObject.GetComponent<WeaponSpawnLocation>().initialVelocity;
		GameObject weaponPrefab = m_aWeaponPrefabs[Random.Range(0, m_aWeaponPrefabs.Length)];

		SpawnWeapon(newTransform, fInitialVelocity, weaponPrefab);
	}

	public void SpawnWeapon(Transform spawnLocation, float fInitialVelocity, GameObject weaponPrefab)
	{
		GameObject newWeapon = Instantiate<GameObject>(weaponPrefab, spawnLocation.position, spawnLocation.localRotation);
		newWeapon.GetComponent<Rigidbody>().velocity = spawnLocation.right * fInitialVelocity;

        WeaponShot weapon = newWeapon.GetComponent<WeaponShot>();
        if (weapon)
        {
            weapon.InitializeLoader();
        }

        ++iCurrentWeaponCount;
	}

	public void OnWeaponDisappeared()
	{
		--iCurrentWeaponCount;
	}

    public void OnDrawGizmos()
    {
        //
        //
        Gizmos.color = Color.red;
        foreach (var oWeaponSpawnerLocation in m_aWeaponSpawnLocations)
        {
            Gizmos.DrawLine(transform.position, oWeaponSpawnerLocation.transform.position);
        }

        //
        //
        Gizmos.color = Color.white;
        Gizmos.DrawIcon(transform.position, "GunSpawner");
    }
}
