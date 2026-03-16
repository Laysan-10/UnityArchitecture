using UnityEngine;


public class DeathByClouds : MonoBehaviour
{
    private int pers;
    [SerializeField] private Player _player;
    [SerializeField] private Player _player_L;

    private void Start()
    {
        pers = PlayerPrefs.GetInt("CharacterSelected"); // получаем значени по ключи из PlayerPrefs

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Low"))
        {
            _player.TakeDamage(200);
        }
            
        
    }
}
