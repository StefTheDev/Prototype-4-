using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(PlayerControllerRigidbody))]
public class HumanPlayer : Player
{
    private string[] horizontalAxes = { "P1_Horizontal", "P2_Horizontal", "P3_Horizontal", "P4_Horizontal", "P5_Horizontal", "P6_Horizontal", "P7_Horizontal", "P8_Horizontal" };
    private string[] verticalAxes = { "P1_Vertical", "P2_Vertical", "P3_Vertical", "P4_Vertical", "P5_Vertical", "P6_Vertical", "P7_Vertical", "P8_Vertical" };
    private string[] chargeButtons = { "P1_Charge", "P2_Charge", "P3_Charge", "P4_Charge", "P5_Charge", "P6_Charge", "P7_Charge", "P8_Charge" };
    private string[] crouchButtons = { "P1_Crouch", "P2_Crouch", "P3_Crouch", "P4_Crouch", "P5_Crouch", "P6_Crouch", "P7_Crouch", "P8_Crouch" };

    private PlayerControllerRigidbody myController;

    private void Awake()
    {
        myController = GetComponent<PlayerControllerRigidbody>();
    }

    private void Update()
    {
        if (myController.IsDisabled()) { return; }

        if (Input.GetButton(chargeButtons[playerID]))
        {
            myController.StartCharging();
        }

        if (Input.GetButtonUp(chargeButtons[playerID]))
        {
            myController.FireProjectile();
        }

        if (Input.GetButtonDown(crouchButtons[playerID]))
        {
            myController.StartCrouch();
        }

        if (Input.GetButtonUp(crouchButtons[playerID]))
        {
            if (myController.IsCrouching()) { myController.EndCrouch(); }
        }
    }

    private void FixedUpdate()
    {
        if (myController.IsDisabled()) { return; }

        float horMove = Input.GetAxisRaw(horizontalAxes[playerID]);
        float vertMove = Input.GetAxisRaw(verticalAxes[playerID]);

        // if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadSceneAsync("TimScene"); }
        Vector3 moveVector = new Vector3(horMove, 0.0f, vertMove).normalized;
        bool crouch = Input.GetButton(crouchButtons[playerID]);

        myController.Move(moveVector);
        
        if (moveVector != Vector3.zero)
        {
            this.transform.forward = moveVector;
        }
    }
}
