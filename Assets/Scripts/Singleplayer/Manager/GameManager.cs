using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public Transform player;

    protected override void Awake()
    {
        base.Awake();
        GameEvent.OnPlayerSpawn.AddListener(OnPlayerSpawn);
    }

    private void OnDestroy()
    {
        GameEvent.OnPlayerSpawn.RemoveAllListeners();
    }

    private void Start()
    {
        
    }

    private void OnPlayerSpawn(GameObject playerPrefab)
    {
        if(playerPrefab != null)
            player = playerPrefab.transform;
    }
}
