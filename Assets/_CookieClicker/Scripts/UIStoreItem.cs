using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIStoreItem : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameTag;
    public TMP_Text priceTag;

    public void Bind(ShopItem item, Sprite sprite)
    {
        nameTag.text = item.name;
        priceTag.text = item.price;
        icon.sprite = sprite;
    }
}
