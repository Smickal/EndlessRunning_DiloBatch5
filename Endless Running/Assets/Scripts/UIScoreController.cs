using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreController : MonoBehaviour
{
    [Header("UI")]
    public Text score;
    public Text HighScore;

    [Header("Score")]
    public ScoreController scoreController;

    
    void Update()
    {
        score.text = scoreController.GetCurrentScore().ToString();
        HighScore.text = ScoreData.highScore.ToString();
    }
}
