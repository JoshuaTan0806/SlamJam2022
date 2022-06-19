using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Items
{
    /// <summary>
    /// Item class
    /// </summary>
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
    public class ItemData : ScriptableObject
    {
        #region Variables
        private static List<ConnectionDirection> tempList = new List<ConnectionDirection>();
        private static List<ConnectionType> tempTypeList = new List<ConnectionType>();
        /// <summary>
        /// Is this a data or instance?
        /// </summary>
        private bool _isInstance = false;
        /// <summary>
        /// Is this a data or instance?
        /// </summary>
        public bool IsInstance => _isInstance;
        /// <summary>
        /// The type of item this is
        /// </summary>
        [SerializeField]
        private ItemType type = ItemType.NULL;
        /// <summary>
        /// The type of item this is
        /// </summary>
        public ItemType Type => type;
        /// <summary>
        /// To randomise the available connections
        /// </summary>
        [Tooltip("The to randomise the possible connections from the given options")]
        public bool randomiseConnectionDirection = false;
        /// <summary>
        /// The chance to remove a connection direction during the randomization process
        /// </summary>
        [Range(0, 1)]
        [ShowIf("randomiseConnectionDirection")]
        public double removeChance = 0.5;
        /// <summary>
        /// To randomise the connection type of each connection
        /// </summary>
        [Tooltip("The to randomise the possible colours")]
        public bool randomiseConnectionType = false;
        /// <summary>
        /// The connections the item can have
        /// </summary>
        [Tooltip("The possible connections this item can have")]
        public ConnectionDictionary possibleConnections = new ConnectionDictionary();
        /// <summary>
        /// The stats for the item
        /// </summary>
        [SerializeField]
        private StatDictionary stats = new StatDictionary();
        /// <summary>
        /// The asset to reference
        /// </summary>
        [SerializeField]
        private GenericSpill skill = null;
        /// <summary>
        /// Instanced copy of the asset
        /// </summary>
        private GenericSpill instance = null;
        /// <summary>
        /// Getter & Setter to make sure intance is always correctly set
        /// </summary>
        public GenericSpill Spill
        {
            get
            {
                if (instance == null && skill != null)
                    instance = Instantiate(skill);

                return instance;
            }
            set
            {
                skill = value;

                if (value == null)
                    instance = null;
                else
                    instance = Instantiate(skill);
            }
        }
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
                    tempTypeList.Clear();
                    var type = ret.possibleConnections[dir];
                    //Build the possible options
                    foreach (ConnectionType value in System.Enum.GetValues(type.GetType()))
                        if (type.HasFlag(value))
                            tempTypeList.Add(value);
                    //Pick a random option
                    int rand = Random.Range(0, tempTypeList.Count);
                    ret.possibleConnections[dir] = tempTypeList[rand];
                }
            #endregion

            return ret;
        }
        /// <summary>
        /// Generates a completely random item
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static ItemData CreateRandomItem(int scale)
        {
            var rollData = ItemBuilder.Instance;
            ItemData ret = CreateInstance<ItemData>();

            byte level = (byte)scale;

            ret._isInstance = true;
            ret.type = rollData.RollType();
            ret.Spill = rollData.RollSkill(level);
            rollData.RollConnections(level, ref ret.possibleConnections);
            rollData.RollStats(level, ref ret.stats);

            return ret;
        }
        /// <summary>
        /// Sets the level of the item
        /// </summary>
        /// <param name="index">The level of the item</param>
        public void SetLevel(int index)
        {
            if (!_isInstance)
                throw ItemIDs.NOT_INSTANCED_ERROR;
            //Calculate stat bonuses
        }
        /// <summary>
        /// Scale the items power
        /// </summary>
        /// <param name="scale"></param>
        public void SetScale(int scale)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Gets the stats for the items
        /// </summary>
        /// <returns></returns>
        public StatDictionary GetStats()
        {
            if (!_isInstance)
                throw ItemIDs.NOT_INSTANCED_ERROR;

            return stats;
        }
        /// <summary>
        /// Converts the item to json
        /// </summary>
        /// <returns>Returns the item has json</returns>
        public string Save()
        {
            return JsonUtility.ToJson(this);
        }
        /// <summary>
        /// Loads an Item from Json
        /// </summary>
        /// <param name="json"></param>
        /// <returns>Return an instanced copy of json</returns>
        public static ItemData Load(string json)
        {
            ItemData ret = JsonUtility.FromJson<ItemData>(json);
            ret._isInstance = true;

            return ret;
        }
        #endregion

        /// <summary>
        /// Stores connect information
        /// </summary>
        [System.Serializable]
        public class ConnectionDictionary : Dictionary<ConnectionDirection, ConnectionType> { }
    }
    /// <summary>
    /// The type of item this is
    /// </summary>
    public enum ItemType
    {
        NULL = 0
    }
    /// <summary>
    /// The connections items can have
    /// </summary>
    public enum ConnectionDirection
    {
        NORTH = 0,
        EAST = 1,
        SOUTH = 2,
        WEST = 3
    }
    /// <summary>
    /// The type of connections
    /// </summary>
    [System.Flags]
    public enum ConnectionType
    {
        RED = 1,
        BLUE = 2,
        GREEN = 4,
        ANY_ALSO_WHITE = 8,
        //Other = 16,
        //Other = 32,
        //Other = 64,
        //Other = etc,


        Count //Should always be last
    }
}