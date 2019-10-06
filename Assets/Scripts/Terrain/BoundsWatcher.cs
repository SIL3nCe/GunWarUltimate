using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsWatcher : MonoBehaviour
{
    [Tooltip("Effect to play when dying out of map")]
    public GameObject DeathEffect;

    private BoxCollider boundsCollider;

    void Start()
    {
        boundsCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void OnTriggerExit(Collider other)
	{
		PlayerGameplay pgpComponent = other.GetComponent<PlayerGameplay>();
        if (null != pgpComponent)
        {
            pgpComponent.OnDie();

            if (null != DeathEffect)
            {
                Vector3 playerLocation = pgpComponent.transform.position;
                GameObject effect = Instantiate(DeathEffect, playerLocation, Quaternion.identity);

                if (null != boundsCollider)
                {
                    Vector3 max = boundsCollider.bounds.max;
                    Vector3 min = boundsCollider.bounds.min;
                    Vector3 rotation = new Vector3(0.0f, 90.0f, 0.0f);

                    if (playerLocation.x >= max.x)
                    {
                        rotation.y = -90.0f;
                    }
                    else if (playerLocation.y >= max.y)
                    {
                        rotation.x = 90.0f;
                    }
                    else if (playerLocation.y <= min.y)
                    {
                        rotation.x = -90.0f;
                    }
                           
                    effect.transform.Rotate(rotation);
                }
            }
        }
    }
}
