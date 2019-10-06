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
    public int m_iWallJumpMaxCount = 4;
    private int m_iWallJumpRemainingCount;

    //
    // private
    //
    private Vector3 m_vCurrentVelocity;
    private bool m_bGrounded = false;
    private Rigidbody m_rigidbody;
    private bool m_bCanDoubleJump = true;
    private bool m_bHangOnWall = false;
    private float m_fHangOnWallDirection = 0.0f;
    private bool m_bWallJumping;
    private float m_fWallJumpDirection;
    private float m_fDoubleJumpAttenuation = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //
        // Ensure rigidbody is present
        m_rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rigidbody);

        //
        // Initialize / Reset
        ResetWallJump();
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
        //
        //
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
            // Reset wall jump
            ResetWallJump();

            EndWallJump();

            //
            // Reset double jump attenuation
            m_fDoubleJumpAttenuation = 1.0f;
        }
        else
        {
            bool bHitAWall = HitAWallTest(fPlayerHorizontalInput);
            bool bWallIsHangable = false;

            //
            //
            if (m_bWallJumping)
            {
                fPlayerHorizontalInput = m_fWallJumpDirection;
            }

            //
            // We test if we hit a wall
            //Debug.DrawRay(transform.position, new Vector3(fPlayerHorizontalInput, 0.0f, 0.0f), Color.red);
            // We test this if we do not already hanging ona wall
            if (!m_bHangOnWall)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, new Vector3(fPlayerHorizontalInput, 0.0f, 0.0f), out hit, 0.6f))
                {
                    bHitAWall = true;
                    if (hit.normal.y == 0.0f)
                    {
                        //
                        // We store the hang wall direction and the flag that we hang a wall
                        if (fPlayerHorizontalInput > 0.0f) { m_fHangOnWallDirection = -1.0f; }
                        if (fPlayerHorizontalInput < 0.0f) { m_fHangOnWallDirection = 1.0f; }
                        StartHangOn();
                        bWallIsHangable = true;
                    }
                }
            }
            else
            {
                //
                // We raycast in the hangonwalldirection (inverted) to test if we are still to the right distance of the wall
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, new Vector3(-m_fHangOnWallDirection, 0.0f, 0.0f), out hit, 0.6f))
                {
                    EndHangOn();
                }

                //
                // If we already hand on a wall, we test if we press the opposite direction button
                // NB : We test hangonwalldirection and player input is equal
                // because the hang on wall direction is already the opposite wall direction
                if (m_fHangOnWallDirection > 0.0f && fPlayerHorizontalInput > 0.0f)
                {
                    m_bCanDoubleJump = true;
                    m_fDoubleJumpAttenuation = 0.4f;
                    EndHangOn();
                }
                else if (m_fHangOnWallDirection < 0.0f && fPlayerHorizontalInput < 0.0f)
                {
                    m_bCanDoubleJump = true;
                    m_fDoubleJumpAttenuation = 0.4f;
                    EndHangOn();
                }
                else
                {
                    bHitAWall = true;
                    bWallIsHangable = true;
                }
            }

            //
            // If we hit a wall
            if (bHitAWall)
            {
                EndWallJump();

                //
                // If the wall is hangable
                if (bWallIsHangable)
                {
                    //
                    // We hang on the wall, so we stop the x velocity
                    vTargetVelocity.x = 0.0f;

                    //
                    //
                    vTargetVelocity.y = m_rigidbody.velocity.y / 1.6f;

                    //
                    // If the wall is hangable we can jump
                    if (Input.GetKeyDown(KeyCode.Space) && m_iWallJumpRemainingCount > 0)
                    {
                        //
                        // This is a wall jump, we jump in Y and in X
                        vTargetVelocity.y = m_fJumpFactor * 1.3f;
                        vTargetVelocity.x = m_fHangOnWallDirection * (m_fPlayerSpeed / 4.0f);

                        //
                        // TODO: Comment
                        m_iWallJumpRemainingCount--;

                        //
                        // We do not hang on anymore
                        EndHangOn();

                        //
                        //
                        m_bWallJumping = true;
                        m_fWallJumpDirection = m_fHangOnWallDirection;

                        //
                        // We unlock the double jump
                        m_bCanDoubleJump = true;
                        m_fDoubleJumpAttenuation = 0.6f;

                        //
                        //
                        Invoke("EndWallJump", 0.4f);
                    }
                }
                else
                {
                    //
                    // We hit a wall that is not climbable, we stop the x velocity to fall 
                    vTargetVelocity.x = 0.0f;
                }
            }
            else
            {
                //
                // We do not hit a wall, we can double jump
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
                        vTargetVelocity.y = m_fJumpFactor * 1.4f * m_fDoubleJumpAttenuation;

                        GetComponent<Animator>().SetBool("bFall", true);
                    }
                }

                //
                // If we do not hit a wall the velocity in x is our directio  * speed (it doesn't change)
                
                //
                // We reset the hang on wall values
                m_bHangOnWall = false;
                m_fHangOnWallDirection = 0.0f;
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
            transform.rotation = Quaternion.Euler(new Vector3(vRotation.x, 90.0f, vRotation.z));
        }
        else if (fMovementDirection < 0.0f)
        {
            Vector3 vRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(new Vector3(vRotation.x, -90.0f, vRotation.z));
        }
    }

    public void ResetWallJump()
    {
        m_iWallJumpRemainingCount = m_iWallJumpMaxCount;
    }

    public bool HitAWallTest(float fPlayerHorizontalInput)
    {
        //
        // Here we do 20 raycasts to be sure to detect walls hits
        float fDirection = fPlayerHorizontalInput > 0.0f ? 1.0f : -1.0f;
        int iNbRays = 20;

        for (int iRay = 0; iRay < iNbRays; ++iRay)
        {
            Debug.DrawLine(transform.position + new Vector3(0.0f, iRay * 0.1f, 0.0f), transform.position + new Vector3(0.0f, iRay * 0.1f, 0.0f) + new Vector3(fPlayerHorizontalInput, 0.0f, 0.0f));
            if (Physics.Raycast(transform.position + new Vector3(0.0f, iRay * 0.1f, 0.0f), new Vector3(fDirection, 0.0f, 0.0f), 0.6f))
            {
                return true;
            }
        }

        return false;
    }

    public void EndWallJump()
    {
        m_bWallJumping = false;
        m_fWallJumpDirection = 0.0f;
    }
}
