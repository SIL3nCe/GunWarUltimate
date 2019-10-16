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
    private bool m_bJump = false;

    void Start()
    {
        //
        // Retrieve the rigidbody (should always be there since there is a RequireComponent directive)
        m_rigidbody = GetComponent<Rigidbody>();
        Assert.AreNotEqual(m_rigidbody, null);
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.CheckSphere(transform.position, 2.0f))
        {
            m_bGrounded = true;
        }
        else
        {
            m_bGrounded = false;
        }
    }

    public void OnMove(InputValue inputValue)
    {
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
            // Source for the calculation to calibrate the jump : https://answers.unity.com/questions/854006/jumping-a-specific-height-using-velocity-gravity.html
            m_rigidbody.AddForce(new Vector3(0.0f, Mathf.Sqrt(m_fJumpHeight * -2f * Physics.gravity.y), 0.0f), ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        //
        // Move the player
        m_rigidbody.MovePosition(transform.position + new Vector3(m_vMoveInput.x, m_vMoveInput.y, 0.0f) * m_fPlayerSpeed * Time.deltaTime);
    }




}
