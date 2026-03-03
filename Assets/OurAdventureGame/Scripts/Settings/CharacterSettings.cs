using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSettings : MonoBehaviour
{
    int selestch;
    [Header("Characters")]
    [SerializeField] private GameObject charahter1;
    [SerializeField] private GameObject charahter2;
    public Transform transform_ch1;
    public Transform transform_ch2;

    [Header("SkillActivate")]
    [SerializeField] private GameObject _shield_skill;
    [SerializeField] private GameObject _attack_skill;

    [Header("TrackingCamera")]
    [SerializeField] private GameObject _virtual_camera;
    private CinemachineVirtualCamera _virtualCameraComponent;


    [Header("Players")]
    [SerializeField] private Player _playerCharahter1;
    [SerializeField] private Player _playerCharahter2;
    enemyAI[] _goblinsSrangAI;
    Hammer_attack[] _goblinsSrangARang;
    Attack_goblin_C_Rang[] _goblinsSrangCRang;


    [Header("HitPositions(magic ball)")]
    [SerializeField] private Transform _hitPositionCharacter1;
    [SerializeField] private Transform _hitPositionCharacter2;
    dark_magic_ball[] _goblinsArang;
    public dark_magic_ball _dark_magic_ballByPlayer1;
    public dark_magic_ball _dark_magic_ballByPlayer2;

    [Header("Hints")]
    [SerializeField] private GameObject _hints;
    [SerializeField] private GameObject _hints_L;



    private void Start()
    {
        Debug.Log(selestch);
        selestch = PlayerPrefs.GetInt("CharacterSelected"); // PlayerPrefs
        _virtualCameraComponent = _virtual_camera.GetComponent<CinemachineVirtualCamera>();

        _goblinsSrangAI = FindObjectsOfType<enemyAI>();
        _goblinsSrangARang = FindObjectsOfType<Hammer_attack>();
        _goblinsArang = FindObjectsOfType<dark_magic_ball>();
        _goblinsSrangCRang = FindObjectsOfType<Attack_goblin_C_Rang>();

        Disable();
    }

    private void Disable()
    {
        if(selestch == 1)
        {
            _shield_skill.SetActive(false);
            _attack_skill.SetActive(false);
            _virtualCameraComponent.Follow = charahter2.transform;
            _hints_L.SetActive(true);

            foreach (var gob in _goblinsSrangAI)
            {
                gob.target = transform_ch2;
            }

            foreach (var gobAttack in _goblinsSrangARang)
            {
                gobAttack._player = _playerCharahter2;
            }

            foreach (var gobAttack in _goblinsSrangCRang)
            {
                gobAttack._player = _playerCharahter2;
            }

            foreach (var magicGob in _goblinsArang)
            {

                magicGob._player = _playerCharahter2;
                magicGob.hitPosition = _hitPositionCharacter2;
            }

            //_dark_magic_ballByPlayer2.enabled = false;
        }

        if (selestch == 0)
        {
            _shield_skill.SetActive(true);
            _attack_skill.SetActive(true);
            _virtualCameraComponent.Follow = charahter1.transform;
            _hints.SetActive(true);

            foreach (var gob in _goblinsSrangAI)
            {
                gob.target = transform_ch1;
            }

            foreach (var gobAttack in _goblinsSrangARang)
            {
                gobAttack._player = _playerCharahter1;
            }

            foreach (var gobAttack in _goblinsSrangCRang)
            {
                gobAttack._player = _playerCharahter1;
            }

            foreach (var magicGob in _goblinsArang)
            {
                if (magicGob.CompareTag("Player") == false)
                {
                    magicGob._player = _playerCharahter1;
                    magicGob.hitPosition = _hitPositionCharacter1;
                }
            }

            //_dark_magic_ballByPlayer1.enabled = false;
        }
    }
}
