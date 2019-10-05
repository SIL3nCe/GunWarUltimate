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

    [Header("Jump")]
    public float m_fGravityScale;   //< The gravity scale of the player controller
    public float m_fJumpScale;  //< A scale to modulate the jump height of the player
    public bool m_bEnableDoubleJump;

    private CharacterController m_characterController; //< Store the CharacterController component of this GameObject
    public Vector3 m_vMoveDirection; //< The move direction is a Vector3 computed with the move vector of the player depending on its state (jumping/falling etc...)

    public bool m_bAlreadyDoubleJumped = false;

    // Start is called before the first frame update
    void Start()
    {
        //
        // Retrieve Character Controller
        m_characterController = GetComponent<CharacterController>();
        Assert.IsNotNull(m_characterController);
    }

    // Update is called once per frame
    void Update()
    {
        //
        // Check if the character is grounder
        if (GetComponent<CharacterController>().isGrounded)
        {
            //
            // Reset the double jump flag
            if (m_bAlreadyDoubleJumped)
            {
                m_bAlreadyDoubleJumped = false;
            }

            //
            // If the character is grounder we can jump pressing space for now
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //
                // To jump we multiply the gravity scale with the jumps scale
                // we do not forget to take into account the delta time
                m_vMoveDirection.y = (m_fJumpScale/* * m_fGravityScale*/) * Time.deltaTime;
            }

            //
            // Retrieve the direction of the Horizontal axis to set the player speed in X axis
            // Because we are grounded we simply use the player speed 
            m_vMoveDirection.x = Input.GetAxis("Horizontal") * m_fPlayerSpeed;

        }
        else
        {
            //
            // Retrieve the direction of the Horizontal axis to set the player speed in X axis
            // Because we are not grounded in this case, we use the air speed instead of the normal speed
            // because air control is different thant grounded control
            m_vMoveDirection.x = Input.GetAxis("Horizontal") * m_fPlayerAirSpeed;

            //
            // Because we are in air we substract the gravity of the player to make him fall
            m_vMoveDirection.y -= m_fGravityScale * Time.deltaTime;

            //
            // If the double jump is enabled
            if (m_bEnableDoubleJump)
            {
                //
                // If the player has not double jumped already (reset when grounded)
                if (!m_bAlreadyDoubleJumped)
                {
                    //
                    // If the player pressed the jump button
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        //
                        // When doing a double jump it is the same thing as the normal jump but we reduce the jump scale
                        // by diving it by 2
                        m_vMoveDirection.y = ( (m_fJumpScale)/* * m_fGravityScale*/) * Time.deltaTime;

                        //
                        // We don't forget to set the double jumped flag to prevent the player from double jumping all the time
                        m_bAlreadyDoubleJumped = true;
                    }
                }
            }
        }

        //
        // We now move the character controller by MoveDirection
        GetComponent<CharacterController>().Move(m_vMoveDirection); 
    }

    private void FixedUpdate()
    {
        // ...
    }
}
