using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Items.UI
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class InventoryUIMaster : MonoBehaviour
    {
        [SerializeField]
        private ItemSlotUI _slotPrefab = null;

        private void Start()
        {
            SpawnInventory();
        }

        public void SpawnInventory()
        {   //Destroy any existing slots.
            transform.DestroyChildren();

            for (int x = 0; x < ItemIDs.INVENTORY_SIZE; x++)
                for (int y = 0; y < ItemIDs.INVENTORY_SIZE; y++)
                {
                    ItemSlotUI slot = Instantiate(_slotPrefab.gameObject, transform).GetComponent<ItemSlotUI>();
                    //Less memory allocations
                    slot.slot.x = x;
                    slot.slot.y = y;
                    //Get it to get its item from the inventory
                    slot.GetSlottedItem();
                }
        }
    }
}