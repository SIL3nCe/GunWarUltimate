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
    private bool m_bCanDoubleJump = true;
    private bool m_bHangOnWall = false;
    private float m_fHangOnWallDirection = 0.0f;

    public float m_fWallStamina = 1.3f;

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

        //
        //
        UpdateCharacterDirection(Input.GetAxis("Horizontal"));

        //
        //
        if (Input.GetAxis("Horizontal") != 0.0f)
        {
            GetComponent<Animator>().SetBool("bRunning", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("bRunning", false);
        }

        GetComponent<Animator>().SetBool("bFall", false);
    }

    private void FixedUpdate()
    {
        float fPlayerHorizontalInput = Input.GetAxis("Horizontal");
        Vector3 vTargetVelocity = new Vector3(m_fPlayerSpeed * fPlayerHorizontalInput, m_rigidbody.velocity.y);
        
        //
        // Jump
        // If we press the jump key we jump
        // Only if we are grounded
        if (m_bGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vTargetVelocity.y = m_fJumpFactor;

                GetComponent<Animator>().SetBool("bFall", true);
            }

            //
            // Reset the double jump
            m_bCanDoubleJump = true;

            //
            // Reset hang on wall because we grounded
            EndHangOn();

            //
            // Reset wall stamina
            m_fWallStamina = 1.3f;
        }
        else
        {
            //
            // If we press space
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //
                // If we can double jump
                if (m_bCanDoubleJump)
                {
                    //
                    // We can't double jump anymore
                    m_bCanDoubleJump = false;

                    //
                    // Add another jump velocity
                    vTargetVelocity.y = m_fJumpFactor * 1.4f;

                    GetComponent<Animator>().SetBool("bFall", true);
                }
            }

            //
            //
            if (fPlayerHorizontalInput != 0.0f)
            {
                Debug.DrawRay(transform.position, new Vector3(fPlayerHorizontalInput, 0.0f, 0.0f), Color.red);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, new Vector3(fPlayerHorizontalInput, 0.0f, 0.0f), out hit, 0.6f))
                {
                    //
                    // TODO: Comment 
                    // TODO: Need the normal.y check ??
                    if (!m_bHangOnWall && hit.normal.y == 0.0f)
                    {
                        m_fHangOnWallDirection = -fPlayerHorizontalInput;

                        //
                        // Start Hang On
                        StartHangOn();

                        GetComponent<Animator>().SetBool("bFall", false);
                    }
                }
                else
                {
                    m_bHangOnWall = false;
                }
            }

            if (m_bHangOnWall)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //
                    // This is a wall jump, we jump in Y and in X
                    vTargetVelocity.y = m_fJumpFactor * m_fWallStamina;
                    vTargetVelocity.x = m_fHangOnWallDirection * (m_fPlayerSpeed / 4.0f) * m_fWallStamina;

                    //
                    // TODO: Comment
                    if (m_fWallStamina > 0.0f)
                    {
                        m_fWallStamina -= 0.3f;
                    }else
                    {
                        m_fWallStamina = 0.0f;
                    }

                    //
                    // We do not hang on anymore
                    EndHangOn();
                }
                else
                {
                    //
                    // When hanging on a wall the y velocity is reduced
                    vTargetVelocity.y = m_rigidbody.velocity.y / 1.6f;
                }

                //GetComponent<Animator>().Play("HangOn");
            }
            else
            {
                GetComponent<Animator>().SetBool("bFall", true);
            }
        }

        //
        // We then SmoothDamp the velocity so it will automatically goe from the current value from the 
        // target velocity computed above.
        // See : https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html
        if (vTargetVelocity.x == 0.0f) { vTargetVelocity.x = m_rigidbody.velocity.x; }
        m_rigidbody.velocity = Vector3.SmoothDamp(m_rigidbody.velocity, vTargetVelocity, ref m_vCurrentVelocity, m_fMovementsSmoothTime);
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

    //
    // States
    //
    public void StartHangOn()
    {
        GetComponent<Animator>().SetBool("bHangOn", true);
        m_bHangOnWall = true;
    }

    public void EndHangOn()
    {
        GetComponent<Animator>().SetBool("bHangOn", false);
        m_bHangOnWall = false;
    }

    public void UpdateCharacterDirection(float fMovementDirection)
    {
        if (fMovementDirection > 0.0f)
        {
            Vector3 vRotation = transform.rotation.eulerAngles;
            //transform.rotation.SetEulerAngles(new Vector3(vRotation.x, 90.0f, vRotation.z));
            transform.rotation = Quaternion.Euler(new Vector3(vRotation.x, 90.0f, vRotation.z));
        }
        else if (fMovementDirection < 0.0f)
        {
            Vector3 vRotation = transform.rotation.eulerAngles;
            //transform.rotation.SetEulerAngles(new Vector3(vRotation.x, -90.0f, vRotation.z));
            transform.rotation = Quaternion.Euler(new Vector3(vRotation.x, -90.0f, vRotation.z));
        }
    }
}
