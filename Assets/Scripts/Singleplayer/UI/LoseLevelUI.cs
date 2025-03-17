using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LoseLevelUI : MonoBehaviour
{
    [SerializeField] private Button replayBtn;
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button menuBtn;

    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject loseContainer;

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
        losePanel.GetComponent<CanvasGroup>().DOFade(1, 0.5f).OnComplete(() => {
            loseContainer.GetComponent<CanvasGroup>().alpha = 0;
            loseContainer.GetComponent<RectTransform>().transform.localPosition = new(0, -1000f, 0);
            loseContainer.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 82f), 1.0f, false).SetEase(Ease.OutElastic);
            loseContainer.GetComponent<CanvasGroup>().DOFade(1, 1.0f);

            //Sequence sequence = DOTween.Sequence();

            //foreach (Transform star in stars)
            //{
            //    sequence.AppendInterval(0.25f);
            //    star.localScale = Vector3.zero;
            //    sequence.Append(star.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBack));
            //}

            //sequence.OnComplete(() => StartCoroutine(WaitForShowAds(1.25f)));

        });
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
            losePanel.GetComponent<CanvasGroup>().alpha = 0;
            loseContainer.GetComponent<CanvasGroup>().alpha = 0;
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
        losePanel.GetComponent<CanvasGroup>().alpha = 0;
        loseContainer.GetComponent<CanvasGroup>().alpha = 0;
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
