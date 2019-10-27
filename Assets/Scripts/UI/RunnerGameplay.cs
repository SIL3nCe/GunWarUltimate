using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerGameplay : MonoBehaviour
{
    [Header("Character to spawn")]
    public GameObject CharacPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void OnPlayerValidated(int playerId)
    {
        Instantiate(CharacPrefab, new Vector3(0.0f, -2.0f, 0.5f), Quaternion.identity);
    }

    public void OnPlayerCanceled(int playerId)
    {

    }
}
