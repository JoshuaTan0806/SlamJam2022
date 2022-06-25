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
        [SerializeField]
        private ItemBinUI _bin = null;

        private void Start()
        {
            ItemInventory.EquipItem(ItemData.CreateRandomItem(0), new Vector2Int(0, 0));
            ItemInventory.EquipItem(ItemData.CreateRandomItem(0), new Vector2Int(2, 2));
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

        public void ToggleInventory()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            _bin.gameObject.SetActive(!_bin.gameObject.activeSelf);
        }
    }
}