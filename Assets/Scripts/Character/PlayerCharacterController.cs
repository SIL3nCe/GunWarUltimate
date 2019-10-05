using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerCharacterController : MonoBehaviour
{
    [Header("Movements")]
    public float m_fPlayerSpeed;
    public float m_fMovementsSmoothTime = 0.01f;

    [Header("Jump")]
    public float m_fJumpFactor = 10.0f;

    //
    // private
    //
    private Vector3 m_vCurrentVelocity;
    private bool m_bGrounded = false;
    private Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        //
        // Ensure rigidbody is present
        m_rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rigidbody);
    }

    // Update is called once per frame
    void Update()
    {
        //
        // Compute the grounded attribute
        ComputeGrounded();
    }

    private void FixedUpdate()
    {
        Vector3 vTargetVelocity = new Vector3(m_fPlayerSpeed * Input.GetAxis("Horizontal"), m_rigidbody.velocity.y);
        
        //
        // Jump
        // If we press the jump key we jump
        // Only if we are grounded
        if (m_bGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vTargetVelocity.y = m_fJumpFactor;
            }
        }

        //
        // We then SmoothDamp the velocity so it will automatically goe from the current value from the 
        // target velocity computed above.
        // See : https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html
        GetComponent<Rigidbody>().velocity = Vector3.SmoothDamp(m_rigidbody.velocity, vTargetVelocity, ref m_vCurrentVelocity, m_fMovementsSmoothTime);
    }

    private void ComputeGrounded()
    {
        //
        //
        Debug.DrawRay(transform.position + new Vector3(0.0f, 0.1f, 0.0f), Vector3.down * 0.3f);

        //
        // We cast a ray downward
        if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.1f, 0.0f), Vector3.down, 0.3f))
        {
            m_bGrounded = true;
        }
        else
        {
            m_bGrounded = false;
        }
    }
}
