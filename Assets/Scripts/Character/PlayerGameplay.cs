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

    private uint? stocks;
    private float percentage;

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
		if(null != stocks)
		{
			--stocks;
		}
        percentage = 0.0f;

		//
		// Notify death
		UiManager.OnPlayerDied(this);

        if (null != stocks && stocks > 0)
        {
            //TODO set to spawn location
            return;
        }

        //
        // Emit die sounds
        int iSound = Random.Range(0, m_aAudioClipsScream.Length);
        GetComponent<AudioSource>().PlayOneShot(m_aAudioClipsScream[iSound]);
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
}
