using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour {
    public Text itemNameText;

    public void updateItemname(ItemName itemName) {
        itemNameText.text = itemName switch {
            ItemName.Key => "����Կ��",
            ItemName.Ticket => "һ�Ŵ�Ʊ",
            ItemName.Prinogem=>"��Ըʯ",
            ItemName.DamagedMask=>"�����",
            ItemName.CorLapis=>"����",
            _ => "Ĭ��"
        };
    }
}
