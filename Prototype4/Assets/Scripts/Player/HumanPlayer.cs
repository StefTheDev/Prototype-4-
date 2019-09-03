using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(PlayerController))]
public class HumanPlayer : Player
{
    public float moveForce = 12.0f;

    private string[] horizontalAxes = { "P1_Horizontal", "P2_Horizontal", "P3_Horizontal", "P4_Horizontal" };
    private string[] verticalAxes = { "P1_Vertical", "P2_Vertical", "P3_Vertical", "P4_Vertical" };
    private string[] chargeButtons = { "P1_Charge", "P2_Charge", "P3_Charge", "P4_Charge" };
    

    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        float horMove = Input.GetAxisRaw(horizontalAxes[playerID]);
        float vertMove = Input.GetAxisRaw(verticalAxes[playerID]);

        if (Input.GetButtonDown(chargeButtons[playerID]))
        {
            controller.StartCharging();
        }

        if (Input.GetButtonUp(chargeButtons[playerID]))
        {
            controller.FireProjectile();
        }

        if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadSceneAsync("TimScene"); }

        controller.Move(new Vector3(horMove, 0.0f, vertMove).normalized * moveForce);
    }
}
