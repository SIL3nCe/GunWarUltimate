using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BonusSpawner : MonoBehaviour
{
	//
	// Bonus
	[Header("Spawnable Bonus")]
	public EBonusType[] m_aBonusType;
	public GameObject bonusPrefab;
	public GameObject bonusPrefabPower;
	public GameObject bonusPrefabSpeed;
	public GameObject bonusPrefabShield;
	public GameObject bonusPrefabInvincibility;
	public GameObject bonusPrefabTripleJump;
	public GameObject bonusPrefabUnlimitedAmmo;

	//
	// Spawn parameters
	[Header("Spawn Parameters")]
	public float m_fThresholdNewBonus;

	//
	//
	private BonusSpawnerManager m_manager;
	private float m_fDurationNewBonus = 0.0f;

	public void SetBonusSpawnerManager(BonusSpawnerManager _manager)
	{
		m_manager = _manager;
	}

	void Update()
	{
		m_fDurationNewBonus += Time.deltaTime;

		if (m_fDurationNewBonus >= m_fThresholdNewBonus)
		{
			if (m_manager.CanSpawnBonus())
			{
				SpawnRandomBonus();

				m_fDurationNewBonus = 0.0f;
			}
		}
	}

	public void SpawnRandomBonus()
	{
		EBonusType eBonusType = m_aBonusType[Random.Range(0, m_aBonusType.Length)];

		SpawnBonus(eBonusType);
	}

	public void SpawnBonus(EBonusType eBonusType)
	{
		GameObject prefabToInstanciate = null;
		switch(eBonusType)
		{
			case EBonusType.power:			{	prefabToInstanciate = bonusPrefabPower;			break;	}
			case EBonusType.speed:			{	prefabToInstanciate = bonusPrefabSpeed;			break;	}
			case EBonusType.shield:			{	prefabToInstanciate = bonusPrefabShield;		break;	}
			case EBonusType.invincibility:	{	prefabToInstanciate = bonusPrefabInvincibility;	break;	}
			case EBonusType.triple_jump:	{	prefabToInstanciate = bonusPrefabTripleJump;	break;	}
			case EBonusType.unlimited_ammo:	{	prefabToInstanciate = bonusPrefabUnlimitedAmmo;	break;	}
			default:
			{
				Assert.IsTrue(false); // Implement me
				prefabToInstanciate = bonusPrefab;
				break;
			}
		}
		GameObject newBonus = Instantiate<GameObject>(prefabToInstanciate, transform.position, transform.localRotation);
		newBonus.transform.SetParent(m_manager.NewBonusParent.transform, true);

		Bonus bonus = newBonus.GetComponent<Bonus>();
		if (bonus)
		{
			bonus.SetSpawner(this);
			bonus.m_eType = eBonusType;
		}

		m_manager.OnBonusSpawned();
	}

	public void OnBonusDisappeared()
	{
		m_manager.OnBonusDisappeared();
	}

	public void OnDrawGizmos()
	{
		//
		//
		Gizmos.color = Color.white;
		Gizmos.DrawIcon(transform.position, "BonusSpawner");
	}
}
