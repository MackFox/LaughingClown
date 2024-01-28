using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip sound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<AudioSource>().PlayOneShot(sound);

            // ToDo Player a Sound
            Debug.Log("Player Triggered a Sound Trap");
            ClownAgent.GetInstance().SetNewDestination(transform.position);
        }
    }
}
