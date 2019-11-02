using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawnerManager : MonoBehaviour
{
	//
	//
	[Header("Reset Parameters")]
	public GameObject NewBonusParent;

	//
	//
	[Header("Spawn Parameters")]
	public int m_iMaxBonusCount;

	private int m_iBonusCount = 0;
	private bool m_bInitialized = false;

	public void Initialize()
	{
		if (!m_bInitialized)
		{
			BonusSpawner[] aWeaponSpawner = GetComponentsInChildren<BonusSpawner>();
			foreach (BonusSpawner spawner in aWeaponSpawner)
			{
				spawner.SetBonusSpawnerManager(this);
			}

			m_bInitialized = true;
		}
	}

	public bool CanSpawnBonus()
	{
		return m_iBonusCount < m_iMaxBonusCount;
	}

	public void OnBonusSpawned()
	{
		++m_iBonusCount;
	}

	public void OnBonusDisappeared()
	{
		--m_iBonusCount;
	}
}
