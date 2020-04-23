//2020-04-23
//Matthew Demoe 
//Developed for Directed Studies in IT under Alvaro Joffre Uribe-Quevedo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timeText;

    [SerializeField]
    TextMeshProUGUI instructionText;

    [SerializeField]
    TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
    }

    void UpdateTime()
    {
        timeText.text = ((int)GameLoop.Instance().GetGameTime()).ToString();
    }

    public void UpdateInstructions()
    {
        switch (GameLoop.Instance().GetState())
        {
            case GameState.MovingHorizontal:
                instructionText.text = "Close fist to move vertically\nOpen fist to stop";
                break;

            case GameState.MovingVertical:
                instructionText.text = "Flex wrist to lower claw\nHook fingers to grab a coin\nExtend wrist to raise claw";
                break;

            case GameState.Lowering:
                instructionText.text = "Relax fingers to drop coin";
                break;

            case GameState.Resetting:
                instructionText.text = "Close fist to move horizontally\nOpen fist to stop";
                break;
        }
    }

    public void UpdateScore()
    {
        scoreText.text = GameLoop.Instance().GetScore().ToString();
    }
}
