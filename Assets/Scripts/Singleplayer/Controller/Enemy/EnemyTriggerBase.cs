using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerBase : MonoBehaviour
{
    private EnemyBase enemyBase;

    private void Start()
    {
        enemyBase = GetComponentInParent<EnemyBase>();
    }
    public void ResetAttack()
    {
        Debug.Log("Attack Done");
        enemyBase.ResetAttack();
    }

    public void AttackTrigger()
    {
        enemyBase.Attack();
    }
}
