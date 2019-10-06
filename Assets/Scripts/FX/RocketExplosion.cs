using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    private float damages; // Set by weapon on shot

    private void Start()
    {
        Invoke("DeletePrefab", 1.0f);
    }

    public void SetDamages(float InDamages)
    {
        damages = InDamages;
    }

    void OnTriggerEnter(Collider collision)
    {
        PlayerGameplay player = collision.gameObject.GetComponent<PlayerGameplay>();
        if (null != player)
        {
            player.TakeDamages(damages);
        }
    }

    void DeletePrefab()
    {
        Destroy(gameObject);
    }
}
