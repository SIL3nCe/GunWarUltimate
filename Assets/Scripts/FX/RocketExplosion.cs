using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    private float damages; // Set by weapon on shot

    public void SetDamages(float InDamages)
    {
        damages = InDamages;
    }

    void OnTriggerEnter(Collider collision)
    {
        PlayerGameplay player = collision.gameObject.GetComponent<PlayerGameplay>();
        if (null != player)
        {
            Vector3 dir = player.transform.position - gameObject.transform.position;
            player.TakeDamages(damages, dir);
        }
    }
}
