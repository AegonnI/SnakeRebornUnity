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
    
    void Start()
    {
        startGameButton.onClick.AddListener(OnStartGameClick);
        settingsButton.onClick.AddListener(OnSettingsClick);
        returnToMenuAfterDeath.onValueChanged.AddListener(OnReturnToMenuAfterDeathTogglehanged);

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
}
