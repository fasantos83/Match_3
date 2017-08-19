using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransformMover))]
public class MessageWindow : MonoBehaviour {

    [Header("Message Panel")]
    public Image messageImage;
    public Text messageText;

    public Sprite loseIcon;
    public Sprite winIcon;
    public Sprite goalIcon;

    [Header("Goal Panel")]
    public Image goalImage;
    public Text goalText;

    public Sprite collectIcon;
    public Sprite timerIcon;
    public Sprite movesIcon;

    public Sprite goalCompleteIcon;
    public Sprite goalFailIcon;

    public GameObject collectionGoalLayout;

    [Header("Button")]
    public Button button;
    public Text buttonText;

    public enum ButtonAction {
        start,
        nextLevel
    }
    public void ShowMessage(Sprite sprite = null, string message = "", string buttonMsg = "start", ButtonAction action = ButtonAction.start) {
        if(messageImage != null) {
            messageImage.sprite = sprite;
        }

        if(messageText != null) {
            messageText.text = message;
        }

        if(button != null) {
            switch (action) {
                case ButtonAction.start:
                    button.onClick.AddListener(delegate { GameManager.Instance.BeginGame(); });
                    break;
                case ButtonAction.nextLevel:
                    button.onClick.AddListener(delegate { GameManager.Instance.LoadNextLevel(); });
                    break;
                default:
                    break;
            }
        }

        if (buttonText != null) {
            buttonText.text = I18nManager.Instance.getText(buttonMsg);
        }
    }

    public void ShowScoreMessage(int scoreGoal) {
        string message = I18nManager.Instance.getText("score goal\n{0}", scoreGoal);
        ShowMessage(goalIcon, message, "start");
    }

    public void ShowWinMessage() {
        ShowMessage(winIcon, I18nManager.Instance.getText("level\ncomplete"), "next level", ButtonAction.nextLevel);
    }

    public void ShowLoseMessage() {
        ShowMessage(loseIcon, I18nManager.Instance.getText("level\nfailed"), "ok");
    }

    public void ShowGoal(string caption = "", Sprite icon = null, int offsetX = 0, int offsetY = 0) {
        if(caption != "") {
            ShowGoalCaption(caption, offsetX, offsetY);
        }

        if(icon != null) {
            ShowGoalImage(icon);
        }
    }

    public void ShowGoalCaption(string caption = "", int offsetX = 0, int offsetY = 0) {
        if(goalText != null) {
            goalText.text = caption;
            RectTransform rectTransform = goalText.GetComponent<RectTransform>();
            rectTransform.anchoredPosition += new Vector2(offsetX, offsetY);
        }
    }

    public void ShowGoalImage (Sprite icon = null) {
        if(goalImage != null) {
            goalImage.gameObject.SetActive(icon != null);
            goalImage.sprite = icon;
        }
    }
    public void ShowTimedGoal(int time) {
        string caption = I18nManager.Instance.getText("{0} seconds", time);
        ShowGoal(caption, timerIcon);
    }

    public void ShowMovesGoal(int moves) {
        string caption = I18nManager.Instance.getText("{0} moves", moves);
        ShowGoal(caption, movesIcon);
    }

    public void ShowCollectionGoal(bool state = true) { 
        if(collectionGoalLayout != null){
            collectionGoalLayout.SetActive(state);
        }

        if (state) {
            ShowGoal("", collectIcon);
        }
    }
}
