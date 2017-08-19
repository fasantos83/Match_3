using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoalTimed : LevelGoal {


    public override void Start() {
        LevelCounter = LevelCounter.Timer;
        base.Start();

        if(UIManager.Instance.timer != null) {
            UIManager.Instance.timer.InitTimer(timeLeft);
        }
    }

    

    public override bool IsWinner() {
        return ScoreManager.Instance.CurrentScore >= scoreGoals[0];
    }

    public override bool IsGameOver() {
        int maxScore = scoreGoals[scoreGoals.Length - 1];

        return ScoreManager.Instance.CurrentScore >= maxScore || timeLeft <= 0;
    }

   
}
