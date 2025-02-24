using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private TextMeshProUGUI countdownText;

    [SerializeField] private Button pauseBtn;
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private RawImage damageScreen;

    private void Awake()
    {
        GameEvent.OnSpawning.AddListener(OnLoading);
        GameEvent.OnSpawnDone.AddListener(OnLoadDone);
        GameEvent.OnCountdown.AddListener(OnCountDown);
        GameEvent.OnKillEnemy.AddListener(UpdateObjectiveText);
        GameEvent.OnTakeDamage.AddListener(ShowDamageScreen);

        pauseBtn.onClick.AddListener(PauseBtnHandle);
    }

    private void OnDestroy()
    {
        GameEvent.OnSpawning.RemoveAllListeners();
        GameEvent.OnSpawnDone.RemoveAllListeners();
        GameEvent.OnCountdown.RemoveAllListeners();

        pauseBtn.onClick.RemoveAllListeners();
    }

    private void OnLoading()
    {
        loadingUI.SetActive(true);
    }

    private void OnLoadDone(LevelInfo levelInfo)
    {
        StartCoroutine(HideLoadingScreenAfterTime(levelInfo, 1));
    }

    private IEnumerator HideLoadingScreenAfterTime(LevelInfo levelInfo, float time = 2.0f)
    {
        UpdateObjectiveText();
        yield return new WaitForSeconds(time);

        loadingUI.SetActive(false);

        GameManager.Instance.StartCountdown(levelInfo);
    }

    private void UpdateObjectiveText()
    {
        objectiveText.text = $"<color=red>KILLED:</color> {GameManager.Instance.enemyKillQuantity}/{GameManager.Instance.levelLoading.enemyQuantity}";
    }

    private void OnCountDown(string text)
    {
        countdownText.text = text;
    }

    private void PauseBtnHandle()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausePanel.SetActive(true);
    }

    private void ShowDamageScreen(EntityInfo entityInfo, Vector3 pos)
    {
        if(entityInfo.entityType == EntityType.Player)
        {
            damageScreen.enabled = true;
            StartCoroutine(HideDamageScreenAfterTime(0.25f));
        }
    }

    private IEnumerator HideDamageScreenAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        damageScreen.enabled = false;
    }
}
