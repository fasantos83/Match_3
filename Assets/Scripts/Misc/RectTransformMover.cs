using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectTransformMover : MonoBehaviour {

    public Vector3 startPosition;
    public Vector3 onscreenPosition;
    public Vector3 endPosition;

    public float timeToMove = 1f;

    RectTransform rectTransform;
    bool isMoving = false;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    void Move(Vector3 startPos, Vector3 endPos, float timeToMove) {
        if (!isMoving) {
            StartCoroutine(MoveRoutine(startPos, endPos, timeToMove));
        }
    }

    IEnumerator MoveRoutine(Vector3 startPos, Vector3 endPos, float timeToMove) {
        if (rectTransform != null) {
            rectTransform.anchoredPosition = startPos;
        }

        bool reachedDestination = false;
        float elapsedTime = 0f;
        isMoving = true;

        while (!reachedDestination) {
            if (Vector3.Distance(rectTransform.anchoredPosition, endPos) < 0.01f) {
                reachedDestination = true;
            } else {
                elapsedTime += Time.deltaTime;

                float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);
                t = t * t * t * (t * (t * 6 - 15) + 10);

                if (rectTransform != null) {
                    rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, t);
                }
                yield return null;
            }
        }

        isMoving = false;
    }

    public void MoveOn() {
        Move(startPosition, onscreenPosition, timeToMove);
    }

    public void MoveOff() {
        Move(onscreenPosition, endPosition, timeToMove);
    }
}
