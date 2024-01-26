using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private bool isCollected;
    void Start()
    {
        
    }

    private void PlayCollectAnimation()
    {
        Debug.Log("Item is collected, playing Animation");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isCollected = true;
            PlayCollectAnimation();
        }
    }
}
