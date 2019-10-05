using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShot : MonoBehaviour
{
    public GameObject   muzzleSocket;
    public GameObject   bulletPrefab;
    public float        bulletSpeed;

    private Vector3     vMuzzleLocation;

    // Start is called before the first frame update
    void Start()
    {
        vMuzzleLocation = muzzleSocket.transform.position;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}

    public void Shot()
    {
        // get muzzle socket orientation
        // spawn bullet on socket location
    }
}
