using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : Singleton<LoadingManager>
{
    [SerializeField] private Button joinBtn;
    protected override void Awake()
    {
        joinBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync(GameConstants.MainScene2, LoadSceneMode.Single);
            //AddressableManager.Instance.CreateScene(AddressableKey.MAIN_SCENE_2);
        });
    }
    private void Start()
    {
        //AddressableManager.Instance.StartUpdate((onComplete) => { });

        
    }
    private void OnDestroy()
    {
        joinBtn.onClick.RemoveAllListeners();
    }

    //[SerializeField] private Slider progressBar;
    //[SerializeField] private TextMeshProUGUI progressText;
    //[SerializeField] private GameObject loadingPanel;

    //private AsyncOperationHandle<SceneInstance> _sceneHandle;

    //private void Start()
    //{
    //    if (SceneManager.GetActiveScene().buildIndex == 0)
    //    {
    //        AddressableManager.Instance.StartUpdate((onComplete) => { OpenApp(); });
    //    }
    //}

    //private void OpenApp()
    //{
    //    LoadScene(GameConstants.MenuScene);
    //}

    //public void LoadScene(string sceneName)
    //{
    //    StartCoroutine(LoadSceneAsync(sceneName));
    //}

    //private IEnumerator LoadSceneAsync(string sceneName)
    //{
    //    if (SceneManager.GetActiveScene().buildIndex != 0)
    //    {
    //        loadingPanel.SetActive(true);
    //    }

    //    _sceneHandle = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single, false);
    //    float fakeProgress = 0f;

    //    while (!_sceneHandle.IsDone || fakeProgress < 1f)
    //    {
    //        float realProgress = _sceneHandle.PercentComplete;

    //        fakeProgress = Mathf.MoveTowards(fakeProgress, realProgress, Time.deltaTime * 0.3f);

    //        progressBar.value = Mathf.Clamp01(fakeProgress);
    //        progressText.text = (fakeProgress * 100).ToString("F0") + "%";

    //        if (_sceneHandle.Status == AsyncOperationStatus.Succeeded && fakeProgress >= 1f)
    //        {
    //            _sceneHandle.Result.ActivateAsync();
    //            break;
    //        }

    //        yield return null;
    //    }

    //    // ?n panel loading sau khi t?i xong
    //    loadingPanel.SetActive(false);
    //}

    //private void OnDestroy()
    //{
    //    if (_sceneHandle.IsValid())
    //    {
    //        Addressables.Release(_sceneHandle);
    //    }
    //}
}
