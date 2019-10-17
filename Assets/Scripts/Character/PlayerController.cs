using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    //--------------------
    // PUBLIC
    //--------------------

    //
    // Movements
    [Header("Movements")]
    [Tooltip("The player speed. Relative value, in fact it is a multiplicator with the inputs for the movements.")]
    public float m_fPlayerSpeed = 1.0f;
    public float m_fJumpHeight = 1.0f;

    //
    // Ground check
    [Header("Ground Check")]
    [Tooltip("The layers to ignore for the raycast to determine if the player is grounded or not")]
    // If all bits in the layerMask are on, we will collide against all colliders. If the layerMask = 0, we will never find any collisions with the ray.
    // Because here we want to raycast against al colliders except this one, we have to invert the bitmask, this is done in the start method
    public LayerMask m_groundedIgnoreLayersMask;


    //--------------------
    // PRIVATE
    //--------------------

    //
    // Gameplay
    private bool m_bGrounded = false;

    //
    // Components
    private Rigidbody m_rigidbody;

    //
    // Inputs
    private Vector2 m_vMoveInput = Vector2.zero;
    private bool m_bCanDoubleJump = true;

    void Start()
    {
        //
        // Retrieve the rigidbody (should always be there since there is a RequireComponent directive)
        m_rigidbody = GetComponent<Rigidbody>();
        Assert.AreNotEqual(m_rigidbody, null);

        //
        // We have to invert the bitmask for the collision layers for the ground raycast
        // This is don with '~'. By doing this we will process the collisions with all the collisions mask 
        // except the one(s) specified in the variable
        m_groundedIgnoreLayersMask = ~m_groundedIgnoreLayersMask;
    }

    void Update()
    {
        //
        // Raycast to know if we are grounded
        if (Physics.CheckSphere(transform.position + new Vector3(0.0f, -1.0f, 0.0f), 0.2f, m_groundedIgnoreLayersMask))
        {
            m_bGrounded = true;

            //
            // We are grounded so we reset some variables
            m_bCanDoubleJump = true;
        }
        else
        {
            m_bGrounded = false;
        }

    }

    public void OnMove(InputValue inputValue)
    {
        //
        // Store the input value (2D vector axis)
        m_vMoveInput = inputValue.Get<Vector2>();
    }

    public void OnJump()
    {
        if (m_bGrounded)
        {
            //
            // We add force in the Y direction. We use the forcemode VelocityChange to directly impact the velocity of the rigidbody 
            // see : https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html
            // To summarize, the VeolcityChange directly set a velocity to the rigidbody without taking its mass into account.
            // Other force mode either take the mass into account (we do not care about the mass since all players will have the same mass, if one day
            // we decide to add different type of characters we will probably change the jump height but not the mass) or adds a countinuous acceleration
            // which we dont want because we want only an impulse (the impulse mode exists but it takes the mass into account).
            //
            // We have to do a simple calculation to calibrate the jump height. This is really important because it allows us to 
            // set a real jump height instead of just a jump force. Since we want to have some precision to calibrate the game this is 
            // an interesting thing, and it is pretty simple. Here is the source : 
            // https://answers.unity.com/questions/854006/jumping-a-specific-height-using-velocity-gravity.html
            //
            //
            // Last thing to do before the jump is to null the vertical velocity. This is very important because it smooth
            // the jump behaviour.
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0.0f);
            //
            //
            // Add force for the jump
            m_rigidbody.AddForce(new Vector3(0.0f, Mathf.Sqrt(m_fJumpHeight * -2f * Physics.gravity.y), 0.0f), ForceMode.VelocityChange);
        }
        else
        {
            //
            // If we are not grounded we can still double jump if we don't already double jumped
            if (m_bCanDoubleJump)
            {
                //
                // If we are not grounded we do the same thing as above. 
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0.0f);
                //
                //
                // The only difference is that we reduce the jump height.
                // For now we simply divide the jump height by 2, but we could add a parameter to control this as a second jump height.
                // TODO: If we want to configure a double jump height, add the variable and insert the value here instead of the calculation based
                // on the initial jump height
                // Add force for the jump
                m_rigidbody.AddForce(new Vector3(0.0f, Mathf.Sqrt((m_fJumpHeight / 2f) * -2f * Physics.gravity.y), 0.0f), ForceMode.VelocityChange);
                //
                // Do not forget to remove the double jump flag, we double jumped, we cant double jump anymore
                m_bCanDoubleJump = false;
            }
        }
    }

    private void FixedUpdate()
    {
        //
        // Move the player
        m_rigidbody.MovePosition(transform.position + new Vector3(m_vMoveInput.x, 0.0f, 0.0f) * m_fPlayerSpeed * Time.deltaTime);
    }

    public void OnDrawGizmos()
    {
        //
        // Draw grounded sphere
        Gizmos.DrawSphere(transform.position + new Vector3(0.0f, -1.0f, 0.0f), 0.2f);
    }

}
