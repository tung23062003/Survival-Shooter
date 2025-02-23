using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : Singleton<LoadingManager>
{
    //[SerializeField] private Button joinBtn;

    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private GameObject loadingPanel;

    protected override void Awake()
    {
        base.Awake();
        //if (joinBtn == null)
        //    return;
        //joinBtn.onClick.AddListener(() =>
        //{
        //    LoadScene(GameConstants.MainScene2);
        //    //AddressableManager.Instance.CreateScene(AddressableKey.MAIN_SCENE_2);
        //});
    }
    private void OnDestroy()
    {
        //if (joinBtn == null)
        //    return;
        //joinBtn.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            OpenApp();
    }

    private void OpenApp()
    {
        LoadScene(GameConstants.MenuScene);
    }

    public void LoadScene(string sceneName, Action onComplete = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, onComplete));
    }

    private IEnumerator LoadSceneAsync(string sceneName, Action onComplete = null)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            //SceneManager.LoadSceneAsync(GameConstants.LoadingScene);
            //yield return null;
            loadingPanel.SetActive(true);
        }


        float fakeProgress = 0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (fakeProgress < 1f)
        {
            fakeProgress += Time.deltaTime * 0.3f;
            progressBar.value = Mathf.Clamp01(fakeProgress);
            progressText.text = (fakeProgress * 100).ToString("F0") + "%";

            if (operation.progress >= 0.9f && fakeProgress >= 1f)
            {
                operation.allowSceneActivation = true;
                onComplete?.Invoke();
            }

            yield return null;
        }
    }
}
