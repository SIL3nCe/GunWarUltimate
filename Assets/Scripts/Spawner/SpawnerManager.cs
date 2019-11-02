using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpawnerManager : MonoBehaviour
{
	public enum ESpawner
	{
		player,
		weapon,
		bonus,
	}

	[Header("PlayerSpawnerManager")]
	public PlayerSpawnerManager playerSpawnerManager;

	[Header("WeaponSpawnerManager")]
	public WeaponSpawnerManager weaponSpawnerManager;

	[Header("BonusSpawnerManager")]
	public BonusSpawnerManager bonusSpawnerManager;

	private bool bInitialized;

	// Start is called before the first frame update
	void Start()
    {
		// ...
	}

	public void Initialize()
	{
		if(!bInitialized)
		{
			bInitialized = true;

			playerSpawnerManager.Initialize();
			weaponSpawnerManager.Initialize();
			bonusSpawnerManager.Initialize();
		}
	}

	public void EnableSpawning(ESpawner eSpawner)
	{
		switch (eSpawner)
		{
			case ESpawner.player:
			{
				playerSpawnerManager.gameObject.SetActive(true);
				break;
			}
			case ESpawner.weapon:
			{
				weaponSpawnerManager.gameObject.SetActive(true);
				break;
			}
			case ESpawner.bonus:
			{
				bonusSpawnerManager.gameObject.SetActive(true);
				break;
			}
			default:
			{
				Assert.IsTrue(false);
				break;
			}
		}
	}

	public void EnableSpawning()
	{
		playerSpawnerManager.gameObject.SetActive(true);
		weaponSpawnerManager.gameObject.SetActive(true);
		bonusSpawnerManager.gameObject.SetActive(true);
	}

	public void DisableSpawning(ESpawner eSpawner)
	{
		switch (eSpawner)
		{
			case ESpawner.player:
			{
				playerSpawnerManager.gameObject.SetActive(false);
				break;
			}
			case ESpawner.weapon:
			{
				weaponSpawnerManager.gameObject.SetActive(false);
				break;
			}
			case ESpawner.bonus:
			{
				bonusSpawnerManager.gameObject.SetActive(false);
				break;
			}
			default:
			{
				Assert.IsTrue(false);
				break;
			}
		}
	}

	public void DisableSpawning()
	{
		playerSpawnerManager.gameObject.SetActive(false);
		weaponSpawnerManager.gameObject.SetActive(false);
		bonusSpawnerManager.gameObject.SetActive(false);
	}
}
