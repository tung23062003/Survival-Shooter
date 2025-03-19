using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using DG.Tweening;

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField inputField;

    [SerializeField] private Button playLocalBtn;
    [SerializeField] private Button playNetworkBtn;
    [SerializeField] private Button tutorialBtn;


    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject levelContainer;
    [SerializeField] private Transform levelBtnParent;

    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject tutorialContainer;

    [SerializeField] private Transform[] buttons;

    private List<Transform> levelPrefabsTrans = new();

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
        //AdsManager.Instance.LoadBannerAd();

        Sequence sequence = DOTween.Sequence();
        foreach (var button in buttons)
        {
            button.localScale = Vector3.zero;
            sequence.Append(button.DOScale(1, 0.25f));
        }
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
        levelContainer.transform.localScale = Vector2.zero;

        Sequence sequence = DOTween.Sequence();
        foreach (var levelTrans in levelPrefabsTrans)
        {
            levelTrans.localScale = Vector2.zero;
        }
        sequence.Append(levelContainer.transform.DOScale(1, 0.5f));
        foreach (var levelTrans in levelPrefabsTrans)
        {
            sequence.Append(levelTrans.DOScale(1, 0.2f));
        }
    }

    private void TutorialBtnHandle()
    {
        tutorialPanel.SetActive(true);
        tutorialContainer.transform.localScale = Vector2.zero;
        tutorialContainer.transform.DOScale(1, 0.5f);
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

            if(prefab != null)
                levelPrefabsTrans.Add(prefab.transform);
        });
        await Task.Yield();
    }

    private void LevelButtonHandle(LevelInfo levelInfo)
    {
        GameManager.Instance.levelLoading = levelInfo;
        LoadingManager.Instance.LoadScene(GameConstants.MainScene2);
    }
}
