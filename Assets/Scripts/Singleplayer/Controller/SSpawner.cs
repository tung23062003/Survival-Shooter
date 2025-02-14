using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSpawner : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddressableManager.Instance.CreateAsset<GameObject>(AddressableKey.ZOMBIE, result =>
            {
                var enemy = ObjectPool.Instance.GetObject(result);
                enemy.SetActive(true);
            });
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            AddressableManager.Instance.CreateAsset<GameObject>(AddressableKey.LOCAL_PLAYER, result =>
            {
                var player = ObjectPool.Instance.GetObject(result);
                player.SetActive(true);
            });
        }
    }
}
