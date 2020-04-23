//2020-04-23
//Matthew Demoe 
//Developed for Directed Studies in IT under Alvaro Joffre Uribe-Quevedo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState { MovingHorizontal, MovingVertical, Lowering, Resetting };

public class GameLoop : MonoBehaviour
{
    private static GameLoop _instance = null;

    public static GameLoop Instance()
    {
        if (_instance == null)
            _instance = new GameObject().AddComponent<GameLoop>();

        return _instance;
    }


    float gameTimer = 0.0f;

    [SerializeField]
    float maxGameTimer = 100.0f;


    [SerializeField]
    ClawController clawController;

    bool clawIsMoving = false;

    int score = 0;

    [SerializeField]
    GameState currentState = GameState.MovingHorizontal;

    [SerializeField]
    UnityEvent onGameStateAdvance = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;

        onGameStateAdvance.AddListener(() => { 
            currentState++;
            currentState = (GameState)((int)currentState % 4);
            clawIsMoving = !clawIsMoving;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (gameTimer > 0.0f)
        {
            gameTimer -= Time.deltaTime;

            TheLoop();
        }

        else
        {

        }
    }

    public void AdvanceGameState()
    {
        onGameStateAdvance.Invoke();
    }

    void TheLoop()
    {
        if (!clawIsMoving)
        {
            switch (currentState)
            {
                case GameState.MovingHorizontal:
                    if (clawController.HandIsClosed())
                        clawIsMoving = !clawIsMoving;
                    break;

                case GameState.MovingVertical:
                    if (clawController.HandIsClosed())
                        clawIsMoving = !clawIsMoving;
                    break;

                case GameState.Lowering:
                    if (clawController.WristIsFlexed())
                        clawIsMoving = !clawIsMoving;
                    break;

                case GameState.Resetting:
                    if (clawController.WristIsStraight())
                    {
                        clawIsMoving = !clawIsMoving;
                        clawController.ResetClaw();
                    }
                    break;
            }
        }

        else
        {
            switch (currentState)
            {
                case GameState.MovingHorizontal:
                    clawController.MoveHorizontal();

                    if (clawController.HandIsOpen())
                        onGameStateAdvance.Invoke();
                    break;

                case GameState.MovingVertical:
                    clawController.MoveVertical();

                    if (clawController.HandIsOpen())
                        onGameStateAdvance.Invoke();
                    break;

                case GameState.Lowering:
                    clawController.Lower();

                    if (clawController.WristIsStraight())
                        onGameStateAdvance.Invoke();
                    break;

               case GameState.Resetting:
                   if (clawController.GetIsReset())
                   {
                       onGameStateAdvance.Invoke();
                   }
                   break;
            }
        }
    }

    public GameState GetState()
    {
        return currentState;
    }

    public float GetGameTime()
    {
        return gameTimer;
    }

    public void IncrementScore()
    {
        score++;
    }

    public int GetScore()
    {
        return score;
    }

    public void StartGame()
    {
        gameTimer = maxGameTimer;
    }
}
