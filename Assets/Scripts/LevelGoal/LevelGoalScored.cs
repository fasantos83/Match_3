public class LevelGoalScored : LevelGoal {

    public override void Awake() {
        levelCounter = LevelCounter.Moves;
        base.Awake();
    }
    
    public override bool IsWinner() {
        return ScoreManager.Instance.CurrentScore >= scoreGoals[0];
    }

    public override bool IsGameOver() {
        int maxScore = scoreGoals[scoreGoals.Length - 1];

        return ScoreManager.Instance.CurrentScore >= maxScore || movesLeft <= 0;
    }
}
