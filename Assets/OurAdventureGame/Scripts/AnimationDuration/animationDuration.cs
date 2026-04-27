using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationDuration : MonoBehaviour
{
    public Animator animator; // Укажите ваш компонент аниматора здесь
    public string animationClipName; // Имя анимации в аниматоре

    void Start()
    {
        float duration = 0f;

        // Получаем контроллер анимаций из компонента аниматора
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        // Проходим по всем анимациям в контроллере и ищем нужную по имени
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == animationClipName)
            {
                duration = ac.animationClips[i].length;
                break;
            }
        }

        Debug.Log("Длительность анимации '" + animationClipName + "': " + duration + " секунд");
    }
}
