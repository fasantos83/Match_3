﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text timeLeftText;
    public Image clockImage;

    public bool paused = false;
    public int flashTimeLimit = 10;
    public AudioClip flashBeep;
    public float flashInterval = 1f;
    public Color flashColor = Color.red;

    int maxTime = 60;
    IEnumerator flashRoutine = null;

    public void InitTimer(int time = 60) {
        maxTime = time;

        if(clockImage != null) {
            clockImage.type = Image.Type.Filled;
            clockImage.fillMethod = Image.FillMethod.Radial360;
            clockImage.fillOrigin = (int) Image.Origin360.Top;
        }

        if(timeLeftText != null) {
            timeLeftText.text = maxTime.ToString();
        }
    }

    public void UpdateTimer(int currentTime) {
        if (!paused) {
            if (clockImage != null) {
                clockImage.fillAmount = (float) currentTime / (float) maxTime;

                if(currentTime <= flashTimeLimit) {
                    flashRoutine = FlashRoutine(clockImage, flashColor, flashInterval);
                    StartCoroutine(flashRoutine);

                    if(flashBeep != null) {
                        SoundManager.Instance.PlayClipAtPoint(flashBeep, Vector3.zero, SoundManager.Instance.fxVolume, false, false);
                    }
                }
            }

            if (timeLeftText != null) {
                timeLeftText.text = currentTime.ToString();
            }
        }
    }

    IEnumerator FlashRoutine(Image image, Color targetColor, float interval) {
        if(image != null) {
            Color originalColor = image.color;
            image.CrossFadeColor(targetColor, interval * 0.3f, true, true);
            yield return new WaitForSeconds(interval * 0.5f);

            image.CrossFadeColor(originalColor, interval * 0.3f, true, true);
            yield return new WaitForSeconds(interval * 0.5f);
        }
    }

    public void FadeOff() {
        if (flashRoutine != null) {
            StopCoroutine(flashRoutine);
        }

        ScreenFader[] screenFaders = GetComponentsInChildren<ScreenFader>();

        foreach (ScreenFader screenFader in screenFaders) {
            screenFader.FadeOff();
        }
    }
}
