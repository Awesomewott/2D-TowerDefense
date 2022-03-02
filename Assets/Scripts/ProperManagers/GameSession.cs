using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GameSession : MonoBehaviour
{
    public bool gameWon = false;

    static GameSession instance = null;
    public static GameSession Instance
    {
        get
        {
            return instance;
        }
    }

    public enum eState
    {
        Load,
        StartSession,
        Session,
        EndSession,
        GameOver,
        WinGame,
        Quit
    }

    public eState State { get; set; } = eState.Load;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    private void Update()
    {
        switch (State)
        {
            case eState.Load:

                State = eState.StartSession;
                break;
            case eState.StartSession:
                State = eState.Session;
                break;
            case eState.Session:
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    GameController.Instance.OnPause();
                }
                CheckWin();
                break;
            case eState.EndSession:
                if (gameWon) State = eState.WinGame;
                else State = eState.GameOver;
                break;
            case eState.GameOver:
                if (GameController.Instance.gameOverScreen != null) GameController.Instance.gameOverScreen.SetActive(true);
                State = eState.Quit;
                break;
            case eState.WinGame:
                State = eState.Quit;
                break;
            case eState.Quit:
                break;
            default:
                break;
        }        
    }

    public void QuitToMainMenu()
    {
        GameController.Instance.OnLoadMenuScene("GameController");
    }

    private void CheckWin()
    {
        if (gameWon)
        {
            State = eState.EndSession;
        }
    }
}
