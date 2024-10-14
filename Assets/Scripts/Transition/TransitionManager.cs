using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>, Isaveable {
    public SceneName startScene = SceneName.Transition;
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;
    public SceneName menuName = SceneName.Menu;

    private bool isFading;
    private bool canTransition;

    private void OnEnable() {
        EventHandler.GameStateChangeEvent += OnGameStateChangeEvent;
        EventHandler.StarNewGameEvent += OnStarNewGameEvent;
    }

    private void Start() {
        Isaveable saveable = this;
        saveable.SaveableRegister();
    }

    private void OnDisable() {
        EventHandler.GameStateChangeEvent -= OnGameStateChangeEvent;
        EventHandler.StarNewGameEvent -= OnStarNewGameEvent;
    }

    private void OnStarNewGameEvent(int obj) {
        StartCoroutine(TransitionToScene(menuName, startScene));
    }

    private void OnGameStateChangeEvent(GameState gameState) {
        canTransition = gameState == GameState.GamePlay;
    }

    public void Transition(SceneName sourceScene, SceneName targetScene) {
        if (!isFading && canTransition)
            StartCoroutine(TransitionToScene(sourceScene, targetScene));

        if (sourceScene == SceneName.Transition|| sourceScene == SceneName.Menu) {
            if (GameManager.Instance.mainCanvas != null) {

                CanvasGroup canvasGroup = GameManager.Instance.mainCanvas.GetComponent<CanvasGroup>();

                if (canvasGroup == null) {
                    canvasGroup = GameManager.Instance.mainCanvas.gameObject.AddComponent<CanvasGroup>();
                }
                canvasGroup.alpha = 0;
                GameManager.Instance.mainCanvas.gameObject.SetActive(true);
                canvasGroup.DOFade(1, 0.9f).From(0);
            }
        }
    }

    private IEnumerator TransitionToScene(SceneName sourceScene, SceneName tagetScene) {
        yield return Fade(1);
        EventHandler.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(sourceScene.ToString());
        yield return SceneManager.LoadSceneAsync(tagetScene.ToString(), LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
        EventHandler.CallAfterSceneLoadedEvent();
        yield return Fade(0);
    }

    private IEnumerator Fade(float targetAlpha) {
        isFading = true;
        fadeCanvasGroup.blocksRaycasts = true;

        /*平滑且稳定的效果
        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha)) {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
         */
        //  更简单的时间控制
        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration) {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = targetAlpha;
        fadeCanvasGroup.blocksRaycasts = false;
        isFading = false;
    }

    public GameSaveData GenerateSaveData() {
        GameSaveData saveData = new GameSaveData();
        //saveData.currentScene = (SceneName)System.Enum.Parse(typeof(SceneName), SceneManager.GetActiveScene().name);
        if (Enum.TryParse(SceneManager.GetActiveScene().name, out SceneName currentScene)) {
            saveData.currentScene = currentScene;
        }
        return saveData;
    }

    public void RestoreSavedGameData(GameSaveData saveData) {
        Transition(menuName, saveData.currentScene);
    }
}
