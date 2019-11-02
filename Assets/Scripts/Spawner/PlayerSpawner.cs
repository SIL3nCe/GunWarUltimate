using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
	private PlayerSpawnerManager m_manager;

	public void SetPlayerSpawnerManager(PlayerSpawnerManager _manager)
	{
		m_manager = _manager;
	}

	public void OnDrawGizmos()
	{
		//
		//
		Gizmos.color = Color.white;
		Gizmos.DrawIcon(transform.position, "PlayerSpawner");
	}
}
