using UnityEngine;

public class Item : MonoBehaviour {
    public ItemName itemName;

    public void ItemClicked() {
        InventoryManager.Instance.AddItem(itemName);
        this.gameObject.SetActive(false);
    }
}
