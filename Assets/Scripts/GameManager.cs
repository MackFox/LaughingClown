using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private GameState _gameState;

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
        DontDestroyOnLoad(this);

        if (instance == null)
            instance = this;

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
}
