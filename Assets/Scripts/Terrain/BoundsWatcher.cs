using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsWatcher : MonoBehaviour
{
	private void OnTriggerExit(Collider other)
	{
		PlayerGameplay pgpComponent = other.GetComponent<PlayerGameplay>();
		if(null != pgpComponent)
		{
			pgpComponent.OnDie();
		}
	}
}
