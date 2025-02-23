using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameConstants
{
    public static string MenuScene = "MenuScene";
    public static string MainScene = "MainScene";
    public static string MainScene2 = "MainScene2";

    public static string playerTag = "Player";
}

public static class GameEvent
{
    public static UnityEvent<GameObject> OnPlayerSpawn = new();
    public static UnityEvent OnEnemySpawn = new();
    public static UnityEvent<EntityInfo, Vector3> OnTakeDamage = new();
    public static UnityEvent OnKillEnemy = new();
    public static UnityEvent OnWinLevel = new();

    public static UnityEvent OnSpawning = new();
    public static UnityEvent<LevelInfo> OnSpawnDone = new();
    public static UnityEvent<string> OnCountdown = new();
    public static UnityEvent<LevelInfo> OnLoadDataDone = new();
}

public static class AddressableKey
{
    public const string ZOMBIE = "Assets/Prefabs/Enemy/Zombie.prefab";
    public const string LOCAL_PLAYER = "Assets/Prefabs/Local Player.prefab";
    public const string MAIN_SCENE_2 = "Assets/Scenes/MainScene2.unity";

    public const string LEVEL_DATA_SO = "Assets/Scripts/Singleplayer/Data/Scriptable Oject/LevelDataSO.asset";

    public const string ZOMBIE_HIT_VFX = "Assets/Prefabs/VFX/Hit/Ice Hit .prefab";
}

public enum EntityType
{
    NONE = 0,
    Player = 1,
    Monster = 2,
}

public enum MonsterType
{
    Zombie = 1,
    MistFiend = 2,
    StoneGolem = 3,
    TigerDemon = 4,
    BullDemon = 5,
}