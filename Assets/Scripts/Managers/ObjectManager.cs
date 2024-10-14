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
    /// ������ʱ����,�����ǵȵ������л�
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
    /// �ռ���ǰ����Ʒ�ͽ���״̬
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
    /// �ָ���Ʒ�ͽ��������״̬
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

    // �ⷽ��ֻ��ʰȡ������Ʒʱ���£�ʰȡ�����ų�����Ʒ���false
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
