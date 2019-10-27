using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerCharacterControllerRunner : MonoBehaviour
{
    [Header("Jump")]
    public float m_fJumpFactor = 10.0f;

    [Header("Sounds")]
    public AudioClip[] m_aAudioClipsSteps;
    public AudioClip[] m_aAudioClipsJump;
    public AudioClip m_audioClipLand;
    private AudioSource m_audioSource;
    private bool m_bCanPlayStepSound = true;

    //
    // private
    //
    private Vector3 m_vCurrentVelocity;
    private bool m_bGrounded = false;
    private Rigidbody m_rigidbody;
    private bool m_bCanDoubleJump = true;
    private float m_fDoubleJumpAttenuation = 1.0f;

    [Header("Inputs")]
    public InputAction m_inputActionJump;

    private bool m_bJumpedTriggered = false;

    public void OnEnable()
    {
        //
        //
        m_inputActionJump.Enable();
    }

    public void OnDisable()
    {
        //
        //
        m_inputActionJump.Disable();
    }

    // Start is called before the first frame update
    private void Start()
    { 
        //
        // Ensure rigidbody is present
        m_rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rigidbody);

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

        if (m_bCanPlayStepSound && m_bGrounded)
        {
            int iSound = Random.Range(0, m_aAudioClipsSteps.Length);
            m_audioSource.PlayOneShot(m_aAudioClipsSteps[iSound], 0.2f);
            m_bCanPlayStepSound = false;
            Invoke("ResetCanPlayStepSound", 0.2f);
        }

        if (m_inputActionJump.triggered)
        {
            m_bJumpedTriggered = true;
        }
    }

    private void FixedUpdate()
    {
        Vector3 vTargetVelocity = m_rigidbody.velocity;

        //
        // Jump
        // If we press the jump key we jump
        // Only if we are grounded
        if (m_bGrounded)
        {
            if (m_bJumpedTriggered)
            {
                m_bJumpedTriggered = false; //< Remove jump flag
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
            // Reset double jump attenuation
            m_fDoubleJumpAttenuation = 1.0f;
        }
        else
        {
            //
            // We do not hit a wall, we can double jump
            // If we press space
            //if (Input.GetKeyDown(KeyCode.Space))
            if (m_bJumpedTriggered)
            {
                m_bJumpedTriggered = false; //< Remove jump flag
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
        }

        //
        // We then SmoothDamp the velocity so it will automatically goe from the current value from the 
        // target velocity computed above.
        // See : https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html
        m_rigidbody.velocity = Vector3.SmoothDamp(m_rigidbody.velocity, vTargetVelocity, ref m_vCurrentVelocity, 0.01f);
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

    private void PlayJumpSound()
    {
        int iIndex = Random.Range(0, m_aAudioClipsJump.Length);
        m_audioSource.PlayOneShot(m_aAudioClipsJump[iIndex], 0.3f);        
    }

    private void ResetCanPlayStepSound()
    {
        m_bCanPlayStepSound = true;
    }
}
