using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(PlayerControllerRigidbody))]
public class HumanPlayer : Player
{
    private string[] horizontalAxes = { "P1_Horizontal", "P2_Horizontal", "P3_Horizontal", "P4_Horizontal" };
    private string[] verticalAxes = { "P1_Vertical", "P2_Vertical", "P3_Vertical", "P4_Vertical" };
    private string[] chargeButtons = { "P1_Charge", "P2_Charge", "P3_Charge", "P4_Charge" };
    private string[] crouchButtons = { "P1_Crouch", "P2_Crouch", "P3_Crouch", "P4_Crouch" };

    private PlayerControllerRigidbody controller;

    private void Awake()
    {
        controller = GetComponent<PlayerControllerRigidbody>();
    }

    private void Update()
    {
        if (controller.IsDisabled()) { return; }

        if (Input.GetButtonDown(chargeButtons[playerID]))
        {
            controller.StartCharging();
        }

        if (Input.GetButtonUp(chargeButtons[playerID]))
        {
            controller.FireProjectile();
        }

        if (Input.GetButtonDown(crouchButtons[playerID]))
        {
            controller.StartCrouch();
        }

        if (Input.GetButtonUp(crouchButtons[playerID]))
        {
            if (controller.IsCrouching()) { controller.EndCrouch(); }
        }
    }

    private void FixedUpdate()
    {
        if (controller.IsDisabled()) { return; }

        float horMove = Input.GetAxisRaw(horizontalAxes[playerID]);
        float vertMove = Input.GetAxisRaw(verticalAxes[playerID]);

        // if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadSceneAsync("TimScene"); }
        Vector3 moveVector = new Vector3(horMove, 0.0f, vertMove).normalized;
        bool crouch = Input.GetButton(crouchButtons[playerID]);

        controller.Move(moveVector);
        
        if (moveVector != Vector3.zero)
        {
            this.transform.forward = moveVector;
        }
    }
}
