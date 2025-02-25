using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
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
