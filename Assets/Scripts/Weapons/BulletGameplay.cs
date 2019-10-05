using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGameplay : MonoBehaviour
{
    private float damages; // Set by weapon on shot

    public void SetDamages(float InDamages)
    {
        damages = InDamages;
    }

    void OnCollisionEnter(Collision collision)
    {  
        BulletGameplay bullet = collision.gameObject.GetComponent<BulletGameplay>();
        if (null != bullet)
        { 
            // Do not delete bullet if hit another one
            return;
        }

        PlayerGameplay player = collision.gameObject.GetComponent<PlayerGameplay>();
        if (null != player)
        {
            player.TakeDamages(damages);
        }

        Destroy(gameObject);
    }
}
