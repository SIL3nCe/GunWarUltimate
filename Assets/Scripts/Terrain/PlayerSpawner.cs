using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
	public GameObject[] SpawnerLocation;

	public Transform GetSpawnLocation()
	{
		int iIndex = (int)(Random.value * (SpawnerLocation.Length-1));
		return SpawnerLocation[iIndex].transform;
	}
}
