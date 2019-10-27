using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnerManager : MonoBehaviour
{
	//
	//
	[Header("Reset Parameters")]
	public GameObject NewWeaponParent;

	//
	//
	[Header("Spawn Parameters")]
	public int m_iMaxWeaponCount;

	private int m_iWeaponCount = 0;
	private bool m_bInitialized = false;

	public void Initialize()
	{
		if(!m_bInitialized)
		{
			WeaponSpawner[] aWeaponSpawner = GetComponentsInChildren<WeaponSpawner>();
			foreach(WeaponSpawner spawner in aWeaponSpawner)
			{
				spawner.SetWeaponSpawnerManager(this);
			}

			m_bInitialized = true;
		}
	}

	public bool CanSpawnWeapon()
	{
		return m_iWeaponCount < m_iMaxWeaponCount;
	}

	public void OnWeaponSpawned()
	{
		++m_iWeaponCount;
	}

	public void OnWeaponDisappeared()
	{
		--m_iWeaponCount;
	}
}
