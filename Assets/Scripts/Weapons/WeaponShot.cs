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

    [Tooltip("Bullet/s")]
    public int      firingRate;
    private float   firingRateDt;
    private float   firingDt;

    [Tooltip("Bullet start velocity")]
    public float    bulletSpeed;

    [Tooltip("% of damage per bullet")]
    public float    bulletDamages;

    [Tooltip("Number of bullets it can shoot")]
    public int      loaderSize;

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

    void Shoot()
    {
        if (firingDt > firingRateDt && loaderSize > 0)
        {
            if (null != muzzleSocket)
            {
                // spawn bullet on socket location
                Vector3 bulletLocation = muzzleSocket.transform.position;
                Quaternion bulletRotation = muzzleSocket.transform.rotation;
                GameObject shotBullet = Instantiate(bulletPrefab, bulletLocation, bulletRotation);
                shotBullet.GetComponent<Rigidbody>().velocity = gameObject.transform.right * bulletSpeed;

                BulletParameters bulletParams = shotBullet.GetComponent<BulletParameters>();
                if (null != bulletParams)
                {
                    bulletParams.SetDamages(bulletDamages);
                }
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
            if (--loaderSize <= 0)
            { // Tell the player to drop the weapon
                //TODO
            }
        }
    }
}
