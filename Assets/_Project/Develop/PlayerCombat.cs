using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private IInputService _inputService;

    // События, на которые подпишется Аниматор
    public event Action OnAttackPhysFired;
    public event Action OnAttackMagFired;

    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
        
        // Подписываемся на команды ввода
        _inputService.OnPhysicalAttack += HandlePhysicalAttack;
        _inputService.OnMagicAttack += HandleMagicAttack;
    }

    private void HandlePhysicalAttack()
    {
        // Здесь позже сделаем логику поиска врагов в радиусе и нанесение урона
        Debug.Log("Логика: Выполнена физическая атака");
        OnAttackPhysFired?.Invoke(); // Говорим аниматору проиграть анимацию
    }

    private void HandleMagicAttack()
    {
        // Здесь позже сделаю логику спавна фаербола
        Debug.Log("Логика: Выполнена магическая атака");
        OnAttackMagFired?.Invoke();
    }

    private void OnDestroy()
    {
        // Отписываемся от событий, чтобы избежать утечек памяти
        if (_inputService != null)
        {
            _inputService.OnPhysicalAttack -= HandlePhysicalAttack;
            _inputService.OnMagicAttack -= HandleMagicAttack;
        }
    }
}