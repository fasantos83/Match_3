using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LevelGoal))]
public class GameManager : Singleton<GameManager> {

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
        DontDestroyOnLoad(this.gameObject);

        board = FindObjectOfType<Board>().GetComponent<Board>();
        levelGoal = GetComponent<LevelGoal>();
        levelGoalCollected = GetComponent<LevelGoalCollected>();
    }

    void InitLevel() {
        InitLevelConfig();
        SetupUI();
        UpdateMovesUI();
        StartCoroutine("ExecuteGameLoop");
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        InitLevel();
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void InitLevelConfig() {
        Level currentLevel = LevelManager.Instance.CurrentLevel;
        board.width = currentLevel.boardWidth;
        board.height = currentLevel.boardHeight;
        levelGoal.scoreGoals = currentLevel.scoreGoals;
        levelGoal.movesLeft = currentLevel.moves;
        levelGoal.timeLeft = currentLevel.time;
    }

    private void SetupUI() {
        I18nManager.Instance.Init();

        Screen.SetResolution(540, 960, false);

        if (UIManager.Instance.scoreMeter != null) {
            UIManager.Instance.scoreMeter.SetupStars(levelGoal);
        }

        if (UIManager.Instance.levelNameText != null) {
            UIManager.Instance.levelNameText.text = "level " + LevelManager.Instance.CurrentLevel.index;
        }

        if (levelGoalCollected != null) {
            UIManager.Instance.EnableCollectionGoalLayout(true);
            UIManager.Instance.SetupCollectionGoalLayout(levelGoalCollected.collectionGoals);
        } else {
            UIManager.Instance.EnableCollectionGoalLayout(false);
        }

        UIManager.Instance.EnableTimer(IsTimedLevel());
        UIManager.Instance.EnableMovesCounter(IsMovesLevel());
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
        return levelGoal != null && levelGoal.LevelCounter == LevelCounter.Timer;
    }

    public bool IsMovesLevel() {
        return levelGoal != null && levelGoal.LevelCounter == LevelCounter.Moves;
    }

    public void BeginGame() {
        isReadyToBegin = true;
    }

    public void ReloadScene() {
        isReadyToReload = true;
    }

    IEnumerator ExecuteGameLoop() {
        yield return StartCoroutine(StartGameRoutine());
        yield return StartCoroutine(PlayGameRoutine());
        yield return StartCoroutine(WaitForBoardRoutine(0.5f));
        yield return StartCoroutine(EndGameRoutine());
    }

    IEnumerator StartGameRoutine() {
        if (UIManager.Instance.messageWindow != null) {
            UIManager.Instance.messageWindow.GetComponent<RectTransformMover>().MoveOn();
            int maxGoal = levelGoal.scoreGoals.Length - 1;
            UIManager.Instance.messageWindow.ShowScoreMessage(levelGoal.scoreGoals[maxGoal]);

            if (levelGoal.LevelCounter == LevelCounter.Timer) {
                UIManager.Instance.messageWindow.ShowTimedGoal(levelGoal.timeLeft);
            } else if (levelGoal.LevelCounter == LevelCounter.Moves) {
                UIManager.Instance.messageWindow.ShowMovesGoal(levelGoal.movesLeft);
            }

            if (levelGoalCollected != null) {
                UIManager.Instance.messageWindow.ShowCollectionGoal(true);

                GameObject goalLayout = UIManager.Instance.messageWindow.collectionGoalLayout;
                if (goalLayout != null) {
                    UIManager.Instance.SetupCollectionGoalLayout(levelGoalCollected.collectionGoals, goalLayout, 70);
                }
            }
        }

        while (!isReadyToBegin) {
            yield return null;
        }

        if (UIManager.Instance.screenFader != null) {
            UIManager.Instance.screenFader.FadeOff();
        }

        yield return new WaitForSeconds(1f);

        if (board != null) {
            board.Init();
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
            ShowWinScreen();
        } else {
            ShowLoseScreen();
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

    void ShowWinScreen() {
        if (UIManager.Instance.messageWindow != null) {
            UIManager.Instance.messageWindow.GetComponent<RectTransformMover>().MoveOn();
            UIManager.Instance.messageWindow.ShowWinMessage();

            UIManager.Instance.messageWindow.ShowCollectionGoal(false);
            string scoreStr = I18nManager.Instance.getText("you scored\n{0}", ScoreManager.Instance.CurrentScore);
            UIManager.Instance.messageWindow.ShowGoalCaption(scoreStr, 0, 50);
            UIManager.Instance.messageWindow.ShowGoalImage(UIManager.Instance.messageWindow.goalCompleteIcon);
        }

        SoundManager.Instance.PlayWinSound();
    }

    void ShowLoseScreen() {
        if (UIManager.Instance.messageWindow != null) {
            UIManager.Instance.messageWindow.GetComponent<RectTransformMover>().MoveOn();
            UIManager.Instance.messageWindow.ShowLoseMessage();

            UIManager.Instance.messageWindow.ShowCollectionGoal(false);
            string caption = "";
            if (levelGoal.LevelCounter == LevelCounter.Timer) {
                caption = I18nManager.Instance.getText("out of time!");
            } else if (levelGoal.LevelCounter == LevelCounter.Moves) {
                caption = I18nManager.Instance.getText("out of moves!");
            }
            UIManager.Instance.messageWindow.ShowGoalCaption(caption, 0, 50);
            UIManager.Instance.messageWindow.ShowGoalImage(UIManager.Instance.messageWindow.goalFailIcon);
        }

        SoundManager.Instance.PlayLoseSound();
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
        if (levelGoalCollected != null && pieceToCheck != null) {
            levelGoalCollected.UpdateGoals(pieceToCheck);
        }
    }

    public void ChangeLanguage() {
        Locale locale = I18nManager.Instance.locale;
        Debug.Log("ChangeLanguage");
        switch (locale) {
            case Locale.enUS:
                Debug.Log("to BR");
                I18nManager.Instance.SwitchLanguage(Locale.ptBR);
                break;
            case Locale.ptBR:
                Debug.Log("to EN");
                I18nManager.Instance.SwitchLanguage(Locale.enUS);
                break;
            default:
                break;
        }
    }

    public void LoadNextLevel() {
        string nextLevel = LevelManager.Instance.SetupNextLevel();
        MenuManager.Instance.LoadLevel(nextLevel);
        Destroy(gameObject);
    }
}
