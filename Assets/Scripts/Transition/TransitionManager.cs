using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>, Isaveable {
    public SceneName dafultScene;
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;
    public SceneName menuName = SceneName.Menu;

    private bool isFade;
    private bool canTransition;

    private void OnEnable() {
        EventHandler.GameStateChangeEvent += OnGameStateChangeEvent;
        EventHandler.StarNewGameEvent += OnStarNewGameEvent;
    }

    private void OnDisable() {
        EventHandler.GameStateChangeEvent -= OnGameStateChangeEvent;
        EventHandler.StarNewGameEvent -= OnStarNewGameEvent;
    }

    private void OnStarNewGameEvent(int obj) {
        StartCoroutine(TransitionToScene(menuName, dafultScene));
    }

    private void Start() {
        Isaveable saveable = this;
        saveable.SaveableRegister();
    }

    private void OnGameStateChangeEvent(GameState gameState) {
        canTransition = gameState == GameState.GamePlay;
    }

    public void Transition(SceneName sourceScene, SceneName tagetScene) {
        if (!isFade && canTransition)
            StartCoroutine(TransitionToScene(sourceScene, tagetScene));
    }

    private IEnumerator TransitionToScene(SceneName sourceScene, SceneName tagetScene) {
        yield return Fade(1);
        //StartCoroutine(Fade(1)); // 让它们同时执行
        EventHandler.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(sourceScene.ToString());
        yield return SceneManager.LoadSceneAsync(tagetScene.ToString(), LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
        EventHandler.CallAfterSceneLoadedEvent();
        yield return Fade(0);
    }

    private IEnumerator Fade(float targetAlpha) {
        isFade = true;
        fadeCanvasGroup.blocksRaycasts = true;

        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha)) {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        fadeCanvasGroup.blocksRaycasts = false;
        isFade = false;
    }

    public GameSaveData GeneratesaveData() {
        GameSaveData saveData = new GameSaveData();
        saveData.currentScene = (SceneName)System.Enum.Parse(typeof(SceneName), SceneManager.GetActiveScene().name);
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData) {
        Transition(menuName, saveData.currentScene);
    }
}
