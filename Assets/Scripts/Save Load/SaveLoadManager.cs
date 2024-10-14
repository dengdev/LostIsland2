using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveLoadManager : Singleton<SaveLoadManager> {
    private string jsonFloder;
    private List<Isaveable> saveableList = new List<Isaveable>();
    private Dictionary<string, GameSaveData> saveableDict = new Dictionary<string, GameSaveData>();

    protected override void Awake() {
        base.Awake();
        jsonFloder = Application.persistentDataPath + "/SAVE/";
    }

    private void OnEnable() {
        EventHandler.StarNewGameEvent += OnStarNewGameEvent;
    }

    private void OnDisable() {
        EventHandler.StarNewGameEvent -= OnStarNewGameEvent;
    }

    private void OnStarNewGameEvent(int obj) {
        string resultPath = jsonFloder + "data.sav";
        if (File.Exists(resultPath)) {
            File.Delete(resultPath);
        }
    }

    public void Register(Isaveable saveable) {
        saveableList.Add(saveable);
    }

    public void Save() {
        saveableDict.Clear();

        foreach (var saveable in saveableList) {
            saveableDict.Add(saveable.GetType().Name, saveable.GenerateSaveData());
        }

        string resultPath = jsonFloder + "data.sav";
        string jsonData = JsonConvert.SerializeObject(saveableDict, Formatting.Indented);

        if (!File.Exists(resultPath)) {
            Directory.CreateDirectory(jsonFloder);
        }

        File.WriteAllText(resultPath, jsonData);
        Debug.Log(resultPath);
    }

    public void Load() {
        string resultPath = jsonFloder + "data.sav";

        if (!File.Exists(resultPath)) return;

        string stringData = File.ReadAllText(resultPath);
        Dictionary<string, GameSaveData> jsonData = JsonConvert.DeserializeObject<Dictionary<string, GameSaveData>>(stringData);

        foreach (Isaveable saveable in saveableList) {
            saveable.RestoreSavedGameData(jsonData[saveable.GetType().Name]);
        }
    }
}
