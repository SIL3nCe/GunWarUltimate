using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
	private GameObject[]	AvailableWeapons;

	//
	// Switch
	private EWeaponType?	WeaponTypeToSwitchTo;
	private bool			bDirtySwitchWeapon;
	private int				NextAmmoCount;

	//
	// Drop-related
	public float			DropForce = 15.0f;

    [Header("Sounds")]
    public AudioClip m_audioClipThrowWeapon;

	// Start is called before the first frame update
	void Start()
    {
		CurrentWeaponType = null;
		CurrentWeapon = null;

		WeaponTypeToSwitchTo = null;
		bDirtySwitchWeapon = false;

		int iWeaponTypeCount = System.Enum.GetValues(typeof(EWeaponType)).Length;
		AvailableWeapons = new GameObject[iWeaponTypeCount];
		AvailableWeapons[(int)EWeaponType.machine_gun]		= SocketMachineGun;
		AvailableWeapons[(int)EWeaponType.pistolSilencer]	= SocketPistolSilencer;
		AvailableWeapons[(int)EWeaponType.rocketLauncher]	= SocketRocketLauncher;
		AvailableWeapons[(int)EWeaponType.sniper]			= SocketSniper;
		AvailableWeapons[(int)EWeaponType.uzi]				= SocketUzi;

		foreach(GameObject weapon in AvailableWeapons)
		{
			Assert.IsNotNull(weapon); // if null, a weapon is missing above
			if(null != weapon)
			{
				weapon.GetComponent<Weapon>().PickUp();
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
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

	public bool SetWeaponTypeToSwitchTo(EWeaponType type, int ammoCount)
	{
		WeaponTypeToSwitchTo = type;
		bDirtySwitchWeapon = true;
		NextAmmoCount = ammoCount;

		return null == CurrentWeapon;
	}

	void SwitchWeapon()
	{
		if (null != CurrentWeapon)
		{
			CurrentWeapon.SetActive(false);
		}

		CurrentWeaponType = WeaponTypeToSwitchTo;
		WeaponTypeToSwitchTo = null;
		CurrentWeapon = AvailableWeapons[(int)CurrentWeaponType];

		if (null != CurrentWeapon)
		{
			CurrentWeapon.SetActive(true);
            WeaponShot weapon = CurrentWeapon.GetComponent<WeaponShot>();

            if (null != weapon)
            {
                weapon.loaderSize = NextAmmoCount;

                if (NextAmmoCount > 0)
                { // Reset rocket laucher alpha
                    weapon.ResetRocketAlpha();
                } 
            }
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

	public void DropWeapon()
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

            //
            //
            AudioManager.GetInstance().PlaySoundEffect(m_audioClipThrowWeapon, 0.8f);
		}
	}

    public void OnPlayerDied()
    {
        if (null != CurrentWeapon)
        {
            CurrentWeapon.SetActive(false);
            CurrentWeapon = null;
            CurrentWeaponType = null;
        }
    }

    public Weapon GetCurrentWeapon()
    {
        if (null != CurrentWeapon)
        {
            return CurrentWeapon.GetComponent<Weapon>();
        }
        else
        {
            return null;
        }
    }
}
