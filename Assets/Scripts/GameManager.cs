using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>{

    public int movesLeft = 30;
    public int scoreGoal = 10000;
    public Text levelNameText;
    public Text movesLeftText;

    public Sprite loseIcon;
    public Sprite winIcon;
    public Sprite goalIcon;

    public ScreenFader screenFader;
    public MessageWindow messageWindow;

    Board board;

    bool isReadyToBegin = false;
    bool isGameOver = false;
    bool isWinner = false;
    bool isReadyToReload = false;

	void Start () {
        Screen.SetResolution(540, 960, false);

        board = FindObjectOfType<Board>().GetComponent<Board>();

        Scene scene = SceneManager.GetActiveScene();
        if(levelNameText != null) {
            levelNameText.text = scene.name;
        }
        UpdateMoves();

        StartCoroutine("ExecuteameLoop");
    }

    public void UpdateMoves() {
        if(movesLeftText != null) {
            movesLeftText.text = movesLeft.ToString();
        }
    }

    public void BeginGame() {
        isReadyToBegin = true;
    }

    public void ReloadScene() {
        isReadyToReload = true;
    }
	
	IEnumerator ExecuteameLoop() {
        yield return StartCoroutine("StartGameRoutine");
        yield return StartCoroutine("PlayGameRoutine");
        yield return StartCoroutine("EndGameRoutine");
    }

    IEnumerator StartGameRoutine() {
        if(messageWindow != null) {
            messageWindow.GetComponent<RectTransformMover>().MoveOn();
            messageWindow.ShowMessage(goalIcon, "score goal\n" + scoreGoal.ToString(), "start");
        }

        while (!isReadyToBegin) {
            yield return null;
        }

        if(screenFader != null) {
            screenFader.FadeOff();
        }

        yield return new WaitForSeconds(1f);

        if (board != null) {
            board.SetupBoard();
        }
    }

    IEnumerator PlayGameRoutine() {
        while (!isGameOver) {
            if(ScoreManager.Instance != null) {
                if(ScoreManager.Instance.CurrentScore >= scoreGoal) {
                    isGameOver = true;
                    isWinner = true;
                }
            }

            if(movesLeft <= 0) {
                isGameOver = true;
                isWinner = false;
            }

            yield return null;
        }
    }

    IEnumerator EndGameRoutine() {
        while (!board.PlayerInputEnabled) {
            yield return new WaitForSeconds(0.5f);
        }

        isReadyToReload = false;

        if (isWinner) {
            if(messageWindow != null) {
                messageWindow.GetComponent<RectTransformMover>().MoveOn();
                messageWindow.ShowMessage(winIcon, "you win!", "ok");
            }

            if (SoundManager.Instance != null) {
                SoundManager.Instance.PlayWinSound();
            }
        } else {
            if (messageWindow != null) {
                messageWindow.GetComponent<RectTransformMover>().MoveOn();
                messageWindow.ShowMessage(loseIcon, "you lose!", "ok");
            }

            if (SoundManager.Instance != null) {
                SoundManager.Instance.PlayLoseSound();
            }
        }

        yield return new WaitForSeconds(1f);
        if (screenFader != null) {
            screenFader.FadeOn();
        }

        while (!isReadyToReload) {
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
