using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataset", menuName = "Weapon Parameters", order = 51)]
public class WeaponDataset : ScriptableObject
{
    [Tooltip("Number of bullets it can shoot")]
    public int LoaderSize;
    [Tooltip("Bullet/s")]
    public int FiringRate;
    public GameObject BulletShellPrefab;
    [Tooltip("Muzzle Flash prefab to enable on shot")]
    public GameObject MuzzleFlash;

    [Header("Bullets")]
    public GameObject BulletPrefab;
    [Tooltip("Prefab to Instantiate on hit")]
    public GameObject HitEffect;
    [Tooltip("% of damage per bullet")]
    public float BulletDamages;
    [Tooltip("Bullet start velocity")]
    public float BulletSpeed;
    [Tooltip("Time before bullet desappear if no hit")]
    public float LifeTime;
    [Tooltip("Factor applied to damage impulse on player hit")]
    public float EjectionFactor;

    [Header("Sounds")]
    public AudioClip AudioClipNoAmmo;
    public AudioClip[] AudioClipsShot;
}
