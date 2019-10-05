using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplay : MonoBehaviour
{
    private int lifes;
    private float percentage;

    void Start()
    {
        lifes = 3;
        percentage = 0.0f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ammo"))
        {
            BulletParameters bulletParams = collision.gameObject.GetComponent<BulletParameters>();
            if (null != bulletParams)
            {
                percentage += bulletParams.GetDamages();
                Debug.Log(percentage);
            }

            Destroy(collision.gameObject); // Remove bullet
        }
    }
}
