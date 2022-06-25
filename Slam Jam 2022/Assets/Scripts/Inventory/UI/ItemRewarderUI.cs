using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Items.UI
{
    public class ItemRewarderUI : MonoBehaviour
    {
        public ItemUI itemPrefab = null;
        public VerticalLayoutGroup parent = null;
        public int rewardCount = 3;

        private System.Action _onItemClaimed = null;

        private void OnEnable()
        {
            GrantRewards();
        }

        private void GrantRewards()
        {
            int next = ItemRewarder.GetNextReward();
            parent.transform.DestroyChildren();
            //No more rewards
            if (next == -1)
            {
                _onItemClaimed = null;
                gameObject.SetActive(false);
                return;
            }
            //Generate UI options
            for (int i = 0; i < rewardCount; i++)
            {
                ItemUI item = Instantiate(itemPrefab.gameObject, parent.transform).GetComponent<ItemUI>();
                item.item = ItemData.CreateRandomItem(next);

                item.RefreshUI();
                item.onEquip += _onItemClaimed.Invoke;
            }

            _onItemClaimed -= GrantRewards;
            _onItemClaimed += GrantRewards;
        }
    }
}