using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _killTrigger;
    [Tooltip("Needs correct Order from CollectableType Enum!")]
    [SerializeField] private List<MeshRenderer> ropeParts;
    [SerializeField] private List<MeshRenderer> nails;
    [SerializeField] private MeshRenderer _anivl;

    [Header("Materials")]
    [SerializeField] private Material _holoMaterial;
    [SerializeField] private Material _nailsMaterial;
    [SerializeField] private Material _ropesMaterial;
    [SerializeField] private Material _anvilMaterial;

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
        SetHoloMaterial();
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

        switch (newCollectable)
        {
            case CollectableType.Anvil:
                _anivl.material = _anvilMaterial;
                break;
            case CollectableType.Rope:
                ropeParts.ForEach(renderer => renderer.material = _ropesMaterial);
                break;
            case CollectableType.Nail:
                nails.ForEach(renderer => renderer.material = _nailsMaterial);
                break;
            default:
                break;
        }

        Player.GetInstance().RemoveItemFromPlayerHand();
        CheckIfAllItemAreAssemblied();
    }

    private void SetHoloMaterial()
    {
        _anivl.material = _holoMaterial;
        ropeParts.ForEach(renderer => renderer.material = _holoMaterial);
        nails.ForEach(renderer => renderer.material = _holoMaterial);
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
