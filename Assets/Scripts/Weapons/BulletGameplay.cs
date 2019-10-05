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
        PlayerGameplay player = collision.gameObject.GetComponent<PlayerGameplay>();
        if (null != player)
        {
            player.TakeDamages(damages);
        }

        Destroy(gameObject);
    }
}
