using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public static class ItemRewarder
    {
        private static Queue<int> s_rewards = new Queue<int>();

        public static void GrantReward(int level)
        {
            s_rewards.Enqueue(level);
        }

        public static int GetNextReward()
        {
            if (s_rewards.Count > 0)
                return s_rewards.Dequeue();

            return -1;
        }
    }
}