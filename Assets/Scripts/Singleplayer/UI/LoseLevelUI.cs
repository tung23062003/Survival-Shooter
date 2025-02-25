using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseLevelUI : MonoBehaviour
{
    [SerializeField] private Button replayBtn;
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button menuBtn;

    [SerializeField] private GameObject losePanel;

    private void Awake()
    {
        continueBtn.onClick.AddListener(HandleContinueLevel);
        replayBtn.onClick.AddListener(RestartBtnHandle);
        menuBtn.onClick.AddListener(MenuBtnHandle);

        GameEvent.OnLoseLevel.AddListener(OnLoseLevel);
    }

    private void OnDestroy()
    {
        continueBtn.onClick.RemoveAllListeners();
        replayBtn.onClick.RemoveAllListeners();
        menuBtn.onClick.RemoveAllListeners();
        GameEvent.OnLoseLevel.RemoveAllListeners();
    }

    private void OnLoseLevel()
    {
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#endif
        
        StartCoroutine(ShowLosePanelAfterTime(1.5f));
    }

    private IEnumerator ShowLosePanelAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        losePanel.SetActive(true);
    }

    private void HandleContinueLevel()
    {
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#endif
        //Show Ads
        AdsManager.Instance.ShowRewardedAd(() =>
        {
            losePanel.SetActive(false);
            GameManager.Instance.RevivePlayer();
            Time.timeScale = 0;
        });
    }

    private void RestartBtnHandle()
    {
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
        Time.timeScale = 1;
        losePanel.SetActive(false);
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
