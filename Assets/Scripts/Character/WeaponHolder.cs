using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
	//
	// Current Weapon
	private EWeaponType?	CurrentWeaponType;
	private GameObject		CurrentWeapon;

	//
	// Sockets
	public GameObject		SocketMachineGun;
	public GameObject		SocketSniper;
	public GameObject		SocketUzi;
	public GameObject		SocketPistolSilencer;
	public GameObject		SocketRocketLauncher;

	//
	// Switch
	private EWeaponType?	WeaponTypeToSwitchTo;
	private bool			bDirtySwitchWeapon;

	//
	// Drop-related
	public float			DropForce = 15.0f;

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
		//
		// Weapon drop
		if (Input.GetKeyUp(KeyCode.Return))
		{
			DropWeapon();
		}
		else if (Input.GetKeyUp(KeyCode.KeypadEnter))
		{
			DebugDropWeapon();
		}

		//
		// Weapon switch
		if (bDirtySwitchWeapon)
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

	void DebugDropWeapon()
	{
		if(null == WeaponTypeToSwitchTo)
		{
			WeaponTypeToSwitchTo = EWeaponType.machine_gun;
			SwitchWeapon();
			DropWeapon();
			WeaponTypeToSwitchTo = null;
			bDirtySwitchWeapon = false;
		}
	}

	void DropWeapon()
	{
		if(null != CurrentWeapon)
		{
			//
			// Create new prefab of current weapon
			GameObject newWeapon = Instantiate(CurrentWeapon, CurrentWeapon.transform.position + gameObject.transform.forward, CurrentWeapon.transform.rotation);

			//
			// Hide current weapon
			CurrentWeapon.SetActive(false);
			CurrentWeapon = null;
			CurrentWeaponType = null;

			//
			// Throw new weapon in player's direction
			Rigidbody body = newWeapon.GetComponent<Rigidbody>();
			body.isKinematic = false;
			body.velocity = gameObject.transform.forward * DropForce;
			newWeapon.GetComponent<Weapon>().Drop();
			newWeapon.GetComponent<BoxCollider>().enabled = true;
		}
	}
}
