using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factories;

namespace Items
{
    /// <summary>
    /// Contains info for generation random items
    /// </summary>
    [CreateAssetMenu(fileName = "ItemBuilder", menuName = "Items/Builder", order = 0)]
    public class ItemBuilder : SingletonFactory<ItemBuilder>
    {
    }
}