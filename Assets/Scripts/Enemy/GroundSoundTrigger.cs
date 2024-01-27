using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSoundTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // ToDo Player a Sound
            Debug.Log("Player Triggered a Sound Trap");
            ClownAgent.GetInstance().SetNewDestination(transform.position);
        }
    }
}
