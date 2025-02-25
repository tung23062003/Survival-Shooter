using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpButtonHandle : MonoBehaviour
{
    [SerializeField] private Button jumpBtn;
    private PlayerController playerController;

    private void Awake()
    {
        GameEvent.OnPlayerSpawn.AddListener(OnPlayerSpawn);
#if UNITY_ANDROID || UNITY_IOS
        jumpBtn.onClick.AddListener(() =>
        {
            playerController.Jump();
        });
#endif
    }

    private void OnDestroy()
    {
        GameEvent.OnPlayerSpawn.RemoveAllListeners();
    }

    private void OnPlayerSpawn(GameObject playerPrefab)
    {
        if (playerPrefab != null)
            playerController = playerPrefab.GetComponent<PlayerController>();
    }
}
