using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ClownAgent;

public class ClownSoundManager : MonoBehaviour
{
    private static ClownSoundManager instance;

    [SerializeField] private AudioSource _laughSource;
    [SerializeField] private AudioSource _laughSource2;
    private AudioClip _currentLaughClip;
    private float _laughDefaultVolume;
    [SerializeField] private AudioSource _movementSource;
    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] private AudioClip _seesYou;
    [SerializeField] private AudioClip _hearsYou;
    [SerializeField] private AudioClip _huntsYou;
    [SerializeField] private AudioClip _heSpawns;
    [SerializeField] private AudioClip _lastLaugh;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _laughDefaultVolume = _laughSource.volume;
    }

    public void PlayLaugh(EnemyStates enemyState)
    {
        StopAllCoroutines();
        switch (enemyState)
        {
            case EnemyStates.Searching:
                StartCoroutine(CrossfadeCoroutine(null));
                //StartCoroutine(FadeAndChangeClip(_laughSource, null, _laughDefaultVolume));
                break;
            case EnemyStates.Seeing:
                StartCoroutine(CrossfadeCoroutine(_seesYou));
                //StartCoroutine(FadeAndChangeClip(_laughSource, _seesYou, _laughDefaultVolume));
                //PlayLaughSource(_seesYou);
                break;
            case EnemyStates.Following:
                StartCoroutine(CrossfadeCoroutine(_huntsYou));
                //StartCoroutine(FadeAndChangeClip(_laughSource, _huntsYou, _laughDefaultVolume));
                //PlayLaughSource(_huntsYou);
                break;
            case EnemyStates.Reached:
                StartCoroutine(CrossfadeCoroutine(null));
                //StartCoroutine(FadeAndChangeClip(_laughSource, null, _laughDefaultVolume));
                break;
            case EnemyStates.Death:
                StartCoroutine(CrossfadeCoroutine(_lastLaugh));
                //StartCoroutine(FadeAndChangeClip(_laughSource, _lastLaugh, _laughDefaultVolume));
                //PlayLaughSource(_lastLaugh);
                break;
            default:
                break;
        }
    }

    IEnumerator FadeAndChangeClip(AudioSource audioSource, AudioClip newClip, float defaultVolume)
    {
        Debug.Log("Player of Sound: " + newClip);
        // Faden aus
        while (audioSource.volume > 0)
        {
            audioSource.volume -= defaultVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Wenn ein neuer Clip vorhanden ist, wechsle zu diesem, sonst spiele den aktuellen Clip weiter
        if (newClip != null)
        {
            audioSource.clip = newClip;
            _currentLaughClip = newClip;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        // Faden ein
        while (audioSource.volume < defaultVolume)
        {
            audioSource.volume += defaultVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    private IEnumerator CrossfadeCoroutine(AudioClip newClip)
    {
        float timer = 0.0f;
        float startVolume = _laughSource.volume;
        float endVolume = 0.0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            _laughSource.volume = Mathf.Lerp(startVolume, endVolume, timer / fadeDuration);
            _laughSource2.volume = Mathf.Lerp(endVolume, startVolume, timer / fadeDuration);
            yield return null;
        }

        // Wechsle die Audioquellen
        AudioSource temp = _laughSource;
        _laughSource = _laughSource2;
        _laughSource2 = temp;

        // Setze den neuen AudioClip und starte das Abspielen der neuen Audioquelle
        if (newClip != null)
        {
            _laughSource.clip = newClip;
            _laughSource.Play();
        }
    }

    private void PlayLaughSource(AudioClip nextClip)
    {
        Debug.Log("Player of Sound: " + nextClip);
        _laughSource.clip = nextClip;
        _laughSource.Play();
    }

    public static ClownSoundManager GetInstance()
    {
        return instance;
    }
}
