using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLevelUI : MonoBehaviour
{
    [SerializeField] private Button nextLevelBtn;

    private void Awake()
    {
        nextLevelBtn.onClick.AddListener(HandleNextLevel);

        GameEvent.OnWinLevel.AddListener(OnWinLevel);
    }

    private void OnDestroy()
    {
        nextLevelBtn.onClick.RemoveAllListeners();
        GameEvent.OnWinLevel.RemoveAllListeners();
    }

    private void OnWinLevel()
    {
        gameObject.SetActive(true);
    }

    private void HandleNextLevel()
    {

    }
}
