using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelDataSO : ScriptableObject
{
    public List<Level> levels;
}

[Serializable]
public class Level
{
    public int enemyQuantity;
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

