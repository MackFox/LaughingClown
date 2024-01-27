using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Clown entered the kill trigger and is now dead!, you win!");
        }
    }
}
