using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button joinBtn;

    [SerializeField] private Button playLocalBtn;
    [SerializeField] private Button playNetworkBtn;


    [SerializeField] private GameObject levelPanel;

    private void Awake()
    {
        joinBtn.onClick.AddListener(OnJointClicked);

        playLocalBtn.onClick.AddListener(PlayLocalBtnHandle);
    }

    private void OnDestroy()
    {
        joinBtn.onClick.RemoveAllListeners();

        playLocalBtn.onClick.RemoveAllListeners();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerNickname"))
            inputField.text = PlayerPrefs.GetString("PlayerNickname");
    }

    public void OnJointClicked()
    {
        PlayerPrefs.SetString("PlayerNickname", inputField.text);
        PlayerPrefs.Save();

        SceneManager.LoadSceneAsync("MainScene");
    }

    private void PlayLocalBtnHandle()
    {
        levelPanel.SetActive(true);
    }
}
