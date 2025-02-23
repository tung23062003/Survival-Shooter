using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void Start()
    {
        GameManager.Instance.SpawnLevel(isSpawnNextLevel: false, isRestartLevel: false);
    }

    private void OnDestroy()
    {
        
    }
}
