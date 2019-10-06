using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
    public int m_iPlayerIndex;
    [Header("Movements")]
    public float m_fPlayerSpeed;
    public float m_fMovementsSmoothTime = 0.01f;

    [Header("Jump")]
    public float m_fJumpFactor = 10.0f;
    public int m_iWallJumpMaxCount = 4;
    private int m_iWallJumpRemainingCount;

    [Header("Sounds")]
    public AudioClip[] m_aAudioClipsSteps;
    public AudioClip[] m_aAudioClipsJump;
    public AudioClip m_audioClipLand;
    public AudioClip m_audioClipHang;
    public AudioClip m_audioClipNoWeapon;
    private AudioSource m_audioSource;
    private bool m_bCanPlayStepSound = true;
    private bool m_bCanPlayNoWeaponSound = true;

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

    public InputAction m_inputActionJump;
    public InputAction m_inputActionMove;
    public InputAction m_inputActionShoot;
    public InputAction m_inputActionThrow;

    //
    // Inputs
    //private PlayerInputActions m_playerInputActions;

    public void Awake()
    {

    }

    private void OnEnable()
    {
        //
        //
        //m_playerInputActions.Enable();
        m_inputActionJump.Enable();
        m_inputActionMove.Enable();
        m_inputActionShoot.Enable();
        m_inputActionThrow.Enable();
    }

    private void OnDisable()
    {
        //
        //
        //m_playerInputActions.Disable();
        m_inputActionJump.Disable();
        m_inputActionMove.Disable();
        m_inputActionShoot.Disable();
        m_inputActionThrow.Disable();
    }

    // Start is called before the first frame update
    private void Start()
    { 
        //
        // Ensure rigidbody is present
        m_rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rigidbody);

        //
        // Initialize / Reset
        ResetWallJump();

        //
        //
        m_audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(m_audioSource);
    }

    // Update is called once per frame
    private void Update()
    {
        //
        // Compute the grounded attribute
        ComputeGrounded();

        //
        //
        UpdateCharacterDirection(m_inputActionMove.ReadValue<float>());

        //
        //
        if (m_inputActionMove.ReadValue<float>() != 0.0f )
        {
            GetComponent<Animator>().SetBool("bRunning", true);

            //
            // Play the grounded sound
            if (m_bCanPlayStepSound && m_bGrounded)
            {
                int iSound = Random.Range(0, m_aAudioClipsSteps.Length);
                m_audioSource.PlayOneShot(m_aAudioClipsSteps[iSound], 0.2f);
                m_bCanPlayStepSound = false;
                Invoke("ResetCanPlayStepSound", 0.2f);
            }
        }
        else
        {
            GetComponent<Animator>().SetBool("bRunning", false);
        }

        GetComponent<Animator>().SetBool("bFall", false);

        //
        //
        if (m_inputActionShoot.ReadValue<float>() > 0.0f)
        {
            if (null != GetComponent<WeaponHolder>().GetCurrentWeapon())
            {
                GetComponent<WeaponHolder>().GetCurrentWeapon().GetComponent<WeaponShot>().Shoot();
            }
            else
            {
                if (m_bCanPlayNoWeaponSound)
                {
                    m_audioSource.PlayOneShot(m_audioClipNoWeapon, 0.2f);
                    m_bCanPlayNoWeaponSound = false;
                    Invoke("ResetCanPayNoWeaponSound", 0.6f);
                }
            }
        }

        //
        //
        if (m_inputActionThrow.triggered)
        {
            GetComponent<WeaponHolder>().DropWeapon();
        }
    }

    private void FixedUpdate()
    {
        //
        //
        //float fPlayerHorizontalInput = Input.GetAxis("Horizontal");
        //Vector3 vTargetVelocity = new Vector3(m_fPlayerSpeed * fPlayerHorizontalInput, m_rigidbody.velocity.y);

        Vector3 vTargetVelocity = Vector3.zero;
        float fPlayerHorizontalInput = m_inputActionMove.ReadValue<float>();
        vTargetVelocity.x = m_inputActionMove.ReadValue<float>() * m_fPlayerSpeed;
        vTargetVelocity.y = m_rigidbody.velocity.y;
        
        //
        // Jump
        // If we press the jump key we jump
        // Only if we are grounded
        if (m_bGrounded)
        {
            if (m_inputActionMove.ReadValue<float>() == 0.0f)
            {
                vTargetVelocity.x = 0.0f;
                m_rigidbody.velocity = new Vector3(0.0f, m_rigidbody.velocity.y, m_rigidbody.velocity.z);
            }

            //if (Input.GetKeyDown(KeyCode.Space))
            if (m_inputActionJump.triggered)
            {
                //
                // Set the velocity for the jump
                vTargetVelocity.y = m_fJumpFactor;

                //
                // Play jump sound
                PlayJumpSound();
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

            //
            // TODO; Comment
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
                    if (m_inputActionJump.triggered && m_iWallJumpRemainingCount > 0)
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
                        // Play jump sound
                        PlayJumpSound();

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
                //if (Input.GetKeyDown(KeyCode.Space))
                if (m_inputActionJump.triggered)
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

                        //
                        // Play jump sound
                        PlayJumpSound();
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
            if (!m_bGrounded)
            {
                m_audioSource.PlayOneShot(m_audioClipLand, 0.2f);
            }
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
    private void StartHangOn()
    {
        GetComponent<Animator>().SetBool("bHangOn", true);
        m_bHangOnWall = true;

        //
        // Play sound
        m_audioSource.PlayOneShot(m_audioClipHang, 0.3f);
    }

    private void EndHangOn()
    {
        GetComponent<Animator>().SetBool("bHangOn", false);
        m_bHangOnWall = false;
    }

    private void UpdateCharacterDirection(float fMovementDirection)
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

    private void ResetWallJump()
    {
        m_iWallJumpRemainingCount = m_iWallJumpMaxCount;
    }

    private bool HitAWallTest(float fPlayerHorizontalInput)
    {
        //
        // Here we do 20 raycasts to be sure to detect walls hits
        float fDirection = fPlayerHorizontalInput > 0.0f ? 1.0f : -1.0f;
        int iNbRays = 20;

        //
        // Cast all the rays and display the debugged ray
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

    private void EndWallJump()
    {
        m_bWallJumping = false;
        m_fWallJumpDirection = 0.0f;
    }

    private void PlayJumpSound()
    {
        int iIndex = Random.Range(0, m_aAudioClipsJump.Length);
        m_audioSource.PlayOneShot(m_aAudioClipsJump[iIndex], 0.3f);        
    }

    private void ResetCanPlayStepSound()
    {
        m_bCanPlayStepSound = true;
    }
    
    private void ResetCanPayNoWeaponSound()
    {
        m_bCanPlayNoWeaponSound = true;
    }
}
