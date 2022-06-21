using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;
using Sirenix.OdinInspector;

namespace Items
{
    /// <summary>
    /// Contains info for generation random items
    /// </summary>
    [CreateAssetMenu(fileName = "ItemBuilder", menuName = "Items/Builder", order = 0)]
    public class ItemBuilder : SingletonFactory<ItemBuilder>
    {
        private static List<GenericSpill> tempSpill = new List<GenericSpill>();
        private static StatDictionary tempStats = new StatDictionary();
        #region Type
#if UNITY_EDITOR
        [OnCollectionChanged("RefreshTypeProbabilty")]
#endif
        [BoxGroup("Type")]
        public TypeChances itemTypeChances = new TypeChances();
#if UNITY_EDITOR
        [Button]
        [BoxGroup("Type")]
        private void RefreshTypeProbabilty()
        {
            float total = 0;

            foreach (var value in itemTypeChances.Values)
                total += value.weight;

            if (total != 0)
                foreach (var value in itemTypeChances.Values)
                    value.probability = (value.weight / total) * 100f;
        }
#endif
        #endregion

        public ItemLevelInfoDictionary levelInfo = new ItemLevelInfoDictionary();
        /// <summary>
        /// Rolls a random type
        /// </summary>
        /// <returns></returns>
        public ItemType RollType()
        {
            float total = 0;

            foreach (var v in itemTypeChances.Values)
                total += v.weight;

            float rand = UnityEngine.Random.Range(0, total);

            total = 0;

            foreach(var k in itemTypeChances.Keys)
            {
                total += itemTypeChances[k].weight;

                if (total > rand)
                    return k;
            }

            throw new Exception("Type failed to roll somehow?");
        }
        /// <summary>
        /// Rolls a random skill for an item of level level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public GenericSpill RollSkill(byte level)
        {
            var l = GetInfo(level);

            float rand = UnityEngine.Random.Range(0f, 1f);
            //No skill
            if (rand > l.skillChance)
                return null;

            tempSpill.Clear();
            tempSpill.AddRange(l.possibleSkills);
            //Also add the lower levels to the list
            if (l.includeLowerLevel)
            {
                int range = l.levelRange;
                int cur = 0;

                while (cur < range)
                {   //Give up no more
                    if (!levelInfo.ContainsKey((byte)(level - cur)))
                        break;

                    var temp = GetInfo((byte)(level - cur));
                    //Add lower level spills but not duplicates
                    foreach (var spill in temp.possibleSkills)
                        if (!tempSpill.Contains(spill))
                            tempSpill.Add(spill);

                    cur++;
                }
            }

            //Return a random item from the list
            int rand2 = UnityEngine.Random.Range(0, tempSpill.Count);
            return tempSpill[rand2];
        }
        /// <summary>
        /// Generates the connections for an item
        /// </summary>
        /// <param name="level"></param>
        /// <param name="dictionary"></param>
        public void RollConnections(byte level, ref ItemData.ConnectionDictionary dictionary)
        {
            var l = GetInfo(level);

            float total = 0;

            foreach (var v in l.connectionChance.Values)
                total += v.weight;

            float rand = UnityEngine.Random.Range(0, total);

            byte amount = 0;
            total = 0;
            bool found = false;
            //Find how many connections we should have
            while (!found && amount < 4)
            {
                if (l.connectionChance.ContainsKey(amount))
                {
                    total += l.connectionChance[amount].weight;

                    if (rand < total)
                        break;
                }
                else
                {   //Just presume that there is no more amounts that we can have
                    amount = 0;
                    break;
                }

                amount++;
            }

            List<ConnectionDirection> connections = new List<ConnectionDirection>();
            connections.Add(ConnectionDirection.NORTH);
            connections.Add(ConnectionDirection.SOUTH);
            connections.Add(ConnectionDirection.EAST);
            connections.Add(ConnectionDirection.WEST);

            total = 0;
            foreach (var v in l.typeChances.Values)
                total += v.weight;
            //Roll the connections
            float temp;
            for (int i = 0; i < amount; i++)
            {
                int rand2 = UnityEngine.Random.Range(0, connections.Count);
                //Select a random type
                ConnectionType type = ConnectionType.RED;
                rand = UnityEngine.Random.Range(0, total);
                temp = 0;
                foreach(var k in l.typeChances.Keys)
                {
                    temp += l.typeChances[k].weight;

                    if (rand < temp)
                    {
                        type = k;
                        break;
                    }
                }

                dictionary.Add(connections[rand2], type);
                connections.RemoveAt(rand2);
            }
        }

