using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    private float damages; // Set by bullet on hit
    private float ejectionFactor; // Set by bullet on hit

    public void SetDamages(float InDamages)
    {
        damages = InDamages;
    }
    public void SetEjectionFactor(float InEjection)
    {
        ejectionFactor = InEjection;
    }

    void OnTriggerEnter(Collider collision)
    {
        PlayerGameplay player = collision.gameObject.GetComponent<PlayerGameplay>();
        if (null != player)
        {
            Vector3 dir = player.transform.position - gameObject.transform.position;
            player.TakeDamages(damages, dir, ejectionFactor);
        }
    }
}
