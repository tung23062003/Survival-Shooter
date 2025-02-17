using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour, ICharacter
{
    [Header("Stats")]
    [SerializeField] private float updateSpeed = 0.1f;
    [SerializeField] private float rangeAttack = 2.5f;
    [SerializeField] private float atkCooldown = 2.0f;
    [SerializeField] private float damage = 5.0f;

    [SerializeField] private Health health;

    [Header("Attack Info")]
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask attackLayer;

    private Transform target;
    private Animator animator;
    [HideInInspector] public NavMeshAgent agent;

    public bool isAttacking = false;
    private float lastTimeAtk;


    public virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        health.OnDead += Die;
    }

    private void OnDestroy()
    {
        health.OnDead -= Die;
    }


    public virtual void OnEnable()
    {
        health.ResetHealth();
        if (GameManager.Instance.player != null)
        {
            target = GameManager.Instance.player;
            StartCoroutine(FollowTarget());
        }
    }

    public virtual void Update()
    {

        if (target == null)
            return;
        if(Vector3.Distance(target.position, transform.position) <= rangeAttack && Time.time - lastTimeAtk >= atkCooldown)
        {
            StartAttack();
            lastTimeAtk = Time.time;
        }

        SetAnimation();
    }

    public virtual IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new(updateSpeed);

        while (enabled)
        {
            Move();

            yield return wait;
        }
    }

    public virtual void SetAnimation()
    {
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.01f && !isAttacking);
    }

    public virtual void Move()
    {
        if(target != null)
            agent.SetDestination(target.transform.position);
    }

    public virtual void StartAttack()
    {
        transform.LookAt(target);
        animator.SetTrigger("isAttacking");
        isAttacking = true;
        agent.isStopped = true;
    }

    public virtual void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackPos.position, attackRadius, attackLayer);
        foreach (var hit in hitColliders)
        {
            if(hit.TryGetComponent(out PlayerController player))
            {
                player.TakeDamage(damage);
                Debug.Log($"{gameObject.name} attack {player.name} kill {damage} damage");
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        health.UpdateHealth(damage);
        Debug.Log($"{gameObject.name} take {damage} damage");
    }


    public virtual void Die()
    {
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} die");
    }

    public virtual void ResetAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
        Gizmos.color = Color.red;
    }
}
