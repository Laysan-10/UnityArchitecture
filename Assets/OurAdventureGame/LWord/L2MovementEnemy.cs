using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent ( typeof(NavMeshAgent))] // add component in defalt
public class L2MovementEnemy : MonoBehaviour
{
    [SerializeField] private Transform _path; // массив точек с их расположением
    private Transform[] _points;
    private int _currentPoints; //кол-во элементов в массиве(для рандомного перемешения)
    [SerializeField] float _speed;
    [SerializeField] float _speedrotation;
   
    [SerializeField] Transform Player;
    [SerializeField] AudioClip myClip;
    [SerializeField] int Damage = 10;
    [SerializeField] Text indicator2;
    public int HeathEnemy = 100;


    void Start()
    {
        _points = new Transform[_path.childCount]; //создаем массив по длине равному кол-ву дочерних элементов
        for(int i = 0; i < _path.childCount; i++) {
            _points[i] = _path.GetChild (i);// в каждый элемент массива добав дочерний объект
        }
    }
    
    // Update is called once per frame
    void Update()
    {
      
        if (Vector3.Distance(Player.transform.position, transform.position) >= 30) //вектор перевижение по x,y,z и дистансе передеает расстояние
        {
            
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Animator>().SetBool("Walk", true);
            GetComponent<Animator>().SetBool("Run", false);
            GetComponent<Animator>().SetBool("Attack", false);
            Transform target = _points[_currentPoints];
            
            Vector3 direction = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _speedrotation);

            transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime); //новая позиция это конец пути от начальной до точки
            if(transform.position == target.position)
            {
                _currentPoints = Random.Range(0, _points.Length);
            }
            
            
        
        }
        if (Vector3.Distance(Player.transform.position, transform.position) < 30) //вектор перевижение по x,y,z и дистансе передеает расстояние
        {
            GetComponent<Animator>().SetBool("Walk", false);
            GetComponent<Animator>().SetBool("Run", true);
            GetComponent<NavMeshAgent>().enabled = true; 
            GetComponent<NavMeshAgent>().destination = Player.transform.position; //destination Пункт названачения(не работает без bake)
        }
        if (Vector3.Distance (Player.transform.position, transform.position) <= 4f) //when enemy close, you stop him
        {
            GetComponent<Animator>().SetBool("Run", false);
            GetComponent<Animator>().SetBool("Walk", false);
            GetComponent<NavMeshAgent>().enabled = false; //off component
            
        }
        if (Vector3.Distance (Player.transform.position, transform.position) <= 4f) 
        {
            
            GetComponent<Animator>().SetBool("Attack", true);
            GetComponent<Animator>().SetBool("Run", false);
            GetComponent<Animator>().SetBool("Walk", false);

        }
       
        Debug.Log(HeathEnemy);
        if(HeathEnemy <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {

            GetComponent<AudioSource>().PlayOneShot(myClip);
            Player.GetComponent<LLevelHeath>().levelHeath -= Damage; // обратимся к игроку и возьмем компонент LLevelHeath
            Debug.Log("Attack by Enemy");

        }
      
    }
}
