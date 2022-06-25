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
        private ItemType type;
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
        private StatDictionary bonusStats = new StatDictionary();
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
        /// <param name="level">The level of the item</param>
        public void SetLevel(int level)
        {
            if (!_isInstance)
                throw ItemIDs.NOT_INSTANCED_ERROR;
            //Calculate stat bonuses
            foreach (var stat in stats.Keys)
            {
                var s = Instantiate(stats[stat]);

                bonusStats[stat] = s;

                var c = ItemBuilder.Instance.statChances[stat];
                //Ignore cap
                s.ModifyStat(c.type, c.gainPerLevel * level);
            }
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

            StatDictionary ret = new StatDictionary();

            foreach (var k in stats.Keys)
            {
                if (bonusStats.ContainsKey(k))
                    ret[k] = stats[k] + bonusStats[k];
                else
                    ret[k] = Instantiate(stats[k]);
            }

            return ret;
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
        Helmet = 0,
        Armour = 1,
        Sword = 2,
        Belt,
        Staff,
        Shoes
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
    public enum ConnectionType
    {
        RED,
        BLUE,
        GREEN,
        ANY_ALSO_WHITE,
        //Other = 16,
        //Other = 32,
        //Other = 64,
        //Other = etc,


        Count //Should always be last
    }
}