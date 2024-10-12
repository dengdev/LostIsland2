using UnityEngine;

public class Interactive : MonoBehaviour {
    public ItemName requireItem;
    public bool isDone; 

    public void CheckItem(ItemName itemname) {
        if (requireItem == itemname && !isDone) {
            isDone = true;
            // 使用并移除背包中的物品
            OnclickedAction();
            EventHandler.CallItemUsedEvent(itemname);
        }
    }

    /// <summary>
    /// 默认是正确的物品执行
    /// </summary>
    protected virtual void OnclickedAction() {

    }

    public virtual void EmptyClicked() {
        Debug.Log("你没有拿起该对象需要的物品");
    }
}
