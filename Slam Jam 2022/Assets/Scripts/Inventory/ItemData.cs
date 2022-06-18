using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Items
{
    /// <summary>
    /// Item class
    /// </summary>
    public class ItemData : ScriptableObject
    {
        #region Variables
        private static List<ConnectionDirection> tempList = new List<ConnectionDirection>();
        /// <summary>
        /// Is this a data or instance?
        /// </summary>
        private bool _isInstance = false;
        /// <summary>
        /// Is this a data or instance?
        /// </summary>
        public bool IsInstance => _isInstance;

        [SerializeField]
        private ItemType type = ItemType.NULL;
        /// <summary>
        /// The type of item this is
        /// </summary>
        public ItemType Type => type;

        [Tooltip("The to randomise the possible connections from the given options")]
        public bool randomiseConnectionDirection = false;
        [Range(0, 1)]
        [ShowIf("randomiseConnectionDirection")]
        public double removeChance = 0.5;
        [Tooltip("The to randomise the possible colours")]
        public bool randomiseConnectionType = false;

        [Tooltip("The possible connections this item can have")]
        public ConnectionDictionary possibleConnections = new ConnectionDictionary();
        #endregion

        #region Functions
        /// <summary>
        /// Creates a new instance of this item.
        /// </summary>
        /// <returns></returns>
        public ItemData CreateInstance()
        {
            if (IsInstance)
                throw new System.Exception("Attempting to Instance instanced Item");

            ItemData ret = CreateInstance<ItemData>();
            ret.type = type;
            ret._isInstance = true;

            #region Connection Randomisation
            if (randomiseConnectionDirection)
            {
                tempList.Clear();
                foreach (var dir in possibleConnections.Keys)
                    tempList.Add(dir);

                //Foreach existing direction
                for (int i = 0; i < tempList.Count; i++)
                {   //Randomise it to be one of the existing directions
                    float rand = Random.Range(0, 1);
                    //Don't remove
                    if (rand > removeChance)
                        ret.possibleConnections.Add(tempList[i], possibleConnections[tempList[i]]);
                }
            }
            else
                //Just direct copy the dictionary
                foreach (var dir in possibleConnections.Keys)
                    ret.possibleConnections[dir] = possibleConnections[dir];

            if (randomiseConnectionType)
                foreach (ConnectionDirection dir in ret.possibleConnections.Keys)
                {   //Randomise the type
                    int rand = Random.Range((int)ConnectionType.RED, (int)ConnectionType.Count);
                    ret.possibleConnections[dir] = (ConnectionType)rand;
                }
            #endregion

            return ret;
        }
        /// <summary>
        /// Rolls an item.
        /// </summary>
        /// <param name="itemToRoll">The item to roll</param>
        /// <returns></returns>
        public static ItemData RollItem(ItemData itemToRoll)
        {   //Null catch
            if (!itemToRoll)
                return null;

            return itemToRoll.CreateInstance();
        }

        public void SetLevel(int index)
        {
            if (!_isInstance)
                throw ItemIDs.NOT_INSTANCED_ERROR;
            //Calculate stat bonuses
        }
        #endregion

        /// <summary>
        /// Stores connect information
        /// </summary>
        [System.Serializable]
        public class ConnectionDictionary : Dictionary<ConnectionDirection, ConnectionType> { }
    }

    public enum ItemType
    {
        NULL = 0
    }
    public enum ConnectionDirection
    {
        NORTH = 0,
        EAST = 1,
        SOUTH = 2,
        WEST = 3
    }
    public enum ConnectionType
    {
        RED = 0,
        BLUE,
        GREEN,
        ANY_ALSO_WHITE,

        Count
    }
}