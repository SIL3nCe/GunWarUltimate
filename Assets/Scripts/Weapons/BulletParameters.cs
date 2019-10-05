using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParameters : MonoBehaviour
{
    private float damages; // Set by weapon on shot

    public float GetDamages()
    {
        return damages;
    }

    public void SetDamages(float InDamages)
    {
        damages = InDamages;
    }
}
