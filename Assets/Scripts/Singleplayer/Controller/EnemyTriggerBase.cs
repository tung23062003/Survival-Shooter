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
    public void test()
    {
        Debug.Log("test");
        enemyBase.isAttacking = false;
        enemyBase.agent.isStopped = false;
    }
}
