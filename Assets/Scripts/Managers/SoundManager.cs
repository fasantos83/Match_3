﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {

    public AudioClip[] musicClips;
    public AudioClip[] winClips;
    public AudioClip[] loseClips;
    public AudioClip[] bonusClips;

    [Range(0, 1)]
    public float musicVolume = 0.5f;

    [Range(0, 1)]
    public float fxVolume = 0.5f;

    public float lowPitch = 0.95f;
    public float highPitch = 1.05f;

    public override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("SoundManager Awake");
    }

    void Start() {
        Debug.Log("SoundManager Start");
        PlayMusic();
    }

    public AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1f, bool loop = false, bool randomizedPitch = true) {
        AudioSource source = null;

        if (clip != null) {
            GameObject go = new GameObject("SoundFX" + clip.name);
            go.transform.position = position;

            source = go.AddComponent<AudioSource>();
            source.clip = clip;

            if (randomizedPitch) {
                float randomPitch = Random.Range(lowPitch, highPitch);
                source.pitch = randomPitch;
            }

            source.volume = volume;

            source.loop = loop;

            source.Play();

            if (!loop) {
                Destroy(go, clip.length);
            }
        }

        return source;
    }

    public AudioSource PlayRandom(AudioClip[] clips, Vector3 position, float volume = 1f, bool loop = false) {
        AudioSource source = null;

        if (clips != null && clips.Length > 0) {
            int randomIndex = Random.Range(0, clips.Length);
            if(clips[randomIndex] != null) {
                source = PlayClipAtPoint(clips[randomIndex], position, volume, loop);
                source.transform.parent = this.transform;
            }
        }

        return source;
    }

    public void PlayMusic() {
        PlayRandom(musicClips, Vector3.zero, musicVolume, true);
    }

    public void PlayWinSound() {
        PlayRandom(winClips, Vector3.zero, fxVolume);
    }

    public void PlayLoseSound() {
        PlayRandom(loseClips, Vector3.zero, fxVolume * 0.5f);
    }

    public void PlayBonusSound() {
        PlayRandom(bonusClips, Vector3.zero, fxVolume);
    }
}
