using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public GameObject player;
    [SerializeField] private SSpawner spawner;

    private LevelDataSO levelData;
    private LevelInfo level;

    private int currentLevel;
    private int enemyKillQuantity = 0;

    protected override void Awake()
    {
        base.Awake();

        if (!PlayerPrefs.HasKey("FirstTimePlay"))
        {
            PlayerPrefs.SetInt("FirstTimePlay", 1);
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.Save();
        }

        currentLevel = PlayerPrefs.GetInt("Level", 1);

        GameEvent.OnPlayerSpawn.AddListener(OnPlayerSpawn);
        GameEvent.OnKillEnemy.AddListener(CheckWinLevel);
    }

    private void OnDestroy()
    {
        GameEvent.OnPlayerSpawn.RemoveAllListeners();
        GameEvent.OnKillEnemy.RemoveAllListeners();
    }

    private void Start()
    {
        AddressableManager.Instance.CreateAsset<ScriptableObject>(AddressableKey.LEVEL_DATA_SO, result =>
        {
            levelData = result as LevelDataSO;

            level = levelData.GetLevel(currentLevel);

            LoadLevel(1);
        });
    }

    private async void LoadLevel(int level)
    {
        var levelInfo = levelData.GetLevel(level);

        //loading bar active
        GameEvent.OnLoading?.Invoke();

        await spawner.SpawnPlayer(levelInfo);

        //loading bar deactive
        GameEvent.OnLoadDone?.Invoke();

        spawner.StartSpawnEnemies(levelInfo);
    }


    private void OnPlayerSpawn(GameObject playerPrefab)
    {
        if(playerPrefab != null)
            player = playerPrefab;
    }

    private void CheckWinLevel()
    {
        enemyKillQuantity++;

        if (enemyKillQuantity >= level.enemyQuantity)
        {
            Debug.Log($"Win level {currentLevel}");
            GameEvent.OnWinLevel?.Invoke();
            SaveLevel();
        }
    }

    private void ResetKillEnemyQuantity()
    {
        enemyKillQuantity = 0;
    }

    private void SaveLevel()
    {
        PlayerPrefs.SetInt("Level", currentLevel + 1);
    }
}
