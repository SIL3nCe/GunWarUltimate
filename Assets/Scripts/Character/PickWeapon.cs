using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickWeapon : MonoBehaviour
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
			SwitchWeapon();

			bDirtySwitchWeapon = false;
		}
    }

	public void SetWeaponTypeToSwitchTo(EWeaponType type)
	{
		WeaponTypeToSwitchTo = type;
		bDirtySwitchWeapon = true;
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
