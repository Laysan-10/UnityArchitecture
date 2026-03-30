using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LLevelHeath : MonoBehaviour
{
    [SerializeField] Text indicator;
    private bool flag;
    public int levelHeath = 100;
    


    // Update is called once per frame
    void Update()
    {
        if (flag)
        {
            if(levelHeath >= 100) { 
            levelHeath = 100;}
            if(levelHeath <= 0) {
                Debug.Log("Game Over");
                flag = false;
            }
        }
        
        indicator.text = "" + levelHeath;
        
    }
   
}
