using System.Collections;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LevelGoal))]
public class GameManager : Singleton<GameManager> {

    //public int movesLeft = 30;
    //public int scoreGoal = 10000;

    public Sprite loseIcon;
    public Sprite winIcon;
    public Sprite goalIcon;

    Board board;

    LevelGoal levelGoal;
    public LevelGoal LevelGoal {
        get { return levelGoal; }
    }
    
    LevelGoalCollected levelGoalCollected;

    bool isReadyToBegin = false;
    bool isGameOver = false;
    public bool IsGameOver {
        get { return isGameOver; }
        set { isGameOver = value; }
    }

    bool isWinner = false;
    bool isReadyToReload = false;

    public override void Awake() {
        base.Awake();
        board = FindObjectOfType<Board>().GetComponent<Board>();
        levelGoal = GetComponent<LevelGoal>();
        levelGoalCollected = GetComponent<LevelGoalCollected>();
    }

    void Start() {
        Screen.SetResolution(540, 960, false);

        if (UIManager.Instance.scoreMeter != null) {
            UIManager.Instance.scoreMeter.SetupStars(levelGoal);
        }

        Scene scene = SceneManager.GetActiveScene();
        if (UIManager.Instance.levelNameText != null) {
            UIManager.Instance.levelNameText.text = scene.name;
        }

        if (levelGoalCollected != null) {
            UIManager.Instance.EnableCollectionGoalLayout(true);
            UIManager.Instance.SetupCollectionGoalLayout(levelGoalCollected.collectionGoals);
        } else {
            UIManager.Instance.EnableCollectionGoalLayout(false);
        }

        UIManager.Instance.EnableTimer(IsTimedLevel());
        UIManager.Instance.EnableMovesCounter(IsMovesLevel());

        UpdateMovesUI();

        StartCoroutine("ExecuteameLoop");
    }

    public void UpdateMoves() {
        if (IsMovesLevel()) {
            levelGoal.movesLeft--;
        }
        UpdateMovesUI();
    }

    void UpdateMovesUI() {
        if (UIManager.Instance.movesLeftText != null) {
            UIManager.Instance.movesLeftText.text = levelGoal.movesLeft.ToString();
        }
    }

    public bool IsTimedLevel() {
        return levelGoal != null && levelGoal.levelCounter == LevelCounter.Timer;
    }

    public bool IsMovesLevel() {
        return levelGoal != null && levelGoal.levelCounter == LevelCounter.Moves;
    }

    public void BeginGame() {
        isReadyToBegin = true;
    }

    public void ReloadScene() {
        isReadyToReload = true;
    }

    IEnumerator ExecuteameLoop() {
        yield return StartCoroutine(StartGameRoutine());
        yield return StartCoroutine(PlayGameRoutine());
        yield return StartCoroutine(WaitForBoardRoutine(0.5f));
        yield return StartCoroutine(EndGameRoutine());
    }

    IEnumerator StartGameRoutine() {
        if (UIManager.Instance.messageWindow != null) {
            UIManager.Instance.messageWindow.GetComponent<RectTransformMover>().MoveOn();
            UIManager.Instance.messageWindow.ShowMessage(goalIcon, "score goal\n" + levelGoal.scoreGoals[0].ToString(), "start");
        }

        while (!isReadyToBegin) {
            yield return null;
        }

        if (UIManager.Instance.screenFader != null) {
            UIManager.Instance.screenFader.FadeOff();
        }

        yield return new WaitForSeconds(1f);

        if (board != null) {
            board.SetupBoard();
        }
    }

    IEnumerator PlayGameRoutine() {
        if (IsTimedLevel()) {
            levelGoal.StartCountdown();
        }

        while (!isGameOver) {
            isGameOver = levelGoal.IsGameOver();
            isWinner = levelGoal.IsWinner();

            yield return null;
        }
    }

    IEnumerator WaitForBoardRoutine(float delay = 0f) {
        if (IsTimedLevel()) {
            Timer timer = UIManager.Instance.timer;
            if (timer != null) {
                timer.FadeOff();
                timer.paused = true;
            }
        }

        if (board != null) {
            yield return new WaitForSeconds(board.swapTime);

            while (board.IsRefilling) {
                yield return null;
            }
        }

        yield return new WaitForSeconds(delay);
    }

    IEnumerator EndGameRoutine() {
        isReadyToReload = false;

        if (isWinner) {
            if (UIManager.Instance.messageWindow != null) {
                UIManager.Instance.messageWindow.GetComponent<RectTransformMover>().MoveOn();
                UIManager.Instance.messageWindow.ShowMessage(winIcon, "you win!", "ok");
            }

            SoundManager.Instance.PlayWinSound();
        } else {
            if (UIManager.Instance.messageWindow != null) {
                UIManager.Instance.messageWindow.GetComponent<RectTransformMover>().MoveOn();
                UIManager.Instance.messageWindow.ShowMessage(loseIcon, "you lose!", "ok");
            }

            SoundManager.Instance.PlayLoseSound();
        }

        yield return new WaitForSeconds(1f);
        if (UIManager.Instance.screenFader != null) {
            UIManager.Instance.screenFader.FadeOn();
        }

        while (!isReadyToReload) {
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ScorePoints(GamePiece piece, int multiplier = 1, int bonus = 0) {
        if (piece != null) {
            ScoreManager.Instance.AddScore(piece.scoreValue * multiplier + bonus);
            levelGoal.UpdateScoreStars(ScoreManager.Instance.CurrentScore);

            if (UIManager.Instance.scoreMeter != null) {
                UIManager.Instance.scoreMeter.UpdateScoreMeter(ScoreManager.Instance.CurrentScore, levelGoal.scoreStars);
            }

            if (piece.clearSound) {
                SoundManager.Instance.PlayClipAtPoint(piece.clearSound, Vector3.zero, SoundManager.Instance.fxVolume);
            }
        }
    }

    public void AddTime(int timeValue) {
        if (IsTimedLevel()) {
            levelGoal.AddTime(timeValue);
        }
    }

    public void UpdateCollectionGoals(GamePiece pieceToCheck) {
        if (pieceToCheck != null) {
            levelGoalCollected.UpdateGoals(pieceToCheck);
        }
    }
}
