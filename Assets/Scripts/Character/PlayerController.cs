﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;

[System.Serializable]
public class PlayerControllerCollisionsOptions
{
    [Header("Ground")]
    [Tooltip("The offset vertically that is used from the real player position to check for the ground. This value is automatically" +
        " inverted (* -1) to check under the player.")]
    public float m_fGroundCheckOffset = 1.0f;
    [Tooltip("The radius of the sphere that is used in a physic raycast to detect ground")]
    public float m_fGroundCheckRadius = 0.2f;

    [Header("Walls")]
    [Tooltip("The offset from the player position to check for walls in the two directions. In fact this value is applied to the X position of the player" +
        " in + and - direction to check for collisions in front of the player and backward.")]
    public float m_fWallCheckHorizontalOffset = 0.6f;
    [Tooltip("Identical to the check offset for the ground check, it offsets the vertical position of the checking of the walls")]
    public float m_fWallCheckVerticalOffset = 0.0f;
    [Tooltip("The radius of the sphere that is used to check for walls")]
    public float m_fWallCheckRadius = 0.2f;

    [Header("Layers")]
    [Tooltip("The layers to ignore for the raycast to determine if the player hits the ground or a wall")]
    // If all bits in the layerMask are on, we will collide against all colliders. If the layerMask = 0, we will never find any collisions with the ray.
    // Because here we want to raycast against al colliders except this one, we have to invert the bitmask, this is done in the start method
    public LayerMask m_ignoredLayersMask;
}

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
    [Tooltip("The height of a jump of the player when he is grounded (in meters).")]
    public float m_fJumpHeight = 1.0f;
    [Tooltip("The height of the second jump the player can do while in air (in meters)")]
    public float m_fDoubleJumpHeight = 1.0f;

    //
    // Collisions Options
    public PlayerControllerCollisionsOptions m_collisionsOptions;


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
        // We have to invert the bitmask for the collision layers 
        // This is done with '~'. By doing this we will process the collisions with all the collisions mask 
        // except the one(s) specified in the variable. This is useful because we want to do the raycast
        // with everything EXCEPT the players (layermask Character) to avoid detecting collisions (like ground and walls) when the sphere
        // checks overlap with the player capsule.
        m_collisionsOptions.m_ignoredLayersMask = ~m_collisionsOptions.m_ignoredLayersMask;
    }

    void Update()
    {
        //
        // Raycast to know if we are grounded
        if (Physics.CheckSphere(transform.position + new Vector3(0.0f, -m_collisionsOptions.m_fGroundCheckOffset, 0.0f), m_collisionsOptions.m_fGroundCheckRadius, m_collisionsOptions.m_ignoredLayersMask))
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

        //
        // We check if we are colliding with a wall
        // The check depends on the move input, because we only check in the direction the player is actually moving.
        // In fact the player can only start hanging on a wall when going in the direction of that wall
        if (Physics.CheckSphere(transform.position + new Vector3(m_collisionsOptions.m_fWallCheckHorizontalOffset * m_vMoveInput.x, m_collisionsOptions.m_fWallCheckVerticalOffset, 0.0f), m_collisionsOptions.m_fWallCheckRadius, m_collisionsOptions.m_ignoredLayersMask))
        {
            Debug.Log("Wall cast ! ");
        }
        else
        {
            //Debug.Log("")
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
                // The jump height is a parameter, generally it is < to the normal jump height
                // Add force for the jump
                m_rigidbody.AddForce(new Vector3(0.0f, Mathf.Sqrt(m_fDoubleJumpHeight * -2f * Physics.gravity.y), 0.0f), ForceMode.VelocityChange);
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
        // Draw ground check sphere
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0.0f, -m_collisionsOptions.m_fGroundCheckOffset, 0.0f), m_collisionsOptions.m_fGroundCheckRadius);

        //
        // Draw wall check sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(m_collisionsOptions.m_fWallCheckHorizontalOffset, m_collisionsOptions.m_fWallCheckVerticalOffset, 0.0f), m_collisionsOptions.m_fWallCheckRadius);
        Gizmos.DrawWireSphere(transform.position + new Vector3(-m_collisionsOptions.m_fWallCheckHorizontalOffset, m_collisionsOptions.m_fWallCheckVerticalOffset, 0.0f), m_collisionsOptions.m_fWallCheckRadius);
    }

}