using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponType
{
	machine_gun,
	sniper,
    uzi,
    pistolSilencer,
    rocketLauncher
}

public class Weapon : MonoBehaviour
{
	public EWeaponType	WeaponType;

	//
	// Dropped-related
	private bool		bEquipped = false;
	public float		ThresholdBlink = 7.0f;
	public float		ThresholdBlinking = 0.25f;
	public float		ThresholdDisappear = 10.0f;
	private float		fDroppedAccumulatedDuration = 0.0f;
	private MeshRenderer[]	rendererArray;

	private void Start()
	{
		rendererArray = GetComponentsInChildren<MeshRenderer>();
	}

	private void Update()
	{
		if(!bEquipped)
		{
			float fDelta = Time.deltaTime;
			fDroppedAccumulatedDuration += fDelta;
			if (fDroppedAccumulatedDuration < ThresholdBlink)
			{
				// ...
			}
			else if (fDroppedAccumulatedDuration < ThresholdDisappear)
			{
				float fBlinkingDuration = fDroppedAccumulatedDuration - ThresholdBlink;
				int iBlinkingDurationRemainder = (int)(fBlinkingDuration / ThresholdBlinking);
				if (iBlinkingDurationRemainder % 2 == 0)
				{
					foreach(MeshRenderer meshRenderer in rendererArray)
					{
						meshRenderer.enabled = false;
					}
				}
				else
				{
					foreach (MeshRenderer meshRenderer in rendererArray)
					{
						meshRenderer.enabled = true;
					}
				}
			}
			else
			{
				Destroy(gameObject);
				fDroppedAccumulatedDuration = 0.0f;
			}
		}
	}

	public void Drop()
	{
		bEquipped = false;
	}
}
