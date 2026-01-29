using TMPro;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button startGameButton;
    public Button settingsButton;
    public GameObject SettingsMenu;

    public Toggle returnToMenuAfterDeath;
    public Toggle useNNplayer;

    public TextMeshProUGUI highScoreText;

    void Start()
    {
        startGameButton.onClick.AddListener(OnStartGameClick);
        settingsButton.onClick.AddListener(OnSettingsClick);
        returnToMenuAfterDeath.onValueChanged.AddListener(OnReturnToMenuAfterDeathTogglehanged);
        useNNplayer.onValueChanged.AddListener(UseNNPlayerTogglehanged);

        GameSattings.returnToMenuAfterDeath = returnToMenuAfterDeath.isOn;

        
    }

    public void OnStartGameClick()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnSettingsClick()
    {
        SettingsMenu.SetActive(!SettingsMenu.activeSelf);
    }

    public void OnReturnToMenuAfterDeathTogglehanged(bool state)
    {
        GameSattings.returnToMenuAfterDeath = state;
    }
    public void UseNNPlayerTogglehanged(bool state)
    {
        GameSattings.useNNPlayer = state;
    }
}
