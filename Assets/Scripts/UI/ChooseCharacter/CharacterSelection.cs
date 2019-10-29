using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine;

// Script used with cursors on runner champ select

public class CharacterSelection : MonoBehaviour
{
    [Header("Characters")]
    public BoxCollider2D[] CharacterBoxes;
    public bool[] CharacterChoseState;
    public Material[] CharacterMaterials;

    [Header("Cursors")]
    public RawImage[] CursorList;

    [Header("Runner ref")]
    public RunnerGameplay RunnerScript;

    struct SPlayer
    {
        //TODO public int deviceID; // Linked device id
        public int chosenId; // true if selected its character
        public bool bValidated; // true if selected its character
    }
    private SPlayer[] aPlayers;

    // Screen values for cursor limits
    private float fHalfHeight;
    private float fScreenWidth, fScreenHeight;

    //debug only
    private int currId;

    void Start()
    {
        Assert.IsTrue(CursorList.Length == 8);
        Assert.IsTrue(CharacterBoxes.Length == 8);
        Assert.IsTrue(CharacterMaterials.Length == 8);

        CharacterChoseState = new bool[8];

        aPlayers = new SPlayer[8];

        fScreenWidth = Screen.width;
        fScreenHeight = Screen.height;
        fHalfHeight = fScreenHeight * 0.5f;
    }
    
    void Update()
    {
        // Move cursors
        // TODO check for move inputs, move relevant cursor based on device id

        // Special case of mouse for P1
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            if (!aPlayers[0].bValidated)
            {
                Vector3 vMouseLocation = Input.mousePosition;
                KeepInCharacterPanel(ref vMouseLocation);
                CursorList[0].transform.position = vMouseLocation;
            }
        }

        // Validate action
        // TODO handle all devices (based on input which try to valide)
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!aPlayers[0].bValidated)
                {
                    int nCharac = CharacterBoxes.Length;
                    for (int i = 0; i < nCharac; ++i)
                    {
                        if (CharacterChoseState[i] == false)
                        {
                            if (CharacterBoxes[i].bounds.Contains(CursorList[0].transform.position))
                            {
                                aPlayers[0].bValidated = true;
                                aPlayers[0].chosenId = i;
                                RunnerScript.OnPlayerValidated(CharacterMaterials[i], i); // TODO replace i with player id
                                CharacterChoseState[i] = true;
                                currId = i;
                            }
                        }
                    }
                }
            }
        }

        // Cancel action
        //TODO - Get playerid based on input device
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                aPlayers[0].bValidated = false;
                CharacterChoseState[aPlayers[0].chosenId] = false;
                RunnerScript.OnPlayerCanceled(currId);
            }
        }
    }

    private void KeepInCharacterPanel(ref Vector3 location)
    {
        location.x = Mathf.Max(location.x, 0.0f);
        location.x = Mathf.Min(location.x, fScreenWidth);
        location.y = Mathf.Max(location.y, fHalfHeight);
        location.y = Mathf.Min(location.y, fScreenHeight);
    }
}
