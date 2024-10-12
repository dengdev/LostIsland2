using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour, Isaveable {
    private Dictionary<ItemName, bool> itemAvailableDict = new Dictionary<ItemName, bool>();
    private Dictionary<string, bool> interactiveStateDict = new Dictionary<string, bool>();

    private void OnEnable() {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneloadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
        EventHandler.StarNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable() {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneloadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
        EventHandler.StarNewGameEvent -= OnStartNewGameEvent;
    }

    private void Start() {
        Isaveable saveable = this;
        saveable.SaveableRegister();
    }

    private void OnStartNewGameEvent(int obj) {
        itemAvailableDict.Clear();
        interactiveStateDict.Clear();
    }

    private void OnBeforeSceneUnloadEvent() {

        foreach (Item item in FindObjectsOfType<Item>()) {
            if (!itemAvailableDict.ContainsKey(item.itemName)) {
                itemAvailableDict.Add(item.itemName, true);
            }
        }

        foreach (Interactive interactive in FindObjectsOfType<Interactive>()) {
            if (interactiveStateDict.ContainsKey(interactive.name))
                interactiveStateDict[interactive.name] = interactive.isDone;
            else
                interactiveStateDict.Add(interactive.name, interactive.isDone);
        }
    }

    private void OnAfterSceneLoadedEvent() {

        foreach (Item item in FindObjectsOfType<Item>()) {
            if (!itemAvailableDict.ContainsKey(item.itemName)) {
                itemAvailableDict.Add(item.itemName, true);
            } else
                item.gameObject.SetActive(itemAvailableDict[item.itemName]);
        }

        foreach (Interactive interactive in FindObjectsOfType<Interactive>()) {
            if (interactiveStateDict.ContainsKey(interactive.name))
                interactive.isDone = interactiveStateDict[interactive.name];
            else
                interactiveStateDict.Add(interactive.name, interactive.isDone);
        }
    }

    // 这方法只在拾取场景物品时更新，拾取代表着场景物品变成false
    private void OnUpdateUIEvent(ItemDetails itemdetails, int arg2) {
        if (itemdetails != null) {
            itemAvailableDict[itemdetails.itemName] = false;
        }
    }

    public GameSaveData GeneratesaveData() {
        GameSaveData saveData = new GameSaveData();
        saveData.itemAvailableDict = this.itemAvailableDict;
        saveData.interactiveStateDict = this.interactiveStateDict;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData) {
        this.itemAvailableDict = saveData.itemAvailableDict;
        this.interactiveStateDict = saveData.interactiveStateDict;
    }
}
