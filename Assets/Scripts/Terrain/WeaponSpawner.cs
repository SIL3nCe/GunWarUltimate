using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject m_spawnLocation;
    public GameObject m_weaponPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(m_weaponPrefab, m_spawnLocation.transform.position, gameObject.transform.rotation);
    }
}
