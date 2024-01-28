using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private GameState _gameState;

    [SerializeField] private AudioSource _audioSource;
    public AudioSource AudioSource => _audioSource;


    [SerializeField] private AudioSource _bgmSource;
    public AudioSource BackgroundMusicSource => _bgmSource;


    [SerializeField] private AudioClip _laughing;
    [SerializeField] private AudioClip _end;
    [SerializeField] private AudioClip _win;

    [SerializeField] private AudioClip _bgmMenu;
    [SerializeField] private AudioClip _bgmGame;

    public GameState CurrentGameState => _gameState;

    public enum GameState
    {
        Start = 0,
        Intro = 1,
        Playing = 2,
        End = 3,
        Pause = 4,
    }

    private void Awake()
    {

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
        SetGameState(GameState.Start);
    }



    public void SetGameState(GameState newGameState)
    {
        _gameState = newGameState;
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    private CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.0f;

    private void Start()
    {
        fadeCanvasGroup = CanvasScript.instance.fade;
        // Start with the fade canvas fully transparent
        fadeCanvasGroup.alpha = 0f;

        _bgmSource.Stop();
        _bgmSource.clip = _bgmMenu;
        _bgmSource.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasScript.instance.SetPauseScreen();
        }
    }

    public void LoadLevel1()
    {
        TransitionToScene(1);
        CanvasScript.instance.DeactivateAllScreens();

        _bgmSource.Stop();
        _bgmSource.clip = _bgmGame;
        _bgmSource.Play();

        _audioSource.PlayOneShot(_laughing);
    }
    public void BackToMainMenu()
    {
        TransitionToScene(0);
    }

    public void TransitionToScene(int sceneIndex)
    {
        StartCoroutine(FadeAndLoadScene(sceneIndex));
    }

    IEnumerator FadeAndLoadScene(int sceneIndex)
    {
        // Fade out
        StartCoroutine(FadeCanvasGroup(fadeCanvasGroup, fadeDuration));

        // Wait for the fade to complete
        yield return new WaitForSeconds(fadeDuration);

        // Load the new scene
        SceneManager.LoadScene(sceneIndex);

        if (sceneIndex != 0) _gameState = GameState.Playing;
        else _gameState = GameState.Intro;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        while (elapsedTime > 0)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime -= Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}

