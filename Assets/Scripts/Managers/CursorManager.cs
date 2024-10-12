using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour {
    public RectTransform hand;

    private Vector3 MouseWorldPos => Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    private ItemName currentItem;
    private bool canClick;
    private bool holdItem;

    private void OnEnable() {
        EventHandler.ItemSelectedEvent += OnItemSelectedevent;
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
    }

    private void OnDisable() {
        EventHandler.ItemSelectedEvent -= OnItemSelectedevent;
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
    }

    private void Update() {
        canClick = ObjectAtMousePosition();

        if (hand.gameObject.activeInHierarchy) {
            hand.position = Input.mousePosition;
        }

        if (InteractWithUI()) return;

        if (canClick && Input.GetMouseButtonDown(0)) {
            ClickAction(ObjectAtMousePosition().gameObject);
        }
    }

    private void OnItemUsedEvent(ItemName name) {
        currentItem = ItemName.None;
        holdItem = false;
        hand.gameObject.SetActive(false);
    }

    private void OnItemSelectedevent(ItemDetails itemDetails, bool isSelected) {
        holdItem = isSelected;

        if (isSelected) {
            currentItem = itemDetails.itemName;
        }
        hand.gameObject.SetActive(holdItem);
    }

    private void ClickAction(GameObject clickObject) {
        switch (clickObject.tag) {
            case "Teleport":
                Teleport teleport = clickObject.GetComponent<Teleport>();
                teleport?.TeleportToScene();
                break;
            case "Item":
                Item item = clickObject.GetComponent<Item>();
                item?.ItemClicked();
                break;
            case "Interactive":
                Interactive interactive = clickObject.GetComponent<Interactive>();
                if (holdItem)
                    interactive?.CheckItem(currentItem);
                else
                    interactive?.EmptyClicked();
                break;
        }
    }


    /// <summary>
    ///  ¼ì²âÊó±êµã»÷·¶Î§µÄÅö×²Ìå
    /// </summary>
    private Collider2D ObjectAtMousePosition() {
        return Physics2D.OverlapPoint(MouseWorldPos);
    }

    /// <summary>
    ///  ÅÐ¶ÏÊÇ·ñ¸úUI»¥¶¯
    /// </summary>
    private bool InteractWithUI() {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return true;
        return false;
    }
}
