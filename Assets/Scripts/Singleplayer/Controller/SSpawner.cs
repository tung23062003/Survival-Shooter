using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SSpawner : MonoBehaviour
{
    [SerializeField] private Button spawnBtn;

    //Spawn Pos
    [SerializeField] private Transform playerSpawnPos;
    [SerializeField] private Transform[] enemySpawnPos;

    // Start is called before the first frame update
    void Start()
    {
        spawnBtn.onClick.AddListener(() =>
        {
            AddressableManager.Instance.CreateAsset<GameObject>(AddressableKey.ZOMBIE, result =>
            {
                var enemy = ObjectPool.Instance.GetObject(result);
                enemy.SetActive(true);
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector3 pos = Vector3.zero;
            if (enemySpawnPos.Length > 0)
                pos = enemySpawnPos[Random.Range(0, enemySpawnPos.Length)].position;
            AddEnemy(AddressableKey.ZOMBIE, pos, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            AddPlayer(AddressableKey.LOCAL_PLAYER, playerSpawnPos.position, Quaternion.identity);
        }
    }

    private void AddPlayer(string key, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        AddressableManager.Instance.CreateAsset<GameObject>(key, result =>
        {
            var player = Instantiate(result, position, rotation, parent);
            player.SetActive(true);

            GameEvent.OnPlayerSpawn.Invoke(player);
        });
    }

    private void AddEnemy(string key, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        AddressableManager.Instance.CreateAsset<GameObject>(AddressableKey.ZOMBIE, result =>
        {
            var enemy = ObjectPool.Instance.GetObject(result);
            enemy.SetActive(true);
            enemy.transform.SetPositionAndRotation(position, rotation);
            enemy.transform.SetParent(parent);

            var enemyBase = enemy.GetComponent<EnemyBase>();
        });
    }
}
