using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int currentScore = 0;
    void Start()
    {
        currentScore = 0;
    }

    // Update is called once per frame

    public float GetCurrentScore()
    {
        return currentScore;
    }

    public void IncreaseCurrentScore(int increment)
    {
        currentScore += increment;
    }

    public void FinishScoring()
    {
        if(currentScore > ScoreData.highScore)
        {
            ScoreData.highScore = currentScore;
        }
    }
}
