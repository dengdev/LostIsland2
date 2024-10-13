using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour {
    public Text itemNameText;

    public void updateItemname(ItemName itemName) {
        itemNameText.text = itemName switch {
            ItemName.Key => "邮箱钥匙",
            ItemName.Ticket => "一张船票",
            ItemName.Prinogem=>"祈愿石",
            ItemName.DamagedMask=>"破面具",
            ItemName.CorLapis=>"琥珀",
            _ => "默认"
        };
    }
}
