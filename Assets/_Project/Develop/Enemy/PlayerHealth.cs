using UnityEngine;

public class PlayerHealth : Heath
{
    public PlayerHealth(int max) : base(max) { } //перадаются значения конструктору
    public void Heal(int count)
    {
    // этот метод вызывается с другог скрипта, который активирует способность по кнопке
        _current = _current + count;
    }
    void Update()
    {
        if(isDead() == true)
        {
            //вызов анимации смерти и UI перезапуска игры
        }
    }
    void SlideBar()
    {
        //количество здоровья соразмерно уменьшается
    }
    
    void LowHealth()
    {
        //последствия малого количесва здоровья(понижается сокрость бега и высота прыжка)
    }

}
