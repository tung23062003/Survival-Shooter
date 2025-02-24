using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLevelUI : MonoBehaviour
{
    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private Button replayBtn;
    [SerializeField] private Button menuBtn;

    [SerializeField] private GameObject winPanel;

    private void Awake()
    {
        nextLevelBtn.onClick.AddListener(HandleNextLevel);
        replayBtn.onClick.AddListener(RestartBtnHandle);
        menuBtn.onClick.AddListener(MenuBtnHandle);

        GameEvent.OnWinLevel.AddListener(OnWinLevel);
    }

    private void OnDestroy()
    {
        nextLevelBtn.onClick.RemoveAllListeners();
        replayBtn.onClick.RemoveAllListeners();
        menuBtn.onClick.RemoveAllListeners();
        GameEvent.OnWinLevel.RemoveAllListeners();
    }

    private void OnWinLevel()
    {
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#endif
        if (GameManager.Instance.IsLevelMax())
        {
            //nextLevelBtn.interactable = false;
            nextLevelBtn.gameObject.SetActive(false);
        }
        StartCoroutine(ShowWinPanelAfterTime(1.5f));
    }

    private IEnumerator ShowWinPanelAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        winPanel.SetActive(true);
    }

    private void HandleNextLevel()
    {
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
        winPanel.SetActive(false);
        GameManager.Instance.SpawnLevel(isSpawnNextLevel: true, isRestartLevel: false);
    }

    private void RestartBtnHandle()
    {
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
        Time.timeScale = 1;
        winPanel.SetActive(false);
        GameManager.Instance.SpawnLevel(isSpawnNextLevel: false, isRestartLevel: true);
    }

    private void MenuBtnHandle()
    {
        Time.timeScale = 1;
        GameManager.Instance.DestroySingleton();
        AddressableManager.Instance.DestroySingleton();
        LoadingManager.Instance.LoadScene(GameConstants.MenuScene);
    }
}
