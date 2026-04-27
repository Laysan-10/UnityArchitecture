using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;

    [Header("Buttons")]
    public Button saveButton;
    public Button loadButton;
    public Button mainMenuButton;

    [Header("Settings")]
    public Toggle peacefulModeToggle;

    public void Show(bool isActive)
    {
        pausePanel.SetActive(isActive);
    }

    public void SetPeacefulMode(bool isEnabled)
    {
        if (peacefulModeToggle != null)
        {
            peacefulModeToggle.SetIsOnWithoutNotify(isEnabled);
        }
    }
}
