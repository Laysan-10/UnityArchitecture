
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(AudioSource))]//вмссте со скриптом прикрепляется этот компонент(позволяет воппроизводить звук)
public class ChangeSpriteScene : MonoBehaviour
{
    
    [SerializeField] Canvas[] _canvas;
    //[SerializeField] AudioClip _audioClip;//принимает звук
    private bool ForOneUseScene2;
    private bool ForOneUseScene3;//Будевая переменная для того чтобы сцена появлялась единожды
    private bool ForOneUseScene4;

    void Start()
    {
        
        ForOneUseScene2 = true;
        ForOneUseScene3 = true;
        ForOneUseScene4 = true;
        _canvas[1].enabled = false;
        _canvas[2].enabled = false;
        _canvas[3].enabled = false;
        AShowUI();//элемент скрыт при старте
        Invoke(nameof(ShowUI), 15);//по истечению д15 секунд показыватся элемент канваса1
        Invoke(nameof(AShowUI), 25);
    }
    
    void ShowUI()//метод для того чтобы показать UI элемент(СценаСМагом1)
    {
        
        _canvas[0].enabled = true;//первый канвас массива показан
        Debug.Log("OnSprite");
       
    }
    void AShowUI()//метод чтобы скрыть UI
    {
        _canvas[0].enabled = false;
        Debug.Log("OffSprite");
        _canvas[1].enabled = false;
    }



    private void OnTriggerEnter(Collider PlayerCollider)//колллайдер персонада
    {
        if(PlayerCollider.tag == "ShowCanvas2" && ForOneUseScene2)//принимает коллайдер на ком скрипт и смотрит на касание                                                                
        {                                                        //с тегом(где есть триггерный коллайдер)
            //GetComponent<AudioSource>().PlayOneShot(_audioClip);
            ForOneUseScene2 = false;
            _canvas[1].enabled = true;
            Invoke(nameof(AShowUI), 8);
        }
        if (PlayerCollider.tag == "ShowCanvas3" && ForOneUseScene3)//принимает коллайдер на ком скрипт и смотрит на касание                                                                
        {                                                        //с тегом(где есть триггерный коллайдер)
            ForOneUseScene3 = false;
            //GetComponent<AudioSource>().PlayOneShot(_audioClip);
            _canvas[2].enabled = true;
            Invoke(nameof(AShowUI), 8);
        }
        if (PlayerCollider.tag == "ShowCanvas4" && ForOneUseScene4)//принимает коллайдер на ком скрипт и смотрит на касание                                                                
        {                                                        //с тегом(где есть триггерный коллайдер)
            ForOneUseScene3 = false;
            //GetComponent<AudioSource>().PlayOneShot(_audioClip);//сразу принимает компонент и проигрываем его
            _canvas[3].enabled = true;
            Invoke(nameof(AShowUI), 8);
        }

    }
    void Update()
    {
        
    }
}
