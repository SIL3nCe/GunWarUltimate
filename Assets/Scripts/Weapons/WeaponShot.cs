using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponShot : MonoBehaviour
{
    [SerializeField]
    private WeaponDataset WeaponDatas;

    [Tooltip("Bullet spawn location")]
    public GameObject muzzleSocket;
    [Tooltip("Shell spawn location")]
    public GameObject shellSocket;
    [Tooltip("Rocket to hide when shooting with rocket launcher")]
    public GameObject rocketMesh;

    private float firingRateDt;
    private float firingDt;

    [HideInInspector]
    public int RemainingAmmos;

    // Sounds
    private AudioSource m_audioSource;
    private bool m_bCanPlayNoAmmoSound = true;
    private bool m_bCanPlayWeaponSound = true;

    // Start is called before the first frame update
    void Start()
    {
        firingDt = 0.0f;
        firingRateDt = 1.0f / WeaponDatas.FiringRate;

        //
        // Retrieve Audio Source
        m_audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(m_audioSource);
    }

    void Update()
    {
        firingDt += Time.deltaTime;

        if (Debug.isDebugBuild)
        {
            if (Input.GetKey(KeyCode.C))
            {
                Shoot(1.0f, false);
            }
        }
    }

    public void Shoot(float fDamageMultiplier, bool bAmmoUnlimited)
    {
        if (firingDt > firingRateDt && RemainingAmmos > 0)
        {
            if (null != muzzleSocket && null != WeaponDatas.BulletPrefab)
            {
                // spawn bullet on socket location
                Vector3 bulletLocation = muzzleSocket.transform.position;
                Quaternion bulletRotation = muzzleSocket.transform.rotation;
                GameObject shotBullet = Instantiate(WeaponDatas.BulletPrefab, bulletLocation, bulletRotation);
                shotBullet.GetComponent<Rigidbody>().velocity = muzzleSocket.transform.right * WeaponDatas.BulletSpeed;
				shotBullet.transform.SetParent(transform, true);

				if (null != WeaponDatas.MuzzleFlash)
                {
                    Destroy(Instantiate(WeaponDatas.MuzzleFlash, bulletLocation, bulletRotation), 0.05f);
                }

                BulletGameplay bulletParams = shotBullet.GetComponent<BulletGameplay>();
                if (null != bulletParams)
                {
                    bulletParams.Initialize(fDamageMultiplier * WeaponDatas.BulletDamages, WeaponDatas.LifeTime, WeaponDatas.EjectionFactor, WeaponDatas.HitEffect);
                }

                firingDt = 0.0f;
				if(!bAmmoUnlimited)
				{
					RemainingAmmos--;
				}

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

                        if (RemainingAmmos > 0)
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
            if (null != shellSocket && null != WeaponDatas.BulletShellPrefab)
            {
                Vector3 shellLocation = shellSocket.transform.position;
                Quaternion shellRotation = shellSocket.transform.rotation;
                GameObject fireShell = Instantiate(WeaponDatas.BulletShellPrefab, shellLocation, shellRotation);
                fireShell.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * -3.0f;
				fireShell.transform.SetParent(transform, true);
            }

            //
            // Emit sound
            if (WeaponDatas.AudioClipsShot.Length > 0 && m_bCanPlayWeaponSound)
            {
                int iSound = Random.Range(0, WeaponDatas.AudioClipsShot.Length);
                m_audioSource.PlayOneShot(WeaponDatas.AudioClipsShot[iSound]);
                m_bCanPlayWeaponSound = false;
                Invoke("ResetCanPlayWeaponSound", 0.05f);
            }
        }
        else
        {
            if (WeaponDatas.AudioClipNoAmmo && m_bCanPlayNoAmmoSound)
            {
                m_audioSource.PlayOneShot(WeaponDatas.AudioClipNoAmmo);
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
    private void ResetNoAmmoSound()
    {
        m_bCanPlayNoAmmoSound = true;
    }

    private void ResetCanPlayWeaponSound()
    {
        m_bCanPlayWeaponSound = true;
    }

    public void SetRocketVisibility(bool bHide)
    {
        if (null != rocketMesh)
        {
            rocketMesh.SetActive(!bHide);
            MeshRenderer mesh = rocketMesh.GetComponent<MeshRenderer>();
            if (null != mesh)
            {
                Material[] materialList = mesh.materials;
                for (int i = 0; i < materialList.Length; ++i)
                {
                    Color newColor = materialList[i].color;
                    newColor.a = bHide ? 0.0f : 1.0f;
                    materialList[i].color = newColor;
                }
            }
        }
    }

    public void InitializeLoader()
    {
        // Set RemainingAmmos to LoaderSize, only used at spawn
        RemainingAmmos = WeaponDatas.LoaderSize;
    }
}
