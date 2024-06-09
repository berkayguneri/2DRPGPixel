using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIItemSlot : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;


    protected virtual void Start()
    {
        ui=GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.itemIcon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);

            AudioManager.instance.PlaySFX(7, null);
            return;
            
        }

        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
            AudioManager.instance.PlaySFX(7, null);
            //Debug.Log("Equipped new item" + item.data.itemName);
        }

        ui.itemToolTip.HideToolTip();
        //ui.shopItemToolTip.HideShopItemToolTip();

        

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null) return;
      
        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equippment);
        //ui.shopItemToolTip.ShowShopItemToolTip(item.data as ItemData_Equippment);

        Debug.Log("Show item info");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;

        ui.itemToolTip.HideToolTip();
        //ui.shopItemToolTip.HideShopItemToolTip();
        Debug.Log("Hide item info");
    }

   
}
