using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerGameplay : MonoBehaviour
{
    [Header("Character to spawn")]
    public GameObject CharacPrefab;

    private GameObject[] aCharacters;
    private int playerCounter;

    [Header("Block to spawn")]
    public GameObject BlockPrefab;

    private List<GameObject> aBlocks;
    private Vector3 vSpawnLocation;
    private float fSpawnCount;
    private float fSpawnTimer;

    private void Start()
    {
        aCharacters = new GameObject[8];
        playerCounter = 0;

        aBlocks = new List<GameObject>();
        vSpawnLocation = new Vector3(13.0f, -3.0f, 0.0f);

        fSpawnCount = 0.0f;
        fSpawnTimer = 2.0f;
    }

    private void Update()
    {
        for (int i = 0; i < aBlocks.Count; ++i)
        {
            aBlocks[i].transform.Translate(Vector3.left * Time.deltaTime * 5);

            if (aBlocks[i].transform.position.x <= -13.0f)
            {
                Destroy(aBlocks[i]);
                aBlocks.RemoveAt(i--);
            }
        }

        fSpawnCount += Time.deltaTime;

        if (fSpawnCount >= fSpawnTimer)
        {
            fSpawnCount = 0.0f;

            aBlocks.Add(Instantiate(BlockPrefab, vSpawnLocation, Quaternion.identity));

            if (Random.Range(0, 100) >= 30)
            {
                Vector3 vUpBlock = vSpawnLocation;
                vUpBlock.y += Random.Range(1.0f, 3.0f);

                aBlocks.Add(Instantiate(BlockPrefab, vUpBlock, Quaternion.identity));
            }
        }
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
