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
            //TODO get damages to deal
            //Debug.Log(percentage);
            percentage += 10.0f;

            Destroy(collision.gameObject); // Remove bullet
        }
    }
}
