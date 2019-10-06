using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponShot : MonoBehaviour
{
    [Tooltip("Bullet spawn location")]
    public GameObject muzzleSocket;
    public GameObject bulletPrefab;

    [Tooltip("Shell spawn location")]
    public GameObject shellSocket;
    public GameObject bulletShellPrefab;

    [Tooltip("Muzzle Flash prefab to enable on shot")]
    public GameObject MuzzleFlash;

    [Tooltip("Rocket to hide when shooting with rocket launcher")]
    public GameObject rocketMesh;

    [Tooltip("Bullet/s")]
    public int firingRate;
    private float firingRateDt;
    private float firingDt;

    [Tooltip("Bullet start velocity")]
    public float bulletSpeed;

    [Tooltip("% of damage per bullet")]
    public float bulletDamages;

    [Tooltip("Number of bullets it can shoot")]
    public int loaderSize;

    [Header("Sounds")]
    public AudioClip m_audioClipPick;
    public AudioClip m_audioClipNoAmmo;
    public AudioClip[] m_aAudioClipsShot;
    private AudioSource m_audioSource;
    private bool m_bCanPlayNoAmmoSound = true;

    // Start is called before the first frame update
    void Start()
    {
        firingDt = 0.0f;
        firingRateDt = 1.0f / firingRate;

        //
        // Retrieve Audio Source
        m_audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(m_audioSource);
    }

    void Update()
    {
        firingDt += Time.deltaTime;

        //Debug only
        if (Input.GetKey(KeyCode.C))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (firingDt > firingRateDt && loaderSize > 0)
        {
            if (null != muzzleSocket && null != bulletPrefab)
            {
                // spawn bullet on socket location
                Vector3 bulletLocation = muzzleSocket.transform.position;
                Quaternion bulletRotation = muzzleSocket.transform.rotation;
                GameObject shotBullet = Instantiate(bulletPrefab, bulletLocation, bulletRotation);
                shotBullet.GetComponent<Rigidbody>().velocity = gameObject.transform.right * bulletSpeed;

                if (null != MuzzleFlash)
                {
                    Destroy(Instantiate(MuzzleFlash, bulletLocation, bulletRotation), 0.05f);
                }

                BulletGameplay bulletParams = shotBullet.GetComponent<BulletGameplay>();
                if (null != bulletParams)
                {
                    bulletParams.SetDamages(bulletDamages);
                }

                firingDt = 0.0f;
                loaderSize--;
               
                // hide rocket for firingRateDt time if rocket is set
                if (null != rocketMesh)
                {
                    MeshRenderer mesh = rocketMesh.GetComponent<MeshRenderer>();
                    if (null != mesh)
                    {
                        Material[] materialList = mesh.materials;
                        for (int i = 0; i < materialList.Length; ++i)
                        {
                            Color newColor = materialList[i].color;
                            newColor.a = 0.0f;
                            materialList[i].color = newColor;
                        }

                        if (loaderSize > 0)
                        {
                            StartCoroutine(UnHideRocket(mesh, firingRateDt * 0.9f));
                        }
                        else
                        {
                            rocketMesh.SetActive(false);
                        }
                    }
                }
            }

            // spawn shell on socket location
            if (null != shellSocket && null != bulletShellPrefab)
            {
                Vector3 shellLocation = shellSocket.transform.position;
                Quaternion shellRotation = shellSocket.transform.rotation;
                GameObject fireShell = Instantiate(bulletShellPrefab, shellLocation, shellRotation);
                fireShell.GetComponent<Rigidbody>().velocity = -gameObject.transform.forward * 3.0f;
            }

            //
            // Emit sound
            if (m_aAudioClipsShot.Length > 0)
            {
                int iSound = Random.Range(0, m_aAudioClipsShot.Length);
                m_audioSource.PlayOneShot(m_aAudioClipsShot[iSound]);
            }
            
        }
        else
        {
            if (m_audioClipNoAmmo && m_bCanPlayNoAmmoSound)
            {
                m_audioSource.PlayOneShot(m_audioClipNoAmmo);
                m_bCanPlayNoAmmoSound = false;
                Invoke("ResetNoAmmoSound", 0.6f);
            }
        }
    }

    IEnumerator UnHideRocket(MeshRenderer mesh, float delayTime)
    {
        if (null != mesh)
        {
            mesh.enabled = true;

            float alpha = mesh.material.color.a;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / delayTime)
            {
                Material[] materialList = mesh.materials;
                for (int i = 0; i < materialList.Length; ++i)
                {
                    Color newColor = materialList[i].color;
                    newColor.a = Mathf.Lerp(alpha, 1.0f, t);
                    materialList[i].color = newColor;
                }

                yield return null;
            }
        }
    }

    public void ResetNoAmmoSound()
    {
        m_bCanPlayNoAmmoSound = true;
    }
}
