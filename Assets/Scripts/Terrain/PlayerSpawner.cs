using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
	public GameObject[] SpawnerLocation;

	public Transform GetSpawnLocation()
	{
		int iIndex = Random.Range(0, SpawnerLocation.Length);
		return SpawnerLocation[iIndex].transform;
	}
}
