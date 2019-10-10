using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPreviewRotator : MonoBehaviour
{
    private Transform transformInit;
    private const int iSpeed = 32;

    private void Start()
    {
        transformInit = transform;
    }

    private void Update()
    {
        float fDt = Time.deltaTime;
        transform.Rotate(iSpeed * Vector3.up * fDt);
    }

    public void ResetRotation()
    {
        transform.SetPositionAndRotation(transformInit.position, transform.rotation);
    }
}
