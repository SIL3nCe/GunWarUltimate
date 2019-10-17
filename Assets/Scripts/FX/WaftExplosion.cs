using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaftExplosion : MonoBehaviour
{
    public float damages;
    public float ejectionFactor;

    void Start()
    {
        Invoke("DisableCollisions", 0.1f);
    }

    void DisableCollisions()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerGameplay player = other.gameObject.GetComponent<PlayerGameplay>();
        if (null != player && other.gameObject != transform.parent.gameObject)
        {
            Vector3 dir = player.transform.position - transform.position;
            player.TakeDamages(damages, dir, ejectionFactor);
        }
    }
}
