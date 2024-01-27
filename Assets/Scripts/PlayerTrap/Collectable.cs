using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerHand;
    [SerializeField] private Transform _item;
    [SerializeField] private Transform _chestTopPivot;
    [Header("Settings")]
    [SerializeField] private CollectableType _type;
    [SerializeField] private float _openingSpeed = 0.1f;
    [SerializeField] private Vector3 _targetOpenRotationPos;

    private OpeningState _state;
    private float timer;
    private Vector3 _defaultRotation;

    private enum OpeningState
    {
        Closed = 0,
        Opening = 1,
        Open = 2,
    }

    private void Start()
    {
        _defaultRotation = _chestTopPivot.eulerAngles;
    }

    private void Update()
    {
        if (_state == OpeningState.Opening)
        {
            // Play Opening Animation
            timer += Time.deltaTime * _openingSpeed;
            _chestTopPivot.eulerAngles = Vector3.Lerp(_defaultRotation, _targetOpenRotationPos, timer);

            if (timer >= 1f)
            {
                _state = OpeningState.Open;
                Player.GetInstance().SetItemInPlayerHand(_item.gameObject, _type);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _state < OpeningState.Opening && Player.GetInstance().CurrentCollectable == CollectableType.None)
        {
            _state = OpeningState.Opening;
        }
    }
}
