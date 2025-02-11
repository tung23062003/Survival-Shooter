using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float updateSpeed = 0.1f;
    [SerializeField] private float rangeAttack = 2.0f;
    [SerializeField] private float atkCooldown = 2.0f;

    private Animator animator;
    [HideInInspector] public NavMeshAgent agent;

    public bool isAttacking = false;
    private float lastTimeAtk;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private void Update()
    {
        

        if(Vector3.Distance(target.position, transform.position) <= rangeAttack && Time.time - lastTimeAtk >= atkCooldown)
        {
            transform.LookAt(target);
            animator.SetTrigger("isAttacking");
            isAttacking = true;
            agent.isStopped = true;
            lastTimeAtk = Time.time;
        }

        SetAnimation();
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new(updateSpeed);

        while (enabled)
        {
            agent.SetDestination(target.transform.position);

            yield return wait;
        }
    }

    private void SetAnimation()
    {
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.01f && !isAttacking);
    }
}
