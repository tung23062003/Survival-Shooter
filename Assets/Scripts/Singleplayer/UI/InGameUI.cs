using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject loadingUI;

    private void Awake()
    {
        GameEvent.OnLoading.AddListener(OnLoading);
        GameEvent.OnLoadDone.AddListener(OnLoadDone);
    }

    private void OnDestroy()
    {
        GameEvent.OnLoading.RemoveAllListeners();
        GameEvent.OnLoadDone.RemoveAllListeners();
    }

    private void OnLoading()
    {
        loadingUI.SetActive(true);
    }

    private void OnLoadDone()
    {
        StartCoroutine(HideAterTime(5));
    }

    private IEnumerator HideAterTime(float time = 2.0f)
    {
        yield return new WaitForSeconds(time);

        loadingUI.SetActive(false);
    }
}
