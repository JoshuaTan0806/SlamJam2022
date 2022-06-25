using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.UI
{
    /// <summary>
    /// For determining the highest parent for UI
    /// </summary>
    public class MasterUIParent : MonoBehaviour
    {
        public static Transform s_masterParent = null;

        void Start()
        {
            s_masterParent = transform;
        }
    }
}