using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager> {

    public override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadLevel(string levelName) {
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevel(int levelIndex) {
        LoadLevel("lvl" + levelIndex);
    }

    public void LoadLevelSelect() {
        LoadLevel("LevelSelect");
    }

    public void LoadSettings() {
        LoadLevel("Settings");
    }

    public void QuitApplication() {
        Application.Quit();
    }


}
