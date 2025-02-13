using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Character Controller Settings")]
    public float jumpForce = 8.0f;
    public float acceleration = 10.0f;
    public float speed = 2.0f;
    public float rotationSpeed = 15.0f;
    public float viewDownRotaionSpeed = 50.0f;

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.05f;


    private Rigidbody rb;
    public bool isGrounded;
    public bool isJumping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.up * -1, groundCheckDistance, groundLayer);
    }

    private void Spawn() { }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    public void Move(Vector3 direction)
    {
        Vector3 targetVelocity = direction * speed;

        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
    }

    public void Rotate(float rotationY)
    {
        transform.Rotate(0, rotationY * Time.deltaTime * rotationSpeed, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(groundCheckPoint.position, Vector3.up * -1 * groundCheckDistance);
        Gizmos.color = Color.red;
    }
}
