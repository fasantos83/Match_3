using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoalCollected : LevelGoal {

    public CollectionGoal[] collectionGoals;

    public void UpdateGoals(GamePiece pieceToCheck) {
        if (pieceToCheck != null) {
            foreach (CollectionGoal goal in collectionGoals) {
                if (goal != null) {
                    goal.CollectPiece(pieceToCheck);
                }
            }
        }

        UpdateUI();
    }

    public void UpdateUI() {
        UIManager.Instance.UpdateCollectionGoalLayout();
    }

    bool AreGoalsComplete(CollectionGoal[] goals) {
        bool areGoalsComplete = true;

        foreach (CollectionGoal goal in goals) {
            if (goals == null || goals.Length == 0 || goal == null || goal.numberToCollect != 0) {
                areGoalsComplete = false;
            }
        }

        return areGoalsComplete;
    }

    public override bool IsGameOver() {
        bool isGameOver = false;

        if (AreGoalsComplete(collectionGoals)) {
            int maxScore = scoreGoals[scoreGoals.Length - 1];
            isGameOver = ScoreManager.Instance.CurrentScore >= maxScore;
        }

        if(levelCounter == LevelCounter.Timer) {
            isGameOver = isGameOver || timeLeft <= 0;
        } else {
            isGameOver = isGameOver || movesLeft <= 0;
        }

        return isGameOver;
    }

    public override bool IsWinner() {
        return ScoreManager.Instance.CurrentScore >= scoreGoals[0] && AreGoalsComplete(collectionGoals);
    }
}
