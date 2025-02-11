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

    private void Awake()
    {
        joinBtn.onClick.AddListener(OnJointClicked);
    }

    private void OnDestroy()
    {
        joinBtn.onClick.RemoveAllListeners();
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
}
