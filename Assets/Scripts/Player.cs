using StarterAssets;
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

        UnlockCursor(false);
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
        SetCollectableToPlayer(itemType);
    }

    public void RemoveItemFromPlayerHand()
    {
        CurrentCollectable = CollectableType.None;
        Destroy(_currentItem);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Death Behavior
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Clown reached player, your are dead!");
            ClownAgent.GetInstance().KillPlayer();
            GetComponent<FirstPersonController>().enabled = false;
            StartCoroutine(LoseScreen());
            //CanvasScript.instance.SetLoseScreen();
        }
    }

    private IEnumerator LoseScreen()
    {
        yield return new WaitForSeconds(1f);
        CanvasScript.instance.SetLoseScreen();
    }

    public void UnlockCursor(bool unlock)
    {
        GetComponent<StarterAssetsInputs>().cursorLocked = !unlock;
        GetComponent<StarterAssetsInputs>().cursorInputForLook = !unlock;
        GetComponent<StarterAssetsInputs>().SetCursorState(!unlock);
    }

    public static Player GetInstance()
    {
        return instance;
    }
}
