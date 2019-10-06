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

	private Transform nextSpawnLocation;

	private UIManager UiManager;

    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        percentage = 0.0f;
    }

    public void TakeDamages(float fDamages, Vector3 orientation)
    {
        percentage = Mathf.Min(percentage + fDamages, 999.0f);

        //
        // Notify death
        if (null != UiManager)
        {
            UiManager.OnPlayerDamageTaken(this);
        }

        // Ejection based on percentage
        orientation.Normalize();

        // convert damages to [-2,5] for exponential
        // Linear ratio conversion ((old_value - old_min) / (old_max - old_min)) * (new_max - new_min) + new_min
        float fRangedVal = (percentage / 999) * 6;

        float fForceFactor = Mathf.Exp(fRangedVal);
        rigidBody.AddForce(orientation * fForceFactor, ForceMode.Impulse);
    }

    public void OnDie()
    {
		if (null != stocks)
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
        GetComponent<AudioSource>().PlayOneShot(m_aAudioClipsScream[iSound]);

        gameObject.SetActive(false);
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
        if (null != stocks && stocks > 0)
        {
            gameObject.SetActive(true);
            gameObject.transform.SetPositionAndRotation(nextSpawnLocation.position, gameObject.transform.rotation);
            if (null != rigidBody)
            {
                rigidBody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                rigidBody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
            }
		}
	}
}
