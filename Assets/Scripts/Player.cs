using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        
    }


    public static Player GetInstance()
    {
        return instance;
    }
}
