using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button startGameButton;
    public Button settingsButton;
    public GameObject SettingsMenu;
    public Button exitButton;

    public Toggle returnToMenuAfterDeath;
    public Toggle useNNplayer;

    public TextMeshProUGUI highScoreText;

    void Start()
    {
        startGameButton.onClick.AddListener(OnStartGameClick);
        settingsButton.onClick.AddListener(OnSettingsClick);
        exitButton.onClick.AddListener(OnExitClick);
        returnToMenuAfterDeath.onValueChanged.AddListener(OnReturnToMenuAfterDeathTogglehanged);
        useNNplayer.onValueChanged.AddListener(UseNNPlayerTogglehanged);

        GameSattings.returnToMenuAfterDeath = returnToMenuAfterDeath.isOn;

        SettingsMenu.SetActive(false);
    }

    public void OnStartGameClick()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnSettingsClick()
    {
        SettingsMenu.SetActive(!SettingsMenu.activeSelf);
    }

    public static void OnExitClick()
    {
        // Для работы кнопки в редакторе Unity
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // Для работы в собранном билде на ПК и смартфонах
                Application.Quit();
        #endif
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
