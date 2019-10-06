using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsWatcher : MonoBehaviour
{
    [Tooltip("Effect to play when dying out of map")]
    public GameObject DeathEffect;

    private void OnTriggerExit(Collider other)
	{
		PlayerGameplay pgpComponent = other.GetComponent<PlayerGameplay>();
        if (null != pgpComponent)
        {
            pgpComponent.OnDie();

            if (null != DeathEffect)
            {
                Vector3 bulletLocation = pgpComponent.transform.position;
                Quaternion bulletRotation = pgpComponent.transform.rotation;
                GameObject effect = Instantiate(DeathEffect, bulletLocation, bulletRotation);
                //effect.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f)); // Fixme
            }
        }
    }
}
