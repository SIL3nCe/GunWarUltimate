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
    public AudioClip[] m_aAudioClipsDamage;
    private AudioSource m_audioSource;
    private bool m_bCanPlayDamageSound = true;

    private uint? stocks;
    private float percentage;

	private Transform nextSpawnLocation;

	private UIManager UiManager;

	void Start()
    {
        percentage = 0.0f;

        m_audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamages(float damages)
    {
        percentage += damages;

        //
        // Play damage sound
        if (m_bCanPlayDamageSound)
        {
            int iSound = Random.Range(0, m_aAudioClipsDamage.Length);
            m_audioSource.PlayOneShot(m_aAudioClipsDamage[iSound], 0.2f);
            m_bCanPlayDamageSound = false;
            Invoke("ResetCanPlayDamageSound", 0.4f);
        }

        //
        // Notify death
        if (null != UiManager)
        {
            UiManager.OnPlayerDamageTaken(this);
        }
    }

    public void OnDie()
    {
		if(null != stocks)
		{
			--stocks;
		}
        percentage = 0.0f;

		//
		// Notify death
		UiManager.OnPlayerDied(this);

        //
        // Emit die sounds
        int iSound = Random.Range(0, m_aAudioClipsScream.Length);
        m_audioSource.PlayOneShot(m_aAudioClipsScream[iSound], 0.2f);

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

    private void ResetCanPlayDamageSound()
    {
        m_bCanPlayDamageSound = true;
    }
}

