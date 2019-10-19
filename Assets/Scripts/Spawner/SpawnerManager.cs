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
	}

	[Header("PlayerSpawner")]
	public PlayerSpawner playerSpawner;
	
	[Header("WeaponSpawner")]
	public WeaponSpawner weaponSpawner;

	//[Header("BonusSpawner")]

	private GameObject[] aSpawners;

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
			aSpawners = new GameObject[System.Enum.GetValues(typeof(ESpawner)).Length];
			aSpawners[(int)ESpawner.player] = playerSpawner.gameObject;
			aSpawners[(int)ESpawner.weapon] = weaponSpawner.gameObject;
		}
	}

    // Update is called once per frame
    void Update()
    {

	}

	public void EnableSpawning(ESpawner eSpawner)
	{
		aSpawners[(int)eSpawner].SetActive(true);
	}

	public void EnableSpawning()
	{
		foreach(GameObject spawner in aSpawners)
		{
			spawner.SetActive(true);
		}
	}

	public void DisableSpawning(ESpawner eSpawner)
	{
		aSpawners[(int)eSpawner].SetActive(false);
	}

	public void DisableSpawning()
	{
		foreach (GameObject spawner in aSpawners)
		{
			spawner.SetActive(false);
		}
	}

	public Transform GetSpawnLocation(ESpawner eSpawner)
	{
		switch(eSpawner)
		{
			case ESpawner.player:
			{
				return aSpawners[(int)eSpawner].GetComponent<PlayerSpawner>().GetSpawnLocation();
			}
			case ESpawner.weapon:
			{
				return aSpawners[(int)eSpawner].GetComponent<WeaponSpawner>().GetSpawnLocation();
			}
			default:
			{
				Assert.IsTrue(false);
				return transform;
			}
		}
	}
}
