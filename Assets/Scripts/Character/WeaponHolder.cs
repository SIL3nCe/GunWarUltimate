using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
	private EWeaponType?	CurrentWeaponType;
	private GameObject		CurrentWeapon;

	//
	// Sockets
	public GameObject		SocketMachineGun;

	private EWeaponType?	WeaponTypeToSwitchTo;
	private bool			bDirtySwitchWeapon;

	// Start is called before the first frame update
	void Start()
    {
		CurrentWeaponType = null;
		CurrentWeapon = null;

		WeaponTypeToSwitchTo = null;
		bDirtySwitchWeapon = false;
	}

    // Update is called once per frame
    void Update()
    {
		if(bDirtySwitchWeapon)
		{
			if(null == CurrentWeapon)
			{
				SwitchWeapon();
			}

			bDirtySwitchWeapon = false;
		}
    }

	public bool SetWeaponTypeToSwitchTo(EWeaponType type)
	{
		WeaponTypeToSwitchTo = type;
		bDirtySwitchWeapon = true;

		return null == CurrentWeapon;
	}

	void SwitchWeapon()
	{
		if(null != CurrentWeapon)
		{
			CurrentWeapon.SetActive(false);
		}

		CurrentWeaponType = WeaponTypeToSwitchTo;
		WeaponTypeToSwitchTo = null;

		switch (CurrentWeaponType)
		{
			case EWeaponType.machine_gun:
			{
				CurrentWeapon = SocketMachineGun;
				break;
			}
			default:
			{
				CurrentWeapon = null;
				break;
			}
		}

		if(null != CurrentWeapon)
		{
			CurrentWeapon.SetActive(true);
		}
	}
	
}
