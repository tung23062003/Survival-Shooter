using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Button closeBtn;

    private void Awake()
    {
        closeBtn.onClick.AddListener(CloseBtnHandle);
    }

    private void OnDestroy()
    {
        closeBtn.onClick.RemoveAllListeners();
    }

    private void CloseBtnHandle()
    {
        gameObject.SetActive(false);
    }
}
