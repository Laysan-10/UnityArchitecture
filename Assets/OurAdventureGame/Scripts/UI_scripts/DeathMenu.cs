using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    //[SerializeField] private GameObject _exit;
    //[SerializeField] private GameObject _restart;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private Player _player;
    [SerializeField] private Player _player_L;

    private int pers;

    //private void Start()
    //{
    //    pers = PlayerPrefs.GetInt("CharacterSelected"); // получаем значени по ключи из PlayerPrefs
    //}

    //private void Update()
    //{
    //    if (pers == 0)
    //    {
    //        if (_player.Health <= 0)
    //        {
    //            deathMenu.SetActive(true);
    //            Time.timeScale = 0f;
    //        }
    //    }

    //    if (pers == 1)
    //    {
    //        if (_player_L.Health <= 0)
    //        {
    //            deathMenu.SetActive(true);
    //            Time.timeScale = 0f;
    //        }
    //    }

    //}

    public void SetActivDeathMenu()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        SceneManager.LoadScene("World-game");
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }
}