        public void RollStats(byte level, ref StatDictionary dictionary)
        {
            //var l = GetInfo(level);

            //tempStats.Clear();

            //int amount = UnityEngine.Random.Range(l.possibleNumberOfStats.x, l.possibleNumberOfStats.y + 1);

            //List<Stat> s = new List<Stat>();
            //foreach (var k in l.statChances.Keys)
            //{   //Random the stat
            //    s.Add(k);
            //    tempStats[k] = UnityEngine.Random.Range(l.statChances[k].statRange.x, l.statChances[k].statRange.y);
            //}
            ////If use previous levels stats, get them
            //if (l.usePreviousLevel)
            //{   
            //    bool checkPrevious = true;
            //    int cur = 0;
            //    while (checkPrevious)
            //    {
            //        cur++;
            //        //Make sure there is a previous level to get
            //        byte n = (byte)(level - cur);

            //        if (!levelInfo.ContainsKey(n))
            //            break;

            //        var temp = GetInfo(n);
            //        //Add it stats
            //        foreach (var k in temp.statChances.Keys)
            //        {   //Already got that stat
            //            if (tempStats.ContainsKey(k))
            //                continue;
            //            //Random the stat
            //            s.Add(k);
            //            tempStats[k] = UnityEngine.Random.Range(temp.statChances[k].statRange.x, temp.statChances[k].statRange.y);
            //        }
            //        //Keep going until a level says not to use the previous stats.
            //        checkPrevious = temp.usePreviousLevel;
            //    }
            //}
            ////Get random stats
            //while (amount > 0)
            //{
            //    int rand = UnityEngine.Random.Range(0, s.Count);
            //    //Add stat
            //    dictionary.Add(s[rand], tempStats[s[rand]]);
            //    s.RemoveAt(rand);

            //    amount--;
            //}
        }

        private ItemLevelInfo GetInfo(byte level)
        {
            ItemLevelInfo l = null;

            if (levelInfo.ContainsKey(level))
                l = levelInfo[level];
            else
            {
                byte closest = 0;
                foreach(byte key in levelInfo.Keys)
                {
                    if (key < level && key > closest)
                        closest = key;
                }

                l = levelInfo[closest];
            }

            return l;
        }

        #region Classes
        [Serializable]
        public class ItemLevelInfo
        {

            #region Skills
            [BoxGroup("Skills")]
            [Range(0f, 1f)]
            [Tooltip("The likely hood of an item of this level having a skill")]
            public float skillChance = 0.5f;

            [BoxGroup("Skills")]
            [Tooltip("To include lower level skills in the pool")]
            [HideIf("skillChance", Value = 0f)]
            public bool includeLowerLevel = false;
            [BoxGroup("Skills")]
            [ShowIf("ShowChance")]
            [Tooltip("How many levels below this to include the skills of")]
            public byte levelRange = 0;
            /// <summary>
            /// Used to check if the property should show
            /// </summary>
            private bool ShowChance => includeLowerLevel && skillChance != 0;
            [BoxGroup("Skills")]
            [HideIf("skillChance", Value = 0f)]
            [Tooltip("The skills that can be chosen from")]
            public List<GenericSpill> possibleSkills = new List<GenericSpill>();
            #endregion

            #region Connections
#if UNITY_EDITOR
            [OnCollectionChanged("RefreshConnectionProbabilty")]
#endif
            [BoxGroup("Connections")]
            public ConnectionChances connectionChance = new ConnectionChances();
#if UNITY_EDITOR
            [OnCollectionChanged("RefreshConnectionTypeProbabilty")]
#endif
            [BoxGroup("Connections")]
            public ConnectionTypeChances typeChances = new ConnectionTypeChances();

#if UNITY_EDITOR
            [Button]
            [BoxGroup("Connections")]
            private void RefreshConnectionProbabilty()
            {
                float total = 0;

                foreach (var value in connectionChance.Values)
                    total += value.weight;

                if (total != 0)
                    foreach (var value in connectionChance.Values)
                        value.probability = (value.weight / total) * 100f;
            }
            [Button]
            [BoxGroup("Connections")]
            private void RefreshConnectionTypeProbabilty()
            {
                float total = 0;

                foreach (var value in typeChances.Values)
                    total += value.weight;

                if (total != 0)
                    foreach (var value in typeChances.Values)
                        value.probability = (value.weight / total) * 100f;
            }
#endif
            #endregion

            #region Stats

            [BoxGroup("Stats")]
            [Tooltip("To also include the previous levels")]
            public bool usePreviousLevel = false;
            [BoxGroup("Stats")]
            public Vector2Int possibleNumberOfStats;
#if UNITY_EDITOR
            [OnCollectionChanged("RefreshStatProbability")]
#endif
            [BoxGroup("Stats")]
            public StatChanceDictionary statChances = new StatChanceDictionary();

#if UNITY_EDITOR
            [Button]
            [BoxGroup("Stats")]
            private void RefreshStatProbability()
            {
                float total = 0;

                foreach (var value in statChances.Values)
                    total += value.weight;

                if (total != 0)
                    foreach (var value in statChances.Values)
                        value.probability = (value.weight / total) * 100f;
            }
#endif
#endregion

            [Serializable]
            public class StatChance : Chance
            {
                public Vector2 statRange = Vector2.up;
            }

            [Serializable]
            public class StatChanceDictionary : SerializableDictionary<Stat, StatChance> {}
            [Serializable]
            public class ConnectionChances : SerializableDictionary<byte, Chance> {}
            [Serializable]
            public class ConnectionTypeChances : SerializableDictionary<ConnectionType, Chance> { }
        }
        /// <summary>
        /// Stores weight with probability
        /// </summary>
        [Serializable]
        public class Chance
        {
            public float weight = 0;
#if UNITY_EDITOR
            [ReadOnly]
            public float probability = 0;
#endif
        }

        [Serializable]
        public class ItemLevelInfoDictionary : SerializableDictionary<byte, ItemLevelInfo> { }
        [Serializable]
        public class TypeChances : SerializableDictionary<ItemType, Chance> { }
        #endregion
    }
}