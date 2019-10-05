using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShot : MonoBehaviour
{
    public GameObject   muzzleSocket;
    public GameObject   bulletPrefab;
    public float        bulletSpeed;

    // Start is called before the first frame update
    void Start()
    { 
    }

    void FixedUpdate()
    {
        //Debug only
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shot();
        }
    }

    public void Shot()
    {
        Transform bulletTransfo = muzzleSocket.transform;

        // spawn bullet on socket location
        GameObject shotBullet = Instantiate(bulletPrefab, muzzleSocket.transform.position, muzzleSocket.transform.rotation);
        shotBullet.GetComponent<Rigidbody>().velocity = shotBullet.transform.forward * bulletSpeed;
    }
}
