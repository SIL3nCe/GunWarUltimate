using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
	//
	// Weapons
	[Header("Weapons prefabs")]
    public GameObject[] m_aWeaponPrefabs;

    //
    // Spawn parameters
    [Header("Spawn Parameters")]
    public float m_fThresholdNewWeapon;
	public float m_fInitialVelocity;

	//
	//
	private WeaponSpawnerManager m_manager;
	private float m_fDurationNewWeapon = 0.0f;

	public void SetWeaponSpawnerManager(WeaponSpawnerManager _manager)
	{
		m_manager = _manager;
	}

    void Update()
	{
		m_fDurationNewWeapon += Time.deltaTime;

		if(m_fDurationNewWeapon >= m_fThresholdNewWeapon)
		{
			if(m_manager.CanSpawnWeapon())
			{
				SpawnRandomWeapon();

				m_fDurationNewWeapon = 0.0f;
			}
		}
	}

	public void SpawnRandomWeapon()
	{
		GameObject weaponPrefab = m_aWeaponPrefabs[Random.Range(0, m_aWeaponPrefabs.Length)];

		SpawnWeapon(weaponPrefab);
	}

	public void SpawnWeapon(GameObject weaponPrefab)
	{
		GameObject newWeapon = Instantiate<GameObject>(weaponPrefab, transform.position, transform.localRotation);
		newWeapon.transform.SetParent(m_manager.NewWeaponParent.transform, true);	
		newWeapon.GetComponent<Rigidbody>().velocity = transform.right * m_fInitialVelocity;

        WeaponShot weaponShot = newWeapon.GetComponent<WeaponShot>();
        if (weaponShot)
        {
			weaponShot.InitializeLoader();
		}

		Weapon weapon = newWeapon.GetComponent<Weapon>();
		if (weapon)
		{
			weapon.SetSpawner(this);
		}

		m_manager.OnWeaponSpawned();
	}

	public void OnWeaponDisappeared()
	{
		m_manager.OnWeaponDisappeared();
	}

	public void OnDrawGizmos()
	{
		//
		//
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + transform.right * m_fInitialVelocity);

		//
		//
		Gizmos.color = Color.white;
		Gizmos.DrawIcon(transform.position, "GunSpawner");
	}
}
