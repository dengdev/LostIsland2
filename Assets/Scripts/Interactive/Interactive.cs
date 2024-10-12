using UnityEngine;

public class Interactive : MonoBehaviour {
    public ItemName requireItem;
    public bool isDone; 

    public void CheckItem(ItemName itemname) {
        if (requireItem == itemname && !isDone) {
            isDone = true;
            // ʹ�ò��Ƴ������е���Ʒ
            OnclickedAction();
            EventHandler.CallItemUsedEvent(itemname);
        }
    }

    /// <summary>
    /// Ĭ������ȷ����Ʒִ��
    /// </summary>
    protected virtual void OnclickedAction() {

    }

    public virtual void EmptyClicked() {
        Debug.Log("��û������ö�����Ҫ����Ʒ");
    }
}
