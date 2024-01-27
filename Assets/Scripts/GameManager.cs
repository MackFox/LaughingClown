using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private GameState _gameState;

    public GameState CurrentGameState => _gameState;
    public CollectableType CurrentCollectable { get; private set; }

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

    public void SetCollectable(CollectableType collectable)
    {
        CurrentCollectable = collectable;
    }

    public static GameManager GetInstance()
    {
        return instance;
    }
}
