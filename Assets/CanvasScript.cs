using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    public static CanvasScript instance;

    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject creditsScreen;

    [SerializeField] private CanvasGroup _fade;
    public CanvasGroup fade => _fade;

    public enum CanvasState
    {
        menu,
        pause,
        win,
        lose,
        hidden,
    }

    private CanvasState _state;
    public CanvasState State => _state;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void BackToMenu()
    {
        SetMenuScreen();
        GameManager.GetInstance().BackToMainMenu();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    public void SetMenuScreen()
    {
        DeactivateAllScreens();
        if (menuScreen != null) menuScreen.SetActive(true);
    }
    public void SetPauseScreen()
    {
        DeactivateAllScreens();
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
            Player.GetInstance().UnlockCursor(true);
            Time.timeScale = 0;
        }
    }
    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
        Player.GetInstance().UnlockCursor(false);
    }

    public void SetWinScreen()
    {
        DeactivateAllScreens();
        if (winScreen != null) winScreen.SetActive(true);
        Player.GetInstance().UnlockCursor(true);
        Time.timeScale = 0;
    }

    public void SetLoseScreen()
    {
        DeactivateAllScreens();
        if (loseScreen != null) loseScreen.SetActive(true);
    }
    public void SetCreditsScreen()
    {
        DeactivateAllScreens();
        if (creditsScreen != null) creditsScreen.SetActive(true);
    }

    public void DeactivateAllScreens()
    {
        if (menuScreen != null) menuScreen.SetActive(false);
        if (pauseScreen != null) pauseScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
        if (winScreen != null) winScreen.SetActive(false);
        if (creditsScreen != null) creditsScreen.SetActive(false);
    }
}
