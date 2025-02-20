using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SSpawner : MonoBehaviour
{
    [SerializeField] private Button spawnEnemBtn;
    [SerializeField] private Button spawnPlayerBtn;

    //Spawn Pos
    [SerializeField] private Transform playerSpawnPos;
    [SerializeField] private Transform[] enemySpawnPos;

    // Start is called before the first frame update
    void Start()
    {
        spawnEnemBtn.onClick.AddListener(() =>
        {
            Vector3 pos = Vector3.zero;
            if (enemySpawnPos.Length > 0)
                pos = enemySpawnPos[UnityEngine.Random.Range(0, enemySpawnPos.Length)].position;
            AddEnemy(AddressableKey.ZOMBIE, pos, Quaternion.identity);
        });


        spawnPlayerBtn.onClick.AddListener(() =>
        {
            AddPlayer(AddressableKey.LOCAL_PLAYER, playerSpawnPos.position, Quaternion.identity);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector3 pos = Vector3.zero;
            if (enemySpawnPos.Length > 0)
                pos = enemySpawnPos[UnityEngine.Random.Range(0, enemySpawnPos.Length)].position;
            AddEnemy(AddressableKey.ZOMBIE, pos, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            AddPlayer(AddressableKey.LOCAL_PLAYER, playerSpawnPos.position, Quaternion.identity);
        }
    }

    public void AddPlayer(string key, Vector3 position, Quaternion rotation, Action onComplete = null, Transform parent = null)
    {
        AddressableManager.Instance.CreateAsset<GameObject>(key, result =>
        {
            var player = Instantiate(result, position, rotation, parent);
            player.SetActive(true);

            onComplete?.Invoke();
            GameEvent.OnPlayerSpawn.Invoke(player);
        });
    }

    public void AddEnemy(string key, Vector3 position, Quaternion rotation, Action onComplete = null, Transform parent = null)
    {
        AddressableManager.Instance.CreateAsset<GameObject>(AddressableKey.ZOMBIE, result =>
        {
            var enemy = ObjectPool.Instance.GetObject(result);
            enemy.SetActive(true);
            enemy.transform.SetPositionAndRotation(position, rotation);
            enemy.transform.SetParent(parent);

            onComplete?.Invoke();
            //var enemyBase = enemy.GetComponent<EnemyBase>();
        });
    }

    public void AddEnemyRandomPosition(string key, Vector3[] enemySpawnPos)
    {
        Vector3 pos = Vector3.zero;
        if (enemySpawnPos.Length > 0)
            pos = enemySpawnPos[UnityEngine.Random.Range(0, enemySpawnPos.Length)];
        AddEnemy(key, pos, Quaternion.identity);
    }

    public void StartSpawnEnemies(LevelInfo levelInfo)
    {
        StartCoroutine(SpawnEnemies(levelInfo));
    }

    public IEnumerator SpawnEnemies(LevelInfo levelInfo)
    {
        int spawnedEnemies = 0;
        while (spawnedEnemies < levelInfo.enemyQuantity)
        {
            AddEnemyRandomPosition(AddressableKey.ZOMBIE, levelInfo.enemySpawnPosition);
            spawnedEnemies++;
            yield return new WaitForSeconds(levelInfo.spawnDelay);
        }
    }

    public async Task SpawnPlayer(LevelInfo level)
    {
        AddPlayer(AddressableKey.LOCAL_PLAYER, level.playerSpawnPosition, Quaternion.identity);
        await Task.Yield();
    }
}
