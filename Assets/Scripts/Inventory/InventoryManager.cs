using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>, Isaveable {
    public ItemDataList_SO itemData;

    [SerializeField] private List<ItemName> itemList = new List<ItemName>();

    private void OnEnable() {
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
        EventHandler.ChangeItemEvent += OnChangerItemEvent;
        EventHandler.AfterSceneloadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.StarNewGameEvent += OnStarNewGameEvent;
    }

    private void OnDisable() {
        EventHandler.ItemUsedEvent -= OnItemUsedEvent;
        EventHandler.ChangeItemEvent -= OnChangerItemEvent;
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

    private void OnChangerItemEvent(int index) {
        if (index >= 0 && index < itemList.Count) {
            ItemDetails item = itemData.GetItemDetails(itemList[index]);
            EventHandler.CallUpdateUIEvent(item, index);
        }
    }

    private void OnItemUsedEvent(ItemName itemName) {
        // 获取物品在列表中的索引
        int index = GetItemIndex(itemName);
        itemList.RemoveAt(index);

        //ToDo 暂时实现单一物品使用效果
        // 如果物品列表为空，调用UI更新事件，清空显示
        if (itemList.Count == 0)
            EventHandler.CallUpdateUIEvent(null, -1);
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
