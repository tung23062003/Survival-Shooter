using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        //transform.DOScale(0, 0.5f);
        gameObject.SetActive(false);
    }
}
