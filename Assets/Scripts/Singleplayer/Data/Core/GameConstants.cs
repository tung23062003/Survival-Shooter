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
    //public static UnityEvent<EntityInfo> OnDead = new();
    public static UnityEvent OnKillEnemy = new();
    public static UnityEvent OnWinLevel = new();
    public static UnityEvent OnLoseLevel = new();

    public static UnityEvent OnSpawning = new();
    public static UnityEvent<LevelInfo> OnSpawnDone = new();
    public static UnityEvent<string> OnCountdown = new();
    public static UnityEvent<LevelDataSO> OnLoadDataDone = new();
}

public static class AddressableKey
{
    //Entity
    public const string ZOMBIE = "Assets/Prefabs/Enemy/Zombie.prefab";
    public const string LOCAL_PLAYER = "Assets/Prefabs/Local Player.prefab";

    //UI
    public const string LEVEL_BTN = "Assets/Prefabs/UI/LevelBtnPrefab.prefab";

    //Scene
    public const string MAIN_SCENE_2 = "Assets/Scenes/MainScene2.unity";

    //SO
    public const string LEVEL_DATA_SO = "Assets/Scripts/Singleplayer/Data/Scriptable Oject/LevelDataSO.asset";

    //VFX
    public const string PLAYER_BULLET_COLLID_BASE_VFX = "Assets/Prefabs/VFX/Hit/Ice Hit .prefab";

    //SFX
    public const string PLAYER_FIRE_SFX = "Assets/Art/BigRookGames/_AssetPacks/Stylized Weapon Pack/M4 Scoped Assault Rifle/Audio/SFX_Assault Single Shot.wav";

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