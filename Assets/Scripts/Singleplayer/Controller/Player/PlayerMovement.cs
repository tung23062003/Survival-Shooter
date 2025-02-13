using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;
    PlayerCamera playerCamera;
    Vector2 moveInput;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerCamera = GetComponentInChildren<PlayerCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = playerCamera.transform.forward;
        Quaternion rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
        transform.rotation = rotation;

        //Move
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        moveDirection.Normalize();

        playerController.Move(moveDirection);

        //Jump
        if (playerController.isJumping)
        {
            playerController.Jump();
            playerController.isJumping = false;
        }

    }

    public void SetMoveInputVector(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }
}
