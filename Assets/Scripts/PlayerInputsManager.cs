using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsManager : MonoBehaviour
{
    public PlayerCharacterController m_player1;
    public PlayerCharacterController m_player2;

    private PlayerInputActions m_playerInputActions;

    private void Awake()
    {
        //m_playerInputActions = new PlayerInputActions();

        //foreach (var item in InputSystem.devices)
        //{
        //    Debug.Log(item);
        //}
        //InputAction inputActionJumpP1 = m_playerInputActions.Movements.Jump;
        //InputAction inputActionJumpP2 = inputActionJumpP1.Clone();
        //inputActionJumpP1.ApplyBindingOverridesOnMatchingControls(InputSystem.devices[2]);
        //inputActionJumpP2.ApplyBindingOverridesOnMatchingControls(InputSystem.devices[3]);

        //inputActionJumpP1.Enable();
        //inputActionJumpP2.Enable();

        //m_player1.m_inputActionJump = inputActionJumpP1;
        //m_player2.m_inputActionJump = inputActionJumpP2;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
