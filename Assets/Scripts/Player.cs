using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;

    [SerializeField] private Transform _playerHand;

    private GameObject _currentItem;
    public CollectableType CurrentCollectable { get; private set; }
    public Transform PlayerHand => _playerHand;



    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        
    }

    private void SetCollectableToPlayer(CollectableType newCollectable)
    {
        CurrentCollectable = newCollectable;
    }

    public void SetItemInPlayerHand(GameObject item, CollectableType itemType)
    {
        _currentItem = item;
        _currentItem.transform.position = _playerHand.position;
        _currentItem.transform.rotation = Quaternion.identity;
        _currentItem.transform.SetParent(_playerHand);
        GameManager.GetInstance().SetCollectable(itemType);
        SetCollectableToPlayer(itemType);
    }

    public void RemoveItemFromPlayerHand()
    {
        Destroy(_currentItem);
    }

    public static Player GetInstance()
    {
        return instance;
    }
}
