using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerCharacterController : MonoBehaviour
{

    [Header("Movement")]
    public float m_fPlayerSpeed;

    private Vector2 m_vCurrentSpeed;

    private Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        //
        // Retrieve Rigidbody
        m_rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rigidbody);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        m_rigidbody.AddForce(new Vector3(1.0f, 0.0f, 0.0f));
    }
}
