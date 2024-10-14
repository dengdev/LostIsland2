using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public Image itemImage;
    public ItemTooltip tooltip;

    public Image background; 

    private ItemDetails currentItem;
    private bool isSelected;

    private void OnEnable() {
        EventHandler.UseItemEvent += EventHandler_UseItemEvent;
        EventHandler.RetunItemEvent += EventHandler_UseItemEvent;

    }

    private void OnDisable() {
        EventHandler.UseItemEvent -= EventHandler_UseItemEvent;
        EventHandler.RetunItemEvent -= EventHandler_UseItemEvent;
    }

    private void EventHandler_UseItemEvent(ItemName obj) {
        isSelected=false;
        background.color = Color.white;
    }

    public void SetItem(ItemDetails itemDetails) {
        if (itemDetails != null) {
            currentItem = itemDetails;
            this.gameObject.SetActive(true);
            itemImage.sprite = itemDetails.itemSprite;
            itemImage.SetNativeSize();
        } else {
            SetEmpty(); 
        }
    }

    public void SetEmpty() {
        currentItem = null;
        this.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        isSelected = !isSelected;
        background.color = isSelected ? Color.yellow : Color.white;

        EventHandler.CallItemSelectedEvent(currentItem, isSelected);
        if (isSelected) {
            MusicManager.Instance.PlayConfirmSound();
        } else {
            MusicManager.Instance.PlayConcelSound();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (this.gameObject.activeInHierarchy) {
            tooltip.gameObject.SetActive(true);
            tooltip.updateItemname(currentItem.itemName);
            itemImage.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltip.gameObject.SetActive(false);
        itemImage.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }
}
