using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelCounter {
    Timer,
    Moves
}

public abstract class LevelGoal : Singleton<LevelGoal> {

    public int scoreStars = 0;
    public int[] scoreGoals = new int[3] { 1000, 2000, 3000 };

    public int movesLeft = 30;
    public int timeLeft = 60;

    int maxTime;
    LevelCounter levelCounter;
    public LevelCounter LevelCounter {
        get { return levelCounter; }
        set { levelCounter = value; }
    }

    public virtual void Start() {
        Init();
    }

    public void Init() {
        scoreStars = 0;
        for (int i = 1; i < scoreGoals.Length; i++) {
            if (scoreGoals[i] < scoreGoals[i - 1]) {
                Debug.LogWarning("LEVELGOAL Setup score goals in increasing order!");
            }
        }

        if (LevelCounter == LevelCounter.Timer) {
            maxTime = timeLeft;
        }
    }

    int UpdateScore(int score) {
        int scoreGoal = 0;

        int i = 0;
        do {
            if (score >= scoreGoals[i++]) {
                scoreGoal = i;
            }
        } while (i < scoreGoals.Length);

        return scoreGoal;
    }

    public void UpdateScoreStars(int score) {
        scoreStars = UpdateScore(score);
    }

    public abstract bool IsWinner();
    public abstract bool IsGameOver();

    public void StartCountdown() {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine() {
        while (timeLeft > 0) {
            yield return new WaitForSeconds(1f);
            timeLeft--;

            if (UIManager.Instance.timer != null) {
                UIManager.Instance.timer.UpdateTimer(timeLeft);
            }
        }
    }

    public void AddTime(int timeValue) {
        timeLeft += timeValue;
        timeLeft = Mathf.Clamp(timeLeft, 0, maxTime);

        if (UIManager.Instance.timer != null) {
            UIManager.Instance.timer.UpdateTimer(timeLeft);
        }
    }
}
