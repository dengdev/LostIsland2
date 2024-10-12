using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, Isaveable {
    public Canvas mainCanvas;

    private Dictionary<string, bool> miniGameStateDict = new Dictionary<string, bool>();
    private GameController currentGame;
    private int gameWeek;

    private void OnEnable() {
        EventHandler.AfterSceneloadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.GamePassEvent += OnGamePassEvent;
        EventHandler.StarNewGameEvent += OnStarNewGameEvent;
    }

    private void OnDisable() {
        EventHandler.AfterSceneloadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.GamePassEvent -= OnGamePassEvent;
        EventHandler.StarNewGameEvent -= OnStarNewGameEvent;
    }

    private void OnStarNewGameEvent(int gameWeek) {
        this.gameWeek = gameWeek;
        miniGameStateDict.Clear();
    }

    private void Start() {
        StartCoroutine(LoadMenuScene());

        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);

        Isaveable saveable = this;
        saveable.SaveableRegister();

        MusicManager.Instance.PlayPaperWings();
    }

    private IEnumerator LoadMenuScene() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName.Menu.ToString(), LoadSceneMode.Additive);

        while (!asyncLoad.isDone) {
            yield return null;
        }

        if (mainCanvas != null) {
            mainCanvas.gameObject.SetActive(true);
        }
    }


    private void OnAfterSceneLoadedEvent() {

        foreach (MiniGame miniGame in FindObjectsOfType<MiniGame>()) {
            if (miniGameStateDict.TryGetValue(miniGame.gameName, out bool isPass)) {
                miniGame.isPass = isPass;
                miniGame.UpdateMiniGameState();
            }
        }
        currentGame = FindObjectOfType<GameController>();
        currentGame?.SetGameWeekData(gameWeek);
    }

    private void OnGamePassEvent(string gameName) {
        miniGameStateDict[gameName] = true;
    }

    public GameSaveData GeneratesaveData() {
        GameSaveData saveData = new GameSaveData();
        saveData.gameWeek = this.gameWeek;
        saveData.miniGameStateDict = this.miniGameStateDict;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData) {
        this.gameWeek = saveData.gameWeek;
        this.miniGameStateDict = saveData.miniGameStateDict;
    }
}
