using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation_health : MonoBehaviour
{
    private Transform _startTransform;

    // Start is called before the first frame update
    void Awake()
    {
        _startTransform = transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = _startTransform.position;
        transform.rotation = _startTransform.rotation;
    }
}
