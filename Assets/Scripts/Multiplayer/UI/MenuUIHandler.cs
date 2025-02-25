using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField inputField;

    [SerializeField] private Button playLocalBtn;
    [SerializeField] private Button playNetworkBtn;
    [SerializeField] private Button tutorialBtn;


    [SerializeField] private GameObject levelPanel;
    [SerializeField] private Transform levelBtnParent;

    [SerializeField] private GameObject tutorialPanel;

    private void Awake()
    {
        playNetworkBtn.onClick.AddListener(PlayNetworkHandle);
        playLocalBtn.onClick.AddListener(PlayLocalBtnHandle);
        tutorialBtn.onClick.AddListener(TutorialBtnHandle);

        GameEvent.OnLoadDataDone.AddListener(OnLoadDataDone);
    }

    private void OnDestroy()
    {
        playNetworkBtn.onClick.RemoveAllListeners();
        playLocalBtn.onClick.RemoveAllListeners();
        tutorialBtn.onClick.RemoveAllListeners();

        GameEvent.OnLoadDataDone.RemoveAllListeners();
    }

    private void OnEnable()
    {
        AdsManager.Instance.LoadBannerAd();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        if (PlayerPrefs.HasKey("PlayerNickname"))
            inputField.text = PlayerPrefs.GetString("PlayerNickname");
    }

    public void PlayNetworkHandle()
    {
        PlayerPrefs.SetString("PlayerNickname", inputField.text);
        PlayerPrefs.Save();

        SceneManager.LoadSceneAsync("MainScene");
    }

    private void PlayLocalBtnHandle()
    {
        levelPanel.SetActive(true);
    }

    private void TutorialBtnHandle()
    {
        tutorialPanel.SetActive(true);
    }

    private async void OnLoadDataDone(LevelDataSO levelData)
    {
        for (int i = 0; i < levelData.levels.Count; i++)
        {
            await StartLoadData(levelData, i);
        }

    }

    private async Task StartLoadData(LevelDataSO levelData, int index)
    {
        AddressableManager.Instance.CreateAsset<GameObject>(AddressableKey.LEVEL_BTN, result => {
            var prefab = Instantiate(result, levelBtnParent);
            int level = levelData.levels[index].level;
            prefab.GetComponentInChildren<TextMeshProUGUI>().text = level.ToString();
            prefab.GetComponent<Button>().onClick.AddListener(() => LevelButtonHandle(levelData.levels[index]));
        });
        await Task.Yield();
    }

    private void LevelButtonHandle(LevelInfo levelInfo)
    {
        GameManager.Instance.levelLoading = levelInfo;
        LoadingManager.Instance.LoadScene(GameConstants.MainScene2);
    }
}
