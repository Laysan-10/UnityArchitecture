using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (PauseGame)
            {
                Resume();
            }
            else
            {
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
    }
    
    public void Pause()
    {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f; // если выйдем, а потом зайдём будем иметь меню на паузе(нам это не нужно)
        SceneManager.LoadScene("MainMenu");

    }
}
