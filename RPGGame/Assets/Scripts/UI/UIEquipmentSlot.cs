using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEquipmentSlot : UIItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name="Equipment slot " +slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Inventory.instance.UnequipedItem(item.data as ItemData_Equippment);
        Inventory.instance.AddItem(item.data as ItemData_Equippment);
        CleanUpSlot();
    }
}
