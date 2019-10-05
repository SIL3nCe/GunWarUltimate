using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShot : MonoBehaviour
{
    public GameObject   muzzleSocket;
    public GameObject   shellSocket;
    public GameObject   bulletPrefab;
    public GameObject   bulletShellPrefab;
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
        if (null != muzzleSocket)
        {
            // spawn bullet on socket location
            Vector3 bulletLocation = muzzleSocket.transform.position;
            Quaternion bulletRotation = muzzleSocket.transform.rotation;
            GameObject shotBullet = Instantiate(bulletPrefab, bulletLocation, bulletRotation);
            shotBullet.GetComponent<Rigidbody>().velocity = gameObject.transform.right * bulletSpeed;
        }

        // spawn shell on socket location
        if (null != muzzleSocket)
        {
            Vector3 shellLocation = shellSocket.transform.position;
            Quaternion shellRotation = shellSocket.transform.rotation;
            GameObject fireShell = Instantiate(bulletShellPrefab, shellLocation, shellRotation);
            fireShell.GetComponent<Rigidbody>().velocity = -gameObject.transform.forward * 3.0f;
        }
    }
}
