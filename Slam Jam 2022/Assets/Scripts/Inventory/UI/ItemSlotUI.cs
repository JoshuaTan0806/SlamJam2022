using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

public class ItemSlotUI : MonoBehaviour
{
    public Vector2Int slot = Vector2Int.zero;

    public ItemUI prefab = null;

    private ItemData item = null;
    private ItemUI ui = null;

    public void GetSlottedItem()
    {
        item = ItemInventory.GetItem(slot);
        //Spawn UI if we don't have any
        if (!ui)
        {
            ui = Instantiate(prefab.gameObject, transform).GetComponent<ItemUI>();
            ui.item = item;
        }

        RefreshUI();
    }

    public void RefreshUI()
    {   //Position it on us
        ui.transform.SetParent(transform);
        ui.transform.localPosition = Vector3.zero;

        ui.RefreshUI();
    }

    public void SetSlot(ItemUI ui)
    {
        item = ui.item;
        this.ui = ui;

        ItemInventory.EquipItem(item, slot);
    }

    public void SetSlot(ItemData i)
    {
        item = i;

        if (item == null)
            ui = null;
    }
}
