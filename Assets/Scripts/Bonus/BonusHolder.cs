using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusHolder : MonoBehaviour
{
	[Header("BonusHolder parameters")]
	public float fBonusDuration = 10.0f;

	//
	// Bonus-related
	private bool[] m_abCurrentBonuses;
	private float[] m_afCurrentBonusesDt;

    // Start is called before the first frame update
    void Start()
    {
		int iBonusTypeCount = System.Enum.GetValues(typeof(EBonusType)).Length;
		m_abCurrentBonuses		= new bool[iBonusTypeCount];
		m_afCurrentBonusesDt	= new float[iBonusTypeCount];
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
				if(m_afCurrentBonusesDt[iBonusIndex] > fBonusDuration)
				{
					//
					// Bonus finished, remove effects
					OnBonusFinished((EBonusType)iBonusIndex);
				}
			}
		}
    }

	public void OnBonusPickedUp(EBonusType eBonusType)
	{
		int iIndex = (int)eBonusType;

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
			ApplyBonus(eBonusType);
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
			RemoveBonus((EBonusType)iIndex);
		}
	}

	public void DeactivateBonuses()
	{
		for(int iBonusIndex = 0; iBonusIndex < m_abCurrentBonuses.Length; ++iBonusIndex)
		{
			OnBonusFinished((EBonusType)iBonusIndex);
		}
	}

	private void ApplyBonus(EBonusType eBonusType)
	{
		SetBonus(eBonusType, true);
	}

	private void RemoveBonus(EBonusType eBonusType)
	{
		SetBonus(eBonusType, false);
	}

	private void SetBonus(EBonusType eBonusType, bool bEnable)
	{
		switch(eBonusType)
		{
			case EBonusType.power:			{	SetBonusPower(bEnable);			break;	}
			case EBonusType.resistance:		{	SetBonusResistance(bEnable);	break;	}
			case EBonusType.speed:			{	SetBonusSpeed(bEnable);			break;	}
			case EBonusType.triple_jump:	{	SetBonusTripleJump(bEnable);	break;	}
			case EBonusType.unlimited_ammo:	{	SetBonusUnlimitedAmmo(bEnable);	break;	}
			case EBonusType.invincibility:	{	SetBonusInvincibility(bEnable);	break;	}
			case EBonusType.none:			{	/* nothing here */				break;	}
		}
	}

	private void SetBonusPower(bool bEnable)
	{
		// TODO
	}

	private void SetBonusResistance(bool bEnable)
	{
		// TODO
	}

	private void SetBonusSpeed(bool bEnable)
	{
		// TODO
	}

	private void SetBonusTripleJump(bool bEnable)
	{
		// TODO
	}

	private void SetBonusUnlimitedAmmo(bool bEnable)
	{
		// TODO
	}

	private void SetBonusInvincibility(bool bEnable)
	{
		// TODO
	}
}