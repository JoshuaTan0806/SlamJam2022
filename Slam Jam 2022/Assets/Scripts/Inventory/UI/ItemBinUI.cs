using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.UI
{
    /// <summary>
    /// Deletes items that are dragged onto it
    /// </summary>
    public class ItemBinUI : ItemSlotUI
    {
        public override void SetSlot(ItemUI ui)
        {
            if (!ui || !ui.item)
                return;
            //Make sure unequipped
            ItemInventory.UnequipItem(ui.item);

            //Delete
            Destroy(ui.item);
            Destroy(ui);
        }
    }
}