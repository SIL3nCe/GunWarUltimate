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
    public Material[] CharacterMaterials;

    [Header("Cursors")]
    public RawImage[] CursorList;

    [Header("Runner ref")]
    public RunnerGameplay RunnerScript;

    //TODO array of device or deviceId, null by default and same size than CursorList

    // Screen values for cursor limits
    private float fHalfHeight;
    private float fScreenWidth, fScreenHeight;

    void Start()
    {
        Assert.IsTrue(CursorList.Length == 8);
        Assert.IsTrue(CharacterBoxes.Length == 8);
        Assert.IsTrue(CharacterMaterials.Length == 8);

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
            Vector3 vMouseLocation = Input.mousePosition;
            KeepInCharacterPanel(ref vMouseLocation);
            CursorList[0].transform.position = vMouseLocation;
        }


        // Validate action
        // TODO handle all devices (based on input which try valide)
        int nCharac = CharacterBoxes.Length;
        for (int i = 0; i < nCharac; ++i)
        {
            if (CharacterBoxes[i].bounds.Contains(CursorList[0].transform.position))
            {
                RunnerScript.OnPlayerValidated(CharacterMaterials[i], i); // TODO replace i with player id
            }
        }

        //TODO
        // Cancel action
        //RunnerScript.OnPlayerCanceled(playerId);
    }

    private void KeepInCharacterPanel(ref Vector3 location)
    {
        location.x = Mathf.Max(location.x, 0.0f);
        location.x = Mathf.Min(location.x, fScreenWidth);
        location.y = Mathf.Max(location.y, fHalfHeight);
        location.y = Mathf.Min(location.y, fScreenHeight);
    }
}
