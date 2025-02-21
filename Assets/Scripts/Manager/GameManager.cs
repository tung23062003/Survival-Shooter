using System;
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
        //loading bar active
        GameEvent.OnLoading?.Invoke();

        AddressableManager.Instance.CreateAsset<ScriptableObject>(AddressableKey.LEVEL_DATA_SO, result =>
        {
            levelData = result as LevelDataSO;

            level = levelData.GetLevel(currentLevel);

            LoadLevel(level.level);
        });
    }

    private async void LoadLevel(int level)
    {
        var levelInfo = levelData.GetLevel(level);

        

        await spawner.SpawnPlayer(levelInfo);

        //loading bar deactive
        GameEvent.OnLoadDone?.Invoke(levelInfo);

        
    }

    public void StartCountdown(LevelInfo levelInfo)
    {
        StartCoroutine(Countdown(levelInfo));
    }
    
    private IEnumerator Countdown(LevelInfo levelInfo, Action onComplete = null)
    {
        int time = levelInfo.countDownTime;
        while(time >= 0)
        {
            if (time == 0)
                GameEvent.OnCountdown?.Invoke("<size=150>START</size>");
            else
                GameEvent.OnCountdown?.Invoke($"<color=#ccc>Enemy spawn in</color>\n<size=100>{time}</size>");
            time--;
            yield return new WaitForSeconds(1.0f);
        }
        GameEvent.OnCountdown?.Invoke(string.Empty);

        spawner.StartSpawnEnemies(levelInfo);
        onComplete?.Invoke();
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
