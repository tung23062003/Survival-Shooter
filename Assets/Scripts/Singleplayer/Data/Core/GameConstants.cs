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
    public static UnityEvent OnEnemySpawn;
}

public static class AddressableKey
{
    public const string ZOMBIE = "Assets/Prefabs/Enemy/Zombie.prefab";
    public const string LOCAL_PLAYER = "Assets/Prefabs/Local Player.prefab";
    public const string MAIN_SCENE_2 = "Assets/Scenes/MainScene2.unity";
}

public enum EnemyType
{
    Zombie
}