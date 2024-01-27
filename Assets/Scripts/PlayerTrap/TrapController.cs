using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [Tooltip("Needs correct Order from CollectableType Enum!")]
    [SerializeField] private List<GameObject> _trapParts;
    [SerializeField] private Material _testMaterial;
    [SerializeField] private GameObject _killTrigger;

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

    private void Start()
    {

    }

    private void CheckIfAllItemAreAssemblied()
    {
        if (AllCollecteablesAssembeld())
        {
            Debug.Log("Player has all Items collected!");
            ActivateTrap();
        }
    }

    private void ActivateTrap()
    {
        Debug.Log("Activation of Trap!");
        _killTrigger.SetActive(true);
    }


    // ToDo: add different Triggerboxes with holo Collectables, that invoke this function
    public void AddCollectable(CollectableType newCollectable)
    {
        Debug.Log($"Player has add the {newCollectable} to the Trap!");
        _collectableStatus[newCollectable] = true;
        ActivateTrapPart((int)newCollectable);
        CheckIfAllItemAreAssemblied();
    }

    private void ActivateTrapPart(int index)
    {
        // -1 because we need to ignore the "None" state in the enum
        _trapParts[index - 1].GetComponent<Renderer>().material = _testMaterial;
        Player.GetInstance().RemoveItemFromPlayerHand();
    }

    bool AllCollecteablesAssembeld()
    {
        foreach (var pair in _collectableStatus)
        {
            if (!pair.Value)
                return false;
        }
        return true;
    }

    public static TrapController GetInstance()
    {
        return instance;
    }
}
