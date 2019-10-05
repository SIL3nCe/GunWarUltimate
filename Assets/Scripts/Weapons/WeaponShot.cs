using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShot : MonoBehaviour
{
    [Tooltip("Bullet spawn location")]
    public GameObject   muzzleSocket;
    public GameObject   bulletPrefab;

    [Tooltip("Shell spawn location")]
    public GameObject   shellSocket;
    public GameObject   bulletShellPrefab;

    [Tooltip("Bullet start velocity")]
    public float        bulletSpeed;

    [Tooltip("Bullet/s")]
    public int          firingRate;
    private float       firingRateDt;
    private float       firingDt;

    // Start is called before the first frame update
    void Start()
    {
        firingDt = 0.0f;
        firingRateDt = 1.0f / firingRate;
    }

    void Update()
    {
        firingDt += Time.deltaTime;

        //Debug only
        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (firingDt > firingRateDt)
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

            firingDt = 0.0f;
        }
    }
}
