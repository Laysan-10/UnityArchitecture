using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterChange : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    private int CharacterIndex;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;

    private bool IsButton1 = false;
    private bool IsButton2 = false;



     private void Start()
    {
        CharacterIndex = PlayerPrefs.GetInt("CharacterSelected");

        foreach (GameObject ch in characters)
        {
            ch.SetActive(false);
        }

        if (characters[CharacterIndex])
        {
            characters[CharacterIndex].SetActive(true);
        }
    }


    // Метод для выбора персонажа и записи в PlayerPrefs
    private void SelectCharacter(int index)
    {
        foreach (GameObject ch in characters)
        {
                ch.SetActive(false);
        }

        characters[index].SetActive(true);

        PlayerPrefs.SetInt("CharacterSelected", index);
    }

    public void OnButtonClick1()
    {
        SelectCharacter(0);
        IsButton1 = true;
        
    }

    public void OnButtonClick2()
    {
        SelectCharacter(1);
        IsButton2 = true;
    }


    public void DefaultChange()
    {
        if(IsButton1 == false && IsButton2 == false)
        {
            SelectCharacter(0);

        }
        
    }


}
