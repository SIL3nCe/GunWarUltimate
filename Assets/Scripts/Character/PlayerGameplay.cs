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

    public void TakeDamages(float damages)
    {
        percentage += damages;
        Debug.Log(percentage);
    }

    public void OnDie()
    {
        lifes--;
        percentage = 0.0f;

        if (lifes > 0)
        {
            //TODO set to spawn location
            return;
        }

        // TODO Game over for this player
    }

    public float GetPercentage()
    {
        return percentage;
    }
}
