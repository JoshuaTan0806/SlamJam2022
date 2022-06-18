using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    /// <summary>
    /// Inventory for items
    /// </summary>
    public static class ItemInventory
    {
        /// <summary>
        /// Currently equipped items
        /// </summary>
        private static ItemData[,] _equipped = new ItemData[ItemIDs.INVENTORY_SIZE, ItemIDs.INVENTORY_SIZE];
        /// <summary>
        /// The combined stats of all equipped items
        /// </summary>
        private static StatDictionary _combinedItemStats = new StatDictionary();
        /// <summary>
        /// Called everytime the items are refreshed
        /// </summary>
        public static System.Action onItemsRefresh = null;
        /// <summary>
        /// Equips an item
        /// </summary>
        /// <param name="item">The item to equip. Set to null to unequip</param>
        /// <param name="slot">The slot to equip to</param>
        public static void EquipItem(ItemData item, Vector2Int slot)
        {
            _equipped[slot.x, slot.y] = item;
            //Not instanced
            if (item && !item.IsInstance)
                throw ItemIDs.NOT_INSTANCED_ERROR;

            RefreshItemBonuses();
        }
        /// <summary>
        /// Refreshs the bonuses you get from items
        /// </summary>
        public static void RefreshItemBonuses()
        {   //Rebuild bonus stats from items
            _combinedItemStats.Clear();

            StatDictionary itemStats;
            for (int x = 0; x < ItemIDs.INVENTORY_SIZE; x++)
                for (int y = 0; y < ItemIDs.INVENTORY_SIZE; y++)
                {
                    ItemData data = _equipped[x, y];
                    //Item is null
                    if (!data)
                        continue;

                    int validConnections = 0;
                    //Calculate how many connections are valid.
                    foreach (var connection in data.possibleConnections.Keys)
                    {   //Get the index of the item one of our connections points to.
                        int oX = x;
                        int oY = y;
                        switch (connection)
                        {
                            case ConnectionDirection.NORTH:
                                oY++;
                                break;
                            case ConnectionDirection.SOUTH:
                                oY--;
                                break;
                            case ConnectionDirection.EAST:
                                oX++;
                                break;
                            case ConnectionDirection.WEST:
                                oX--;
                                break;
                            default:
                                throw ItemIDs.NOT_IMPLEMENTED_CONNECTION;
                        }
                        //Invalid index
                        if (oX < 0 || oX >= ItemIDs.INVENTORY_SIZE || oY < 0 || oY >= ItemIDs.INVENTORY_SIZE)
                            continue;
                        //Get Item in adjacent slot
                        var otherItem = _equipped[oX, oY];
                        //Item is null
                        if (!otherItem)
                            continue;

                        var otherConnections = otherItem.possibleConnections;
                        //Get the opposite direction
                        ConnectionDirection opposite = ItemIDs.GetOppositeDirection(connection);
                        if (otherConnections.ContainsKey(opposite) 
                            //Check connections
                            && (data.possibleConnections[connection] == otherConnections[opposite]
                                //Check for any conneciton
                                || data.possibleConnections[connection] == ConnectionType.ANY_ALSO_WHITE
                                || otherConnections[opposite] == ConnectionType.ANY_ALSO_WHITE))
                                //Is valid connection
                                validConnections++;
                    }
                    //Set the level of the item
                    data.SetLevel(validConnections);

                    itemStats = data.GetStats();

                    //Refresh bonus stats
                    foreach (var stat in itemStats.Keys)
                    {   //If has, add
                        if (_combinedItemStats.ContainsKey(stat))
                            _combinedItemStats[stat] += itemStats[stat];
                        //Else set
                        else
                            _combinedItemStats[stat] = itemStats[stat];
                    }
                }

            onItemsRefresh.SafeInvoke();
        }
        /// <summary>
        /// Get an item from the inventory.
        /// </summary>
        /// <param name="index">Index of the item to get</param>
        /// <returns>Returns the item at the index. Returns null if no item</returns>
        public static ItemData GetItem(Vector2Int index)
        {
            ItemData ret = _equipped[index.x, index.y];

            if (ret && !ret.IsInstance)
                throw ItemIDs.NOT_INSTANCED_ERROR;

            return ret;
        }
        /// <summary>
        /// Gets the combined stats from all equipped items
        /// </summary>
        /// <returns></returns>
        public static StatDictionary GetItemStats() => _combinedItemStats;
    }
}