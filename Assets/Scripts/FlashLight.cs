using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField] private float flickerIntensity = 0.2f;
    [SerializeField] private float flickerPerSecond = 3.0f;
    [SerializeField] private float speedRandomness = 1.0f;
    [SerializeField] private Light _light;

    private float time;
    private float startingItensity;

    private void Start()
    {
        startingItensity = _light.intensity;
    }

    private void Update()
    {
        // Flicker Effect
        time += Time.deltaTime * (1 - Random.Range(-speedRandomness, speedRandomness)) * Mathf.PI;
        _light.intensity = startingItensity + Mathf.Sin(time * flickerPerSecond) * flickerIntensity;
    }
}
