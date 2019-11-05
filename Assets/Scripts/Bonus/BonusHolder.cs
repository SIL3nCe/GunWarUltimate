using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BonusHolder : MonoBehaviour
{
	//
	// Bonus-related
	private bool[] m_abCurrentBonuses;
	private float[] m_afCurrentBonusesDt;
	private Bonus[] m_aCurrentBonuses;

	//
	// private reference
	private PlayerCharacterController m_character;

	// Start is called before the first frame update
	void Start()
    {
		int iBonusTypeCount = System.Enum.GetValues(typeof(EBonusType)).Length;
		m_abCurrentBonuses		= new bool[iBonusTypeCount];
		m_afCurrentBonusesDt	= new float[iBonusTypeCount];
		m_aCurrentBonuses		= new Bonus[iBonusTypeCount];

		m_character = GetComponent<PlayerCharacterController>();
		Assert.IsNotNull(m_character);
	}

// Update is called once per frame
void Update()
    {
		float fDt = Time.deltaTime;
		for (int iBonusIndex = 0; iBonusIndex < m_abCurrentBonuses.Length; ++iBonusIndex)
		{
			if (m_abCurrentBonuses[iBonusIndex])
			{
				m_afCurrentBonusesDt[iBonusIndex] += fDt;
				if(m_afCurrentBonusesDt[iBonusIndex] > m_aCurrentBonuses[iBonusIndex].fDuration)
				{
					//
					// Bonus finished, remove effects
					OnBonusFinished((EBonusType)iBonusIndex);
				}
			}
		}
    }

	public void OnBonusPickedUp(Bonus bonus)
	{
		int iIndex = (int)bonus.m_eType;

		//
		// Check if already registered
		bool bAlreadyRegistered = m_abCurrentBonuses[iIndex];

		//
		// Register bonus and reset dt
		m_abCurrentBonuses[iIndex] = true;
		m_afCurrentBonusesDt[iIndex] = 0.0f;

		//
		// Apply bonus effect on holder
		if(!bAlreadyRegistered)
		{
			ApplyBonus(bonus);
		}
	}

	private void OnBonusFinished(EBonusType eBonusType)
	{
		int iIndex = (int)eBonusType;

		//
		// Check if already registered
		bool bRegistered = m_abCurrentBonuses[iIndex];

		//
		// Unregister bonus
		m_abCurrentBonuses[iIndex] = false;

		//
		// Remove bonus effect
		if(bRegistered)
		{
			RemoveBonus(m_aCurrentBonuses[iIndex]);
		}
	}

	public void DeactivateBonuses()
	{
		for(int iBonusIndex = 0; iBonusIndex < m_abCurrentBonuses.Length; ++iBonusIndex)
		{
			OnBonusFinished((EBonusType)iBonusIndex);
		}
	}

	private void ApplyBonus(Bonus bonus)
	{
		SetBonus(bonus, true);
	}

	private void RemoveBonus(Bonus bonus)
	{
		SetBonus(bonus, false);
	}

	private void SetBonus(Bonus bonus, bool bEnable)
	{
		//
		// Update bonus
		m_aCurrentBonuses[(int)bonus.m_eType] = bonus;

		//
		// Appply Bonus
		switch(bonus.m_eType)
		{
			case EBonusType.power:			{	SetBonusPower(bonus, bEnable);			break;	}
			case EBonusType.shield:			{	SetBonusShield(bonus, bEnable);			break;	}
			case EBonusType.speed:			{	SetBonusSpeed(bonus, bEnable);			break;	}
			case EBonusType.triple_jump:	{	SetBonusTripleJump(bonus, bEnable);		break;	}
			case EBonusType.unlimited_ammo:	{	SetBonusUnlimitedAmmo(bonus, bEnable);	break;	}
			case EBonusType.invincibility:	{	SetBonusInvincibility(bonus, bEnable);	break;	}
			case EBonusType.none:			{	/* nothing here */						break;	}
		}
	}

	private void SetBonusPower(Bonus bonus, bool bEnable)
	{
		// Update character's weapon damage factor
		m_character.SetPlayerWeaponDamageFactor(bEnable ? bonus.fBonusPowerWeaponDamageFactor : 1.0f );
	}

	private void SetBonusShield(Bonus bonus, bool bEnable)
	{
		// TODO
	}

	private void SetBonusSpeed(Bonus bonus, bool bEnable)
	{
		// Update character's player speed factor
		m_character.SetPlayerSpeedFactor(bEnable ? bonus.fBonusSpeedFactor : 1.0f);
	}

	private void SetBonusTripleJump(Bonus bonus, bool bEnable)
	{
		// TODO
	}

	private void SetBonusUnlimitedAmmo(Bonus bonus, bool bEnable)
	{
		// TODO
	}

	private void SetBonusInvincibility(Bonus bonus, bool bEnable)
	{
		// TODO
	}

	public bool IsBonusEnabled(EBonusType eBonusType)
	{
		return m_abCurrentBonuses[(int)eBonusType];
	}
}