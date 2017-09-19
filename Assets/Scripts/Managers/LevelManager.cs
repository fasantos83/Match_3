using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager> {

    public GameObject levelButtonPrefab;

    string filePath;
    string jsonString;
    LevelList levelList;
    Level currentLevel;
    public Level CurrentLevel {
        get { return currentLevel; }
        set { currentLevel = value; }
    }

    public override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        StartCoroutine(Init());
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    IEnumerator Init() {
        yield return StartCoroutine(ReadLevelFromJSON());
        SetupLevelSelection();
    }

    IEnumerator ReadLevelFromJSON() {
        filePath = Path.Combine(Application.streamingAssetsPath, "Levels.json");
        if (filePath.Contains("://")) {
            WWW www = new WWW(filePath);
            yield return www;
            jsonString = www.text;
        } else {
            jsonString = File.ReadAllText(filePath);
        }
        levelList = JsonUtility.FromJson<LevelList>(jsonString);
    }

    public void SetupLevelSelection() {
        if (LevelLayout.Instance != null && levelButtonPrefab != null && levelList != null) {
            foreach (Level level in levelList.levels) {
                GameObject levelButtonGO = Instantiate(levelButtonPrefab, LevelLayout.Instance.transform) as GameObject;

                Button button = levelButtonGO.GetComponent<Button>();
                Text levelButtonText = button.GetComponentInChildren<Text>();
                levelButtonText.text = level.index.ToString();
                button.onClick.AddListener(delegate {
                    MenuManager.Instance.LoadLevel(level.sceneName);
                    currentLevel = level;
                });
            }
        }
    }

    public Level GetLevel(int index) {
        Level level = null;
        if (levelList != null) {
            int i = 0;
            while (i < levelList.levels.Length && level == null) {
                if (levelList.levels[i].index == index) {
                    level = levelList.levels[i];
                }
                i++;
            }
        }
        return level;
    }

    public string SetupNextLevel() {
        string nextScene = "LevelSelect";
        int nextLevelIndex = currentLevel.nextLevelIndex;
        if(nextLevelIndex != -1) {
            currentLevel = GetLevel(nextLevelIndex);
            nextScene = currentLevel.sceneName;
        }
        return nextScene;
    }
}



