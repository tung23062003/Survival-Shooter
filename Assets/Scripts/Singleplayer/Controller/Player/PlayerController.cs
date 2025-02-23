using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public EntityInfo entityInfo;
    [Header("Character Controller Settings")]
    public float jumpForce = 8.0f;
    public float acceleration = 10.0f;
    public float speed = 2.0f;
    public float rotationSpeed = 15.0f;
    public float viewDownRotaionSpeed = 50.0f;

    [Header("Stats")]
    [SerializeField] private float damage = 10.0f;

    [Header("Attack Info")]
    [SerializeField] private float atkCoundown = 0.5f;
    [SerializeField] private float hitDistance = 100.0f;
    [SerializeField] private Transform aimPoint;
    [SerializeField] private LayerMask atkLayer;


    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.05f;

    private Health health;
    private float lastTimeAtk;
    private Rigidbody rb;
    private PlayerCamera playerCamera;
    public bool isGrounded;
    public bool isJumping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<PlayerCamera>();
        health = GetComponent<Health>();

        health.ResetHealth();
    }

    private void OnEnable()
    {
        health.ResetHealth();
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


    public void TakeDamage(Vector3 attackPos, float damage)
    {
        health.UpdateHealth(entityInfo, damage);
        GameEvent.OnTakeDamage?.Invoke(entityInfo, attackPos);

        Debug.Log($"{gameObject.name} take {damage} damage");
    }

    public void Attack()
    {
        if (Time.time - lastTimeAtk < atkCoundown)
            return;

        var aimForwardVector = playerCamera.transform.forward;

        bool isHitOtherPlayer = false;

        if(Physics.Raycast(aimPoint.position, aimForwardVector, out RaycastHit hitInfo, hitDistance, atkLayer))
        {
            if(hitInfo.collider.TryGetComponent(out EnemyBase enemy))
            {
                enemy.TakeDamage(hitInfo.point, damage);
            }
            else
                VFXManager.Instance.PlayVFX_Addressable(AddressableKey.PLAYER_ATTACKBASE_VFX, hitInfo.point, Quaternion.identity);

            isHitOtherPlayer = true;
        }


        if (isHitOtherPlayer)
            Debug.DrawRay(aimPoint.position, aimForwardVector * hitDistance, Color.red, 1);
        else
            Debug.DrawRay(aimPoint.position, aimForwardVector * hitDistance, Color.green, 1);

        lastTimeAtk = Time.time;
    }

    public void Die()
    {
        Debug.Log($"{transform.name} die");
    }
}
