using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFlash : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float _speed = 1f;

    private float timer;
    private Color _color;
    // Update is called once per frame

    private void Start()
    {
        _color = text.color;
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (text == null && _speed > 0)
        {
            timer += Time.deltaTime * _speed;
            _color.a = Mathf.Sin(timer);
            text.color = _color;
        }

    }
}
