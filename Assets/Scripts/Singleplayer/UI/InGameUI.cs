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

    private void Awake()
    {
        GameEvent.OnSpawning.AddListener(OnLoading);
        GameEvent.OnSpawnDone.AddListener(OnLoadDone);
        GameEvent.OnCountdown.AddListener(OnCountDown);
    }

    private void OnDestroy()
    {
        GameEvent.OnSpawning.RemoveAllListeners();
        GameEvent.OnSpawnDone.RemoveAllListeners();
        GameEvent.OnCountdown.RemoveAllListeners();
    }

    private void OnLoading()
    {
        loadingUI.SetActive(true);
    }

    private void OnLoadDone(LevelInfo levelInfo)
    {
        StartCoroutine(HideAterTime(levelInfo, 1));
    }

    private IEnumerator HideAterTime(LevelInfo levelInfo, float time = 2.0f)
    {
        yield return new WaitForSeconds(time);

        loadingUI.SetActive(false);

        GameManager.Instance.StartCountdown(levelInfo);
    }

    private void OnCountDown(string text)
    {
        countdownText.text = text;
    }
}
