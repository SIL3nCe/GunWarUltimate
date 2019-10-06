using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerEnum
{
	p1,
	p2,
}

public class PlayerGameplay : MonoBehaviour
{
	public EPlayerEnum playerEnum;
	public SkinnedMeshRenderer Head;

    [Header("Sounds")]
    public AudioClip[] m_aAudioClipsScream;

    [Tooltip("Effect to play when dying out of map")]
    public GameObject DeathEffect;

    private uint? stocks;
    private float percentage;

	private Transform nextSpawnLocation;

	private UIManager UiManager;

	void Start()
    {
        percentage = 0.0f;
    }

    public void TakeDamages(float damages)
    {
        percentage += damages;

        //
        // Notify death
        if (null != UiManager)
        {
            UiManager.OnPlayerDamageTaken(this);
        }
    }

    public void OnDie()
    {
		if (null != stocks)
		{
			--stocks;
		}
        percentage = 0.0f;

        if (null != DeathEffect)
        {
            Vector3 bulletLocation = gameObject.transform.position;
            Quaternion bulletRotation = gameObject.transform.rotation;
            GameObject effect = Instantiate(DeathEffect, gameObject.transform);
            effect.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f)); // Fixme
        }

        //
        // Notify death
        UiManager.OnPlayerDied(this);

        //
        // Emit die sounds
        int iSound = Random.Range(0, m_aAudioClipsScream.Length);
        GetComponent<AudioSource>().PlayOneShot(m_aAudioClipsScream[iSound]);

        //
        //
        if (null != stocks && stocks > 0)
        {
            //TODO set to spawn location
            return;
        }
	}

	public float GetPercentage()
	{
		return percentage;
	}

	public uint? GetRemainingStocks()
	{
		return stocks;
	}

	public void SetUiManager(UIManager Manager)
	{
		UiManager = Manager;
	}

	public void SetStocks(uint? iStocks)
	{
		stocks = iStocks;
	}

	public void SetNextSpawnLocation(Transform newPose)
	{
		nextSpawnLocation = newPose;

		Invoke("Spawn", 3.0f);
	}

	private void Spawn()
	{
		if(null != stocks && stocks > 0)
		{
			gameObject.transform.SetPositionAndRotation(nextSpawnLocation.position, nextSpawnLocation.rotation);
		}
	}
}
