using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour, ICharacter
{
    public EntityInfo entityInfo;

    [Header("Stats")]
    [SerializeField] private float updateSpeed = 0.1f;
    [SerializeField] private float rangeAttack = 2.5f;
    [SerializeField] private float atkCooldown = 2.0f;
    [SerializeField] private float damage = 5.0f;


    [Header("Attack Info")]
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask attackLayer;

    private Health health;
    private Transform target;
    private Animator animator;
    [HideInInspector] public NavMeshAgent agent;

    public bool isAttacking = false;
    private float lastTimeAtk;


    public virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        health.OnDead += Die;
    }

    private void OnDestroy()
    {
        health.OnDead -= Die;
    }


    public virtual void OnEnable()
    {
        ResetEnemy();
        health.ResetHealth();
        if (GameManager.Instance.player != null)
        {
            target = GameManager.Instance.player.transform;
            StartCoroutine(FollowTarget());
        }
    }

    private void ResetEnemy()
    {
        animator.Rebind();
        animator.Update(0f);
        isAttacking = false;
        lastTimeAtk = 0;
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
            Move(target.transform.position);

            yield return wait;
        }
    }

    public virtual void SetAnimation()
    {
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.01f && !isAttacking);
    }

    public virtual void Move(Vector3 direction)
    {
        if(target != null)
            agent.SetDestination(direction);
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
                player.TakeDamage(hitColliders[0].transform.position, damage);
                Debug.Log($"{gameObject.name} attack {player.name} kill {damage} damage");
            }
        }
    }

    public virtual void TakeDamage(Vector3 attackPos, float damage)
    {
        health.UpdateHealth(entityInfo, damage);
        GameEvent.OnTakeDamage?.Invoke(entityInfo, attackPos);

        Debug.Log($"{gameObject.name} take {damage} damage");
    }


    public virtual void Die()
    {
        gameObject.SetActive(false);
        GameEvent.OnKillEnemy?.Invoke();
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
