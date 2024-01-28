using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ClownAgent;

public class ClownSoundManager : MonoBehaviour
{
    private static ClownSoundManager instance;

    [SerializeField] private AudioSource _movementSource;
    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] private AudioSource _seesYou;
    [SerializeField] private AudioSource _hearsYou;
    [SerializeField] private AudioSource _huntsYou;
    [SerializeField] private AudioSource _heSpawns;
    [SerializeField] private AudioSource _lastLaugh;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void PlayLaugh(EnemyStates enemyState)
    {
        StopAllCoroutines();
        switch (enemyState)
        {
            case EnemyStates.Searching:
                PlayLaughSource(null);
                break;
            case EnemyStates.Seeing:
                PlayLaughSource(_seesYou);
                break;
            case EnemyStates.Following:
                PlayLaughSource(_huntsYou);
                break;
            case EnemyStates.Reached:
                PlayLaughSource(null);
                break;
            case EnemyStates.Death:
                PlayLaughSource(_lastLaugh);
                break;
            default:
                break;
        }
    }

    private void PlayLaughSource(AudioSource newSource)
    {
        if (newSource != null)
        {
            Debug.Log("Player of Sound: " + newSource.name);
            FadeTargetSourceUp(newSource);
        }

        FadeOtherSourcesDown(newSource);
        //_laughSource.Play();
    }

    private void FadeTargetSourceUp(AudioSource targetSource)
    {
        StartCoroutine(FadeVolumeCoroutine(targetSource, 0.0f, 1.0f, fadeDuration));

        // Setze den neuen AudioClip und starte das Abspielen der neuen Audioquelle
        targetSource.Stop();
        targetSource.clip = targetSource.clip;
        targetSource.Play();
    }

    private void FadeOtherSourcesDown(AudioSource targetSource)
    {
        if (_seesYou != targetSource)
            FadeVolume(_seesYou, 0.0f, fadeDuration);
        if (_hearsYou != targetSource)
            FadeVolume(_hearsYou, 0.0f, fadeDuration);
        if (_huntsYou != targetSource)
            FadeVolume(_huntsYou, 0.0f, fadeDuration);
        if (_heSpawns != targetSource)
            FadeVolume(_heSpawns, 0.0f, fadeDuration);
        if (_lastLaugh != targetSource)
            FadeVolume(_lastLaugh, 0.0f, fadeDuration);
    }

    private IEnumerator FadeVolumeCoroutine(AudioSource audioSource, float startVolume, float endVolume, float duration)
    {
        float timer = 0.0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, timer / duration);
            yield return null;
        }

        // Optional: Zurücksetzen der Lautstärke für zukünftige Verwendung
        audioSource.volume = endVolume;
    }

    private void FadeVolume(AudioSource audioSource, float targetVolume, float duration)
    {
        StartCoroutine(FadeVolumeCoroutine(audioSource, audioSource.volume, targetVolume, duration));
    }

    public static ClownSoundManager GetInstance()
    {
        return instance;
    }
}
