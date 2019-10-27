using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerManager : MonoBehaviour
{
	private bool m_bInitialized = false;

	public void Initialize()
	{
		if (!m_bInitialized)
		{
			PlayerSpawner[] aPlayerSpawner = GetComponentsInChildren<PlayerSpawner>();
			foreach (PlayerSpawner spawner in aPlayerSpawner)
			{
				spawner.SetPlayerSpawnerManager(this);
			}

			m_bInitialized = true;
		}
	}

	public Transform GetSpawnLocation()
	{
		PlayerSpawner[] aSpawners = GetComponentsInChildren<PlayerSpawner>();

		return aSpawners[Random.Range(0, aSpawners.Length-1)].transform;
	}
}
