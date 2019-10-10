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

    [Tooltip("Effect to trigger on respawn location")]
    public GameObject RespawnEffect;

    [Header("Sounds")]
    public AudioClip[] m_aAudioClipsScream;
    public AudioClip[] m_aAudioClipsDamage;
    public AudioClip m_aAudioClipImplose;
    private AudioSource m_audioSource;
    private bool m_bCanPlayDamageSound = true;

    private uint? stocks;
    private float percentage;

	private Transform nextSpawnLocation;

	private UIManager UiManager;

    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        percentage = 0.0f;

        m_audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamages(float fDamages, Vector3 orientation, float fEjectionFactor)
    {
        percentage = Mathf.Min(percentage + fDamages, 999.0f);

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

        // Ejection based on percentage
        orientation.Normalize();

        // convert damages to [-2,5] for exponential
        // Linear ratio conversion ((old_value - old_min) / (old_max - old_min)) * (new_max - new_min) + new_min
        float fRangedVal = (percentage / 999) * 7;

        float fForceFactor = Mathf.Exp(fRangedVal);

        //Debug.Log("percentage " + percentage);
        //Debug.Log("fRangedVal " + fRangedVal);
        //Debug.Log("fForceFactor " + fForceFactor);
        //Debug.Log("fEjectionFactor " + fEjectionFactor);

        rigidBody.AddForce(orientation * fForceFactor * (fEjectionFactor * 2.0f), ForceMode.Impulse);
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
        if (null != UiManager)
        {
            UiManager.OnPlayerDied(this);
        }

        WeaponHolder weaponHolder = gameObject.GetComponent<WeaponHolder>();
        if (null != weaponHolder)
        { // Remove current weapon
            weaponHolder.OnPlayerDied();
        }

        //
        // Emit die sounds
        int iSound = Random.Range(0, m_aAudioClipsScream.Length);
        AudioManager.GetInstance().PlaySoundEffect(m_aAudioClipsScream[iSound], 0.6f);
        AudioManager.GetInstance().PlaySoundEffect(m_aAudioClipImplose, 1.0f);
        
        //
        // Hide Character during death
        // !! SetActive on entire prefab will block Animator and let skeleton in mid-anim positions
        // Find a better to achieve this if possible
        foreach (var component in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            component.enabled = false;
        }
        // Disable inputs
        GetComponent<PlayerCharacterController>().OnDisable();
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

        if (null != RespawnEffect)
        {
            Instantiate(RespawnEffect, newPose);
        }

        Invoke("Spawn", 3.0f);
	}

	private void Spawn()
    {
        if (null != stocks && stocks > 0)
        {
            //
            // Unhide Character during death
            // !! SetActive on entire prefab will block Animator and let skeleton in mid-anim positions
            // Find a better to achieve this if possible
            foreach (var component in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                component.enabled = true;
            }
            // Enable inputs
            GetComponent<PlayerCharacterController>().OnEnable();

            gameObject.transform.SetPositionAndRotation(nextSpawnLocation.position, gameObject.transform.rotation);
            if (null != rigidBody)
            {
                rigidBody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                rigidBody.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
            }
		}
	}

    private void ResetCanPlayDamageSound()
    {
        m_bCanPlayDamageSound = true;
    }
}

