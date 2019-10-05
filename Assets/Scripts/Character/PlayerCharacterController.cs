using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterController : MonoBehaviour
{

    [Header("Movement")]
    public float m_fPlayerSpeed;    //< The speed of the player when grounded
    public float m_fPlayerAirSpeed; //< The speed of the player when he is in air

    [Header("Inputs")]
    public string m_stringAxisHorizontal;
    public KeyCode m_keyCodeJump;
    public KeyCode m_keyCodeShoot;

    [Header("Jump")]
    public float m_fGravityScale;   //< The gravity scale of the player controller
    public float m_fJumpScale;  //< A scale to modulate the jump height of the player
    public bool m_bEnableDoubleJump;

    private CharacterController m_characterController; //< Store the CharacterController component of this GameObject
    public Vector3 m_vMoveDirection; //< The move direction is a Vector3 computed with the move vector of the player depending on its state (jumping/falling etc...)

    //public ParticleSystem m_hangOnWallParticleSystem;

    private bool m_bFalling = false;    //< True if the player is falling
    private bool m_bAlreadyDoubleJumped = false;    //< Boolean that is a true when the player already double jumped
                                                    //  since he left the ground. False if he hasn't double jumped since he left
                                                    //  the ground
    private bool m_bHangOnWall = false; //< Boolean that is true if the player is hanging on a wall (if he is colliding on a wall
                                        //  while going on the same direction and falling)
    private bool m_bWallJumping = false;    //< True when the player jsute performed a wall jump
    private float m_fHangOnWallDirection = 0.0f;
    private float m_fWallJumpDirection = 0.0f; //< The direction of the current wall jump

    // Start is called before the first frame update
    void Start()
    {
        //
        // Retrieve Character Controller
        m_characterController = GetComponent<CharacterController>();
        Assert.IsNotNull(m_characterController);

        //m_hangOnWallParticleSystem.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //
        //
        float fPlayerInputHorizontal = Input.GetAxis(m_stringAxisHorizontal);

        //
        // Check if the character is grounder
        if (GetComponent<CharacterController>().isGrounded)
        {
            UpdateGrounded(fPlayerInputHorizontal);
        }
        else
        {
            UpdateAir(fPlayerInputHorizontal);
        }

        //
        // We now move the character controller by MoveDirection
        GetComponent<CharacterController>().Move(m_vMoveDirection); 
    }

    private void FixedUpdate()
    {
        // ...
    }

    private void UpdateGrounded(float fPlayerInputHorizontal)
    {
        bool bSloped = false;
        RaycastHit raycastHitInfos;
        if (Physics.Raycast(transform.position, new Vector3(0.0f, -1.0f, 0.0f), out raycastHitInfos, 1.0f))
        {
            Debug.Log(raycastHitInfos.normal);
            if (raycastHitInfos.normal != new Vector3(0.0f, 1.0f, 0.0f))
            {
                bSloped = true;
            }
        }

        //
        //
        m_vMoveDirection.y = 0.0f;
        m_bWallJumping = false;

        //
        // Reset the double jump flag
        if (m_bAlreadyDoubleJumped)
        {
            m_bAlreadyDoubleJumped = false;
        }

        //
        // Retrieve the direction of the Horizontal axis to set the player speed in X axis
        // Because we are grounded we simply use the player speed 
        if (bSloped)
        {
            m_vMoveDirection.y = -100.0f;
        }else
        {
            m_vMoveDirection.y = 0.0f;
        }

        //
        // If the character is grounder we can jump pressing space for now
        if (Input.GetKeyDown(m_keyCodeJump))
        {
            //
            // To jump we multiply the gravity scale with the jumps scale
            // we do not forget to take into account the delta time
            m_vMoveDirection.y = m_fJumpScale * Time.deltaTime;
        }

        m_vMoveDirection.x = fPlayerInputHorizontal * m_fPlayerSpeed * Time.deltaTime;
    }

    private void UpdateAir(float fPlayerInputHorizontal)
    {
        //
        // 
        if (m_vMoveDirection.y < 0.0f)
        {
            m_bFalling = true;
        }else
        {
            m_bFalling = false;
        }

        //
        // If we are wall jumping we remove the opposite direction of the wall jump to the input axis
        // so that the player can't go in thath direction during wall jump
        if (m_bWallJumping)
        {
            fPlayerInputHorizontal = m_fWallJumpDirection;
        }

        //
        // Set the horizontal move direction of the player
        // Because we are not grounded in this case, we use the air speed instead of the normal speed
        // because air control is different thant grounded control
        if (m_bFalling)
        {
            m_vMoveDirection.x = fPlayerInputHorizontal * (m_fPlayerAirSpeed * 0.6f) * Time.deltaTime;
        }
        else
        {
            m_vMoveDirection.x = fPlayerInputHorizontal * m_fPlayerAirSpeed * Time.deltaTime;
        }

        //
        // We tes if we hang on a wall
        RaycastHit raycastHitInfos;
        if (Physics.Raycast(transform.position, new Vector3(m_vMoveDirection.x, 0.0f, 0.0f), out raycastHitInfos, 0.6f))
        {
            //
            //
            m_bHangOnWall = true;
        }
        else
        {
            m_bHangOnWall = false;
        }

        //
        // Because we are in air we substract the gravity of the player to make him fall
        // If the player is hanging on a wall, we reduce its falling speed (wall jump system)
        if (m_bHangOnWall && m_bFalling)
        {
            m_vMoveDirection.y -= (m_fGravityScale / 2.0f) * Time.deltaTime;
        }
        else
        {
            m_vMoveDirection.y -= m_fGravityScale * Time.deltaTime;
        }

        //
        // If the double jump is enabled
        if (m_bEnableDoubleJump)
        {
            //
            // If the player has not double jumped already (reset when grounded)
            // AND that he is not hanging on a wall
            if (!m_bAlreadyDoubleJumped && !m_bHangOnWall)
            {
                //
                // If the player pressed the jump button
                if (Input.GetKeyDown(m_keyCodeJump))
                {
                    //
                    // When doing a double jump it is the same thing as the normal jump but we reduce the jump scale
                    // by diving it by 2
                    m_vMoveDirection.y = ((m_fJumpScale)/* * m_fGravityScale*/) * Time.deltaTime;

                    //
                    // We don't forget to set the double jumped flag to prevent the player from double jumping all the time
                    m_bAlreadyDoubleJumped = true;
                }
            }
        }

        //
        // If the player is hanging on a wall we can wall jump
        if (m_bHangOnWall && m_bFalling)
        {
            //
            // IF the player press space
            if (Input.GetKeyDown(m_keyCodeJump))
            {
                //
                // The player will do a wall jump, he jumps a certain height and 
                // in the opposite direction of the wall he is hanging on
                if (m_vMoveDirection.x > 0.0f)
                {
                    m_vMoveDirection.x = -(/*m_fPlayerSpeed **/ 10.0f) * Time.deltaTime;
                    m_fWallJumpDirection = -1.0f;
                }
                else
                {
                    m_vMoveDirection.x = (/*m_fPlayerSpeed **/ 10.0f) * Time.deltaTime;
                    m_fWallJumpDirection = 1.0f;
                }

                //
                // The move direction y is also set to jump height
                m_vMoveDirection.y = (m_fJumpScale/* * m_fGravityScale*/) * Time.deltaTime;
                m_bWallJumping = true;
                Invoke("WallJumpRecoveryTimeEnd", 0.6f);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0.6f, 0.0f, 0.0f));
    }

    private void WallJumpRecoveryTimeEnd()
    {
        Debug.Log("WallJumpEnd");
        m_bWallJumping = false;
    }
}
