using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTriggerCollider : MonoBehaviour
{
	private Weapon _Weapon;

	void CopyComponent<T>(T original, T newComponent, GameObject destination) where T : Component
	{
		System.Type type = original.GetType();
		var dst = newComponent;
		var fields = type.GetFields();
		foreach (var field in fields)
		{
			if (field.IsStatic) continue;
			field.SetValue(dst, field.GetValue(original));
		}
		var props = type.GetProperties();
		foreach (var prop in props)
		{
			if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
			prop.SetValue(dst, prop.GetValue(original, null), null);
		}
	}

	// Start is called before the first frame update
	void Start()
    {
		//
		// Create TriggerCollider in function of Collider
		BoxCollider colliderRigid = gameObject.GetComponent<BoxCollider>();
		if(null != colliderRigid && colliderRigid.enabled)
		{
			BoxCollider colliderTrigger = gameObject.AddComponent<BoxCollider>() as BoxCollider;
			CopyComponent<BoxCollider>(colliderRigid, colliderTrigger, gameObject);
			colliderTrigger.isTrigger = true;
		}

		//
		// Store weapon script
		_Weapon = gameObject.GetComponent<Weapon>();

	}

    private void OnTriggerEnter(Collider other)
    {
		WeaponHolder componentWeaponHolder = other.gameObject.GetComponent<WeaponHolder>();
        if(null != componentWeaponHolder)
        {
			//
			// Show correct weapon in body
			bool bCanSwitch = componentWeaponHolder.SetWeaponTypeToSwitchTo(_Weapon.WeaponType);

			//
			// Destroy current weapon
			if(bCanSwitch)
			{
				Destroy(gameObject);
			}
		}
	}
}
