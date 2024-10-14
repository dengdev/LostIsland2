using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    public Button leftButton, rightButton;
    public SlotUI slotUI;
    public int currentIndex;

    private void OnEnable() {
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
    }

    private void OnDisable() {
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
    }

    private void OnUpdateUIEvent(ItemDetails itemDetails, int index) {

        if (itemDetails == null) {
            slotUI.SetEmpty();
            currentIndex = -1;

            leftButton.interactable = false;
            rightButton.interactable = false;
        } else {
            currentIndex = index;
            slotUI.SetItem(itemDetails);

            leftButton.interactable = index > 0;
            rightButton.interactable = index < InventoryManager.Instance.itemList.Count - 1; // 防止越界
        }
    }

    /// <summary>
    /// 左右按钮Event事件
    /// </summary>
    /// <param name="amout"></param>
    public void SwitchItem(int amout) {
        int index = currentIndex + amout;

        if (index < 0) {
            index = 0;
        } else if (index >= InventoryManager.Instance.itemList.Count) {
            index = InventoryManager.Instance.itemList.Count - 1;

        }

        leftButton.interactable = index > 0;
        rightButton.interactable = index < InventoryManager.Instance.itemList.Count - 1;

        EventHandler.CallChangeItemEvent(index);
    }
}
