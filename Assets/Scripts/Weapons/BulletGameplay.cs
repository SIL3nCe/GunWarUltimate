using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGameplay : MonoBehaviour
{
    [Tooltip("Prefab to Instantiate on hit")]
    public GameObject HitEffect;

    private float damages; // Set by weapon on shot

    public void SetDamages(float InDamages)
    {
        damages = InDamages;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (null != HitEffect)
        {
            Vector3 bulletLocation = gameObject.transform.position;
            Quaternion bulletRotation = gameObject.transform.rotation;
            GameObject hitExplosion = Instantiate(HitEffect, bulletLocation, bulletRotation);
            RocketExplosion rocketScript = hitExplosion.GetComponent<RocketExplosion>();
            if (null != rocketScript)
            {
                rocketScript.SetDamages(damages);
            }
        }
        else
        {
            PlayerGameplay player = collision.gameObject.GetComponent<PlayerGameplay>();
            if (null != player)
            {
                player.TakeDamages(damages);
            }
        }

        Destroy(gameObject);
    }
}
