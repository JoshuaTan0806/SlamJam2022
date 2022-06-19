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
        public ItemLevelInfoDictionary levelInfo = new ItemLevelInfoDictionary();

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
            [ShowIf("includeLowerLevel")]
            [HideIf("skillChance", Value = 0f)]
            [Tooltip("How many levels below this to include the skills of")]
            public byte levelRange = 0;
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
            [OnCollectionChanged("RefreshTypeProbabilty")]
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
            private void RefreshTypeProbabilty()
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
            public class StatChance : Chance
            {
                public Vector2 statRange = Vector2.up;
            }

            [Serializable]
            public class StatChanceDictionary : SerializableDictionary<Stat, StatChance> {}
            [Serializable]
            public class ConnectionChances : SerializableDictionary<byte, Chance> {}
            [Serializable]
            public class ConnectionTypeChances : SerializableDictionary<ConnectionType, Chance> {}
        }

        [Serializable]
        public class ItemLevelInfoDictionary : SerializableDictionary<byte, ItemLevelInfo> { }
    }
}