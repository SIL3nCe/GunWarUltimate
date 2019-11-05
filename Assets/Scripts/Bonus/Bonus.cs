using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBonusType
{
	none,
	speed,
	power,
	shield,
	invincibility,
	triple_jump,
	unlimited_ammo
}

public class Bonus : MonoBehaviour
{
	//
	// Spawner
	private BonusSpawner m_spawner;

	//
	// Properties
	public EBonusType m_eType = EBonusType.none;
	public float fDuration = 5.0f;
	public float fBonusSpeedFactor = 3.0f;
	public float fBonusPowerWeaponDamageFactor = 3.0f;

	public void SetSpawner(BonusSpawner spawner)
	{
		m_spawner = spawner;
	}

	public EBonusType GetBonusType()
	{
		return m_eType;
	}

	private void OnTriggerEnter(Collider other)
	{
		BonusHolder bonusHolder = other.gameObject.GetComponent<BonusHolder>();
		if (null != bonusHolder)
		{
			//
			// Notify BonusHolder
			bonusHolder.OnBonusPickedUp(this);
		
			//
			// Destroy Bonus and notify spawner
			Destroy(this.gameObject);
			m_spawner.OnBonusDisappeared();
		}
	}
}
