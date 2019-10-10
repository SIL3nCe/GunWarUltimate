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

	public void SpawnRandomWeapon()
	{
		GameObject spawnLocation = m_aWeaponSpawnLocations[Random.Range(0, m_aWeaponSpawnLocations.Length)].transform.gameObject;
		GameObject weaponPrefab = m_aWeaponPrefabs[Random.Range(0, m_aWeaponPrefabs.Length)];

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
