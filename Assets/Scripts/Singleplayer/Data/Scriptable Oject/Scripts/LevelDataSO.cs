using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Object/Create level data", order = 1)]
public class LevelDataSO : ScriptableObject
{
    public List<Level> levels;
}

[Serializable]
public class Level
{
    public int level;
    public int enemyQuantity;
    public float spawnDelay;
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

