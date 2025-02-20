using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Object/Create level data", order = 1)]
public class LevelDataSO : ScriptableObject
{
    public List<LevelInfo> levels;

    public int GetEnemyQuantityByLevel(int level)
    {
        return levels.Find(item => item.level == level).enemyQuantity;
    }

    public LevelInfo GetLevel(int level)
    {
        return levels.Find(item => item.level == level);
    }
}

[Serializable]
public class LevelInfo
{
    public int level;
    public int countDownTime;
    public int enemyQuantity;
    public float spawnDelay;
    public Vector3 playerSpawnPosition;
    public Vector3[] enemySpawnPosition;
}

/* In progress
[Serializable]
public class Level
{
    public int level;
    public List<Wave> waves;
}

[Serializable]
public class Wave
{
    public EnemyType enemyType;
    public int quantity;
}

*/

