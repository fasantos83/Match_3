﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mgl;

public class UIManager : Singleton<UIManager> {

    public GameObject collectionGoalLayout;
    public int collectionGoalBaseWidth = 125;

    public Text levelNameText;
    public Text movesLeftText;
    
    public ScreenFader screenFader;
    public MessageWindow messageWindow;
    public ScoreMeter scoreMeter;

    public GameObject movesCounter;
    public Timer timer;

    public override void Awake() {
        base.Awake();

        if (messageWindow != null) {
            messageWindow.gameObject.SetActive(true);
        }

        if (screenFader != null) {
            screenFader.gameObject.SetActive(true);
        }
    }

    public void SetupCollectionGoalLayout(CollectionGoal[] collectionGoals, GameObject goalLayout, int spacingWidth) {
        if (goalLayout != null && collectionGoals != null && collectionGoals.Length > 0) {
            RectTransform rectTransform = goalLayout.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(collectionGoals.Length * spacingWidth, rectTransform.sizeDelta.y);

            CollectionGoalPanel[] panels = goalLayout.gameObject.GetComponentsInChildren<CollectionGoalPanel>();
            for (int i = 0; i < panels.Length; i++) {
                if (i < collectionGoals.Length && collectionGoals[i] != null) {
                    panels[i].gameObject.SetActive(true);
                    panels[i].collectionGoal = collectionGoals[i];
                    panels[i].SetupPanel();
                } else {
                    panels[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetupCollectionGoalLayout(CollectionGoal[] collectionGoals) {
        SetupCollectionGoalLayout(collectionGoals, collectionGoalLayout, collectionGoalBaseWidth);
    }

    public void UpdateCollectionGoalLayout(GameObject goalLayout) {
        if(goalLayout != null) {
            CollectionGoalPanel[] panels = goalLayout.GetComponentsInChildren<CollectionGoalPanel>();

            if(panels != null && panels.Length != 0) {
                UpdateCollectionGoalLayout(panels);
            }
        }
    }

    public void UpdateCollectionGoalLayout() {
        UpdateCollectionGoalLayout(collectionGoalLayout);
    }

    public void UpdateCollectionGoalLayout(CollectionGoalPanel[] panels) {
        foreach (CollectionGoalPanel panel in panels) {
            if (panel != null && panel.gameObject.activeInHierarchy) {
                panel.UpdatePanel();
            }
        }
    }

    public void EnableTimer(bool state) {
        if (timer != null) {
            timer.gameObject.SetActive(state);
        }
    }

    public void EnableMovesCounter(bool state) {
        if (movesCounter != null) {
            movesCounter.SetActive(state);
        }
    }

    public void EnableCollectionGoalLayout(bool state) {
        if (collectionGoalLayout != null) {
            collectionGoalLayout.gameObject.SetActive(state);
        }
    }

}
