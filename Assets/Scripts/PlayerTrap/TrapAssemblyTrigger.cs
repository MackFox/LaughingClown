using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAssemblyTrigger : MonoBehaviour
{
    [SerializeField] private CollectableType _requiredCollectable;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && GameManager.GetInstance().CurrentCollectable == _requiredCollectable)
        {
            TrapController.GetInstance().AddCollectable(_requiredCollectable);
        }
    }
}
