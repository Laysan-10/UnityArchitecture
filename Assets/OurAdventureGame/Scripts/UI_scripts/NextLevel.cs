using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private ParticleSystem _portal;
    [SerializeField] private GameObject _cavnas;
    [SerializeField] private GameObject _eventSystemUI;

    private void Start()
    {
        DontDestroyOnLoad(_cavnas);
        DontDestroyOnLoad(_eventSystemUI);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("portal"))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
