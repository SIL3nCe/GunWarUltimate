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
        Vector3 bulletLocation = muzzleSocket.transform.position;
        Quaternion bulletRotation = muzzleSocket.transform.rotation;
        
        // spawn bullet on socket location
        GameObject shotBullet = Instantiate(bulletPrefab, bulletLocation, bulletRotation);

        shotBullet.GetComponent<Rigidbody>().velocity = gameObject.transform.right * bulletSpeed;
    }
}
