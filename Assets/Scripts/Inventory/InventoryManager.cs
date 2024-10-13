using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>, Isaveable {
    public ItemDataList_SO itemData;

    public List<ItemName> itemList = new List<ItemName>();

    private void OnEnable() {
        EventHandler.UseItemEvent += OnUseItemEvent;
        EventHandler.ChangeItemEvent += OnChangeItemEvent;
        EventHandler.AfterSceneloadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.StarNewGameEvent += OnStarNewGameEvent;
    }

    private void OnDisable() {
        EventHandler.UseItemEvent -= OnUseItemEvent;
        EventHandler.ChangeItemEvent -= OnChangeItemEvent;
        EventHandler.AfterSceneloadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.StarNewGameEvent -= OnStarNewGameEvent;
    }

    private void Start() {
        Isaveable savaable = this;
        savaable.SaveableRegister();
    }

    private void OnStarNewGameEvent(int obj) {
        itemList.Clear();
    }

    private void OnAfterSceneLoadedEvent() {
        // 如果物品列表为空，更新UI，显示无物品状态
        if (itemList.Count == 0) {
            EventHandler.CallUpdateUIEvent(null, -1);
        } else {
            for (int i = 0; i < itemList.Count; i++) {
                EventHandler.CallUpdateUIEvent(itemData.GetItemDetails(itemList[i]), i);
            }
        }
    }

    private void OnChangeItemEvent(int index) {
        if (index >= 0 && index < itemList.Count) {
            ItemDetails item = itemData.GetItemDetails(itemList[index]);
            EventHandler.CallUpdateUIEvent(item, index);
        } else {
            EventHandler.CallUpdateUIEvent(null, -1); // 索引无效时清空UI
        }
    }

    private void OnUseItemEvent(ItemName itemName) {
        int index = GetItemIndex(itemName);
        itemList.RemoveAt(index);

        if (itemList.Count == 0) {
            EventHandler.CallUpdateUIEvent(null, -1); // 清空UI
        } else {
            int nextIndex = Mathf.Clamp(index, 0, itemList.Count - 1);
            EventHandler.CallUpdateUIEvent(itemData.GetItemDetails(itemList[nextIndex]), nextIndex);
        }
    }

    public void AddItem(ItemName itemName) {
        // 检查物品列表中是否已存在该物品
        if (!itemList.Contains(itemName)) {
            itemList.Add(itemName);
            EventHandler.CallUpdateUIEvent(itemData.GetItemDetails(itemName), itemList.Count - 1);
        }
    }

    private int GetItemIndex(ItemName itemName) {
        // 遍历物品列表，查找指定物品的索引
        for (int i = 0; i < itemList.Count; i++) {
            if (itemList[i] == itemName)
                return i;
        }
        return -1;
    }

    public GameSaveData GeneratesaveData() {
        GameSaveData saveData = new GameSaveData();
        saveData.itemList = this.itemList;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData) {
        this.itemList = saveData.itemList;
    }
}
