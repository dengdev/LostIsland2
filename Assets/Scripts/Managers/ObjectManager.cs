using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>, Isaveable {
    private Dictionary<ItemName, bool> availableItems = new Dictionary<ItemName, bool>();
    private Dictionary<string, bool> interactiveObjectStates = new Dictionary<string, bool>();

    private void OnEnable() {
        EventHandler.BeforeSceneUnloadEvent += HandleBeforeSceneUnload;
        EventHandler.AfterSceneloadedEvent += HandleAfterSceneLoaded;
        EventHandler.UpdateUIEvent += HandleUpdateUIEvent;
        EventHandler.StarNewGameEvent += HandleStartNewGameEvent;
    }

    private void OnDisable() {
        EventHandler.BeforeSceneUnloadEvent -= HandleBeforeSceneUnload;
        EventHandler.AfterSceneloadedEvent -= HandleAfterSceneLoaded;
        EventHandler.UpdateUIEvent -= HandleUpdateUIEvent;
        EventHandler.StarNewGameEvent -= HandleStartNewGameEvent;
    }

    private void Start() {
        Isaveable saveable = this;
        saveable.SaveableRegister();
    }

    private void HandleStartNewGameEvent(int obj) {
        availableItems.Clear();
        interactiveObjectStates.Clear();
    }

    /// <summary>
    /// 交互后及时更新,而不是等到场景切换
    /// </summary>
    /// <param name="interactiveName"></param>
    /// <param name="isDone"></param>
    public void UpdateInteractiveState(string interactiveName, bool isDone) {
        if (interactiveObjectStates.ContainsKey(interactiveName)) {
            interactiveObjectStates[interactiveName] = isDone;
        } else {
            interactiveObjectStates.Add(interactiveName, isDone);
        }
    }

    /// <summary>
    /// 收集当前的物品和交互状态
    /// </summary>
    private void HandleBeforeSceneUnload() {

        foreach (Item item in FindObjectsOfType<Item>()) {
            if (!availableItems.ContainsKey(item.itemName)) {
                availableItems.Add(item.itemName, true);
            }
        }

        foreach (Interactive interactive in FindObjectsOfType<Interactive>()) {
            if (interactiveObjectStates.ContainsKey(interactive.name))
                interactiveObjectStates[interactive.name] = interactive.isDone;
            else
                interactiveObjectStates.Add(interactive.name, interactive.isDone);
        }
    }

    /// <summary>
    /// 恢复物品和交互对象的状态
    /// </summary>
    private void HandleAfterSceneLoaded() {

        foreach (Item item in FindObjectsOfType<Item>()) {
            if (!availableItems.ContainsKey(item.itemName)) {
                availableItems.Add(item.itemName, true);
            } else
                item.gameObject.SetActive(availableItems[item.itemName]);
        }

        foreach (Interactive interactive in FindObjectsOfType<Interactive>()) {
            if (interactiveObjectStates.ContainsKey(interactive.name))
                interactive.isDone = interactiveObjectStates[interactive.name];
            else
                interactiveObjectStates.Add(interactive.name, interactive.isDone);
        }
    }

    // 这方法只在拾取场景物品时更新，拾取代表着场景物品变成false
    private void HandleUpdateUIEvent(ItemDetails itemdetails, int arg2) {
        if (itemdetails != null) {
            availableItems[itemdetails.itemName] = false;
        }
    }

    public GameSaveData GenerateSaveData() {
        GameSaveData saveData = new GameSaveData();
        saveData.itemAvailableDict = this.availableItems;
        saveData.interactiveStateDict = this.interactiveObjectStates;
        return saveData;
    }

    public void RestoreSavedGameData(GameSaveData saveData) {
        this.availableItems = saveData.itemAvailableDict;
        this.interactiveObjectStates = saveData.interactiveStateDict;
    }
}
