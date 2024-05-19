using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftSlot : UIItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equippment craftData=item.data as ItemData_Equippment;
        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
    }
}
