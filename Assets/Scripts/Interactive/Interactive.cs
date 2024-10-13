using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Interactive : MonoBehaviour {
    public ItemName requireItem;
    public bool isDone;

    protected float popupDuration = 2.2f;
    private float upOffset = 160f;

    public void CheckItem(ItemName itemname) {
        if (requireItem == itemname && !isDone) {
            isDone = true;
            // ʹ�ò��Ƴ������е���Ʒ
            OnclickedAction();
            EventHandler.CallUseItemEvent(itemname);
        } else if (requireItem != itemname) {
            ShowPopup("�����Ʒ����ʹ�������");
        }
    }

    protected virtual void OnclickedAction() {

    }

    public virtual void EmptyClicked() {
    }

    protected void ShowPopup(string message) {
        GameObject popup = Resources.Load<GameObject>("Prefabs/Tip");
        if (popup == null) {
            Debug.LogError("�޷������ı���Ԥ���壬·��Ϊ��");
            return;
        }

        GameObject popupInstance = Instantiate(popup, Vector3.zero, Quaternion.identity);
        Text popupText = popupInstance.GetComponentInChildren<Text>();
        popupText.text = message;

        popupInstance.transform.SetParent(GameObject.Find("Main Canvas").transform, false);
        RectTransform rectTransform = popupInstance.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;

        popupInstance.GetComponent<CanvasGroup>().alpha = 1f;

        rectTransform.DOLocalMoveY(rectTransform.localPosition.y + upOffset, popupDuration)
            .OnStart(() => popupInstance.GetComponent<CanvasGroup>().alpha = 1)
            .OnUpdate(() => {
                float elapsed = rectTransform.localPosition.y / upOffset;
                popupInstance.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, elapsed);
            })
            .OnComplete(() => Destroy(popupInstance));
    }
}
