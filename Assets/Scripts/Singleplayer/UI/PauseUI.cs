using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button menuBtn;

    [SerializeField] private GameObject pausePanel;
    private void Awake()
    {
        continueBtn.onClick.AddListener(ContinueBtnHandle);
        restartBtn.onClick.AddListener(RestartBtnHandle);
        menuBtn.onClick.AddListener(MenuBtnHandle);
    }

    private void OnDestroy()
    {
        continueBtn.onClick.RemoveAllListeners();
        restartBtn.onClick.RemoveAllListeners();
        menuBtn.onClick.RemoveAllListeners();

    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        pausePanel.SetActive(!pausePanel.activeSelf);

        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = !Cursor.visible;
    }
#endif
    private void ContinueBtnHandle()
    {
        
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    private void RestartBtnHandle()
    {
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
        Time.timeScale = 1;
        pausePanel.SetActive(false);
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
