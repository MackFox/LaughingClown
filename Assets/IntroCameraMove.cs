using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCameraMove : MonoBehaviour
{
    [SerializeField] Vector3 _startPosition;
    [SerializeField] Vector3 _endPosition;
    [SerializeField] float _speed;

    private float timer;

    void Update()
    {
        transform.position = Vector3.Lerp(_startPosition, _endPosition, timer);
        timer += Time.deltaTime * _speed;
        while (timer > 1) timer -= 1.0f;
    }
}
