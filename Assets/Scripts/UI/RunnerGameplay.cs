using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerGameplay : MonoBehaviour
{
    [Header("Character to spawn")]
    public GameObject CharacPrefab;

    private GameObject[] aCharacters;

    private int playerCounter;

    private void Start()
    {
        playerCounter = 0;
        aCharacters = new GameObject[8];
    }

    public void OnPlayerValidated(Material mat, int playerId)
    {
        GameObject prefab = Instantiate(CharacPrefab, new Vector3(playerId * -1.3f, -2.0f, 0.5f), CharacPrefab.transform.rotation);
        aCharacters[playerId] = prefab;
        PlayerGameplayRunner charac = prefab.GetComponent<PlayerGameplayRunner>();
        if (charac != null)
        {
            charac.material = mat;
            playerCounter++;
        }
    }

    public void OnPlayerCanceled(int playerId)
    {
        Destroy(aCharacters[playerId]);
        playerCounter--;
    }
}
