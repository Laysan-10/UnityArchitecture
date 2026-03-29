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

    public void Show(bool isActive)
    {
        pausePanel.SetActive(isActive);
    }
}