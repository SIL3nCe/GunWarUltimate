using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGameplay : MonoBehaviour
{
    [Tooltip("Prefab to Instantiate on hit")]
    public GameObject HitEffect;

    [Tooltip("Time before bullet desappear if no hit")]
    public float LifeTime;

    private float Damages; // Set by weapon on shot
    private bool IsRocket;
    
    void Start()
    {
        IsRocket = null != HitEffect && HitEffect.name.Contains("Rocket");

        Invoke("OnBulletLifeTimeSpend", LifeTime);
    }

    public void SetDamages(float InDamages)
    {
        Damages = InDamages;
    }

    void OnBulletLifeTimeSpend()
    {
        if (IsRocket)
        {
            TriggerHitEffect();
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsRocket)
        {
            TriggerHitEffect();
        }
        else
        {
            PlayerGameplay player = collision.gameObject.GetComponent<PlayerGameplay>();
            if (null != player)
            {
                player.TakeDamages(Damages);
                TriggerHitEffect();
            }
        }

        Destroy(gameObject);
    }

    private void TriggerHitEffect()
    {
        if (null != HitEffect)
        {
            Vector3 bulletLocation = gameObject.transform.position;
            Quaternion bulletRotation = gameObject.transform.rotation;
            GameObject hitEffectObject = Instantiate(HitEffect, bulletLocation, bulletRotation);
            RocketExplosion rocketScript = hitEffectObject.GetComponent<RocketExplosion>();
            if (null != rocketScript)
            {
                rocketScript.SetDamages(Damages);
            }
        }
    }
}
