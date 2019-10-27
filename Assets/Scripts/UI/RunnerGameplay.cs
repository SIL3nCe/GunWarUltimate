using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerGameplay : MonoBehaviour
{
    [Header("Character to spawn")]
    public GameObject CharacPrefab;
    
    public void OnPlayerValidated(Material mat, int playerId)
    {
        GameObject prefab = Instantiate(CharacPrefab, new Vector3(0.0f, -2.0f, 0.5f), Quaternion.identity);
        PlayerGameplayRunner charac = prefab.GetComponent<PlayerGameplayRunner>();
        if (charac != null)
        {
            charac.material = mat;
        }
    }

    public void OnPlayerCanceled(int playerId)
    {

    }
}
