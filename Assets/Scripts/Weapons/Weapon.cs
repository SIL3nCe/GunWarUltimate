using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponType
{
	machine_gun,
	sniper,
}

public class Weapon : MonoBehaviour
{
	public EWeaponType WeaponType;
}
