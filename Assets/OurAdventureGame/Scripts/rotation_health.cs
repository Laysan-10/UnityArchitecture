using UnityEngine;

public class rotation_health : MonoBehaviour
{
    private Camera _mainCamera;

    private void Start()
    {
        // Находим главную камеру на сцене
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_mainCamera != null)
        {
            // Заставляем Canvas смотреть ровно в ту же сторону, куда смотрит камера
            transform.LookAt(transform.position + _mainCamera.transform.forward);
        }
    }
}