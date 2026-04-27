using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    public Button newGameButton;
    public Button continueButton;
    public Button settingsButton;
    public Button quitButton;

    [Header("Settings Panel")]
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public Button closeSettingsButton;

    public void ShowSettings(bool isActive)
    {
        settingsPanel.SetActive(isActive);
    }

    public void SetContinueInteractable(bool isInteractable)
    {
        if (continueButton != null)
        {
            continueButton.interactable = isInteractable;
        }
    }
}
