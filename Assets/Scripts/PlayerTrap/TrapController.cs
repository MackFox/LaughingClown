using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    private static TrapController instance;
    private Dictionary<CollectableType, bool> _collectableStatus = new Dictionary<CollectableType, bool>();

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _collectableStatus.Add(CollectableType.Anvil, false);
        _collectableStatus.Add(CollectableType.Rope, false);
        _collectableStatus.Add(CollectableType.Nail, false);
    }

    void Start()
    {

    }


    // ToDo: add different Triggerboxes with holo Collectables, that invoke this function
    public void AddCollectable(CollectableType newCollectable)
    {
        Debug.Log($"Player has add the {newCollectable} to the Trap!");
        _collectableStatus[newCollectable] = true;
    }

    public static TrapController GetInstance()
    {
        return instance;
    }
}
