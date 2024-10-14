using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : Singleton<CursorManager> {
    public Texture2D defaultCursor;
    public Texture2D clickableCursor;
    public Texture2D handCursor;

    public Vector2 cursorHotspot = Vector2.zero;

    private ItemName currentItem;
    private bool holdItem;

    private void OnEnable() {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.UseItemEvent += OnUseItemEvent;
    }

    private void OnDisable() {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.UseItemEvent -= OnUseItemEvent;
    }

    private void Update() {

        if (Input.GetMouseButtonDown(1) && holdItem) {
            MusicManager.Instance.PlayConcelSound();
            EventHandler.CallRetunItemEvent(currentItem);
            CancelItemSelection();
            return; 
        }

        if (InteractWithUI()) {
            SetCursor(defaultCursor);
            return;
        }

        Collider2D hitObject = ObjectAtMousePosition();
        SetCursor(holdItem ? handCursor :GetCursorBasedOnTag(hitObject) );

        if (hitObject && Input.GetMouseButtonDown(0)) {
            ClickAction(hitObject.gameObject);
        }
    }
    private void CancelItemSelection() {
        holdItem = false; 
        currentItem = ItemName.None;
        SetCursor(defaultCursor);
    }

    private void OnUseItemEvent(ItemName name) {
        CancelItemSelection();
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected) {
        holdItem = isSelected;
        currentItem = isSelected ? itemDetails.itemName : ItemName.None;
        SetCursor(holdItem ? handCursor : defaultCursor);
    }

    private Texture2D GetCursorBasedOnTag(Collider2D hitObject) {
        if (hitObject == null) return defaultCursor;
        switch (hitObject.tag) {
            case "Teleport":
            case "Item":
                return clickableCursor;
            case "Interactive":
                return holdItem ? handCursor : clickableCursor;
            default:
                return defaultCursor;
        }
    }

    private void ClickAction(GameObject clickObject) {
        switch (clickObject.tag) {
            case "Teleport":
                if (!holdItem) {
                    Teleport teleport = clickObject.GetComponent<Teleport>();
                    teleport?.TeleportToScene();
                }
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

    private Collider2D ObjectAtMousePosition() {
        return Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private bool InteractWithUI() {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private void SetCursor(Texture2D cursorType) {
        Cursor.SetCursor(cursorType, cursorHotspot, CursorMode.Auto);
    }

}
