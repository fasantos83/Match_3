using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ScoreMeter : MonoBehaviour {

    public Slider slider;
    public ScoreStar[] scoreStars = new ScoreStar[3];

    LevelGoal levelGoal;
    int maxScore = 0;

    void Awake() {
        slider = GetComponent<Slider>();
    }

    public void SetupStars(LevelGoal levelGoal) {
        if (levelGoal == null) {
            Debug.LogWarning("SCOREMETER Invalid level goal!");
            return;
        }

        this.levelGoal = levelGoal;
        int scoreGoalsLength = levelGoal.scoreGoals.Length;
        maxScore = levelGoal.scoreGoals[scoreGoalsLength - 1];
        float sliderWidth = slider.GetComponent<RectTransform>().rect.width;

        if (maxScore > 0) {
            for (int i = 0; i < scoreGoalsLength; i++) {
                if (scoreStars[i] != null) {
                    float newX = (sliderWidth * levelGoal.scoreGoals[i] / maxScore) - (sliderWidth / 2f);

                    RectTransform starRectTransform = scoreStars[i].GetComponent<RectTransform>();

                    if (starRectTransform != null) {
                        starRectTransform.anchoredPosition = new Vector2(newX, starRectTransform.anchoredPosition.y);
                    }
                }
            }
        }
    }

    public void UpdateScoreMeter(int score, int starCount) {
        if(levelGoal != null) {
            slider.value = (float)score / (float)maxScore;
        }

        for (int i = 0; i < starCount; i++) {
            if(scoreStars[i] != null) {
                scoreStars[i].Activate();
            }
        }
    }

}
