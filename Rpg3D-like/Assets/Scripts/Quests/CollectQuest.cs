using System;
using Inventory;
using Stats;
using TMPro;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "CollectingQuest", menuName = "Quests/CollectingQuest", order = 0)]
    public class CollectQuest : InitializationQuest
    {
        [SerializeField] private ItemObject _item;
        public override void InitQuest(Action completed)
        {
            GameObject player = GameObject.FindWithTag("Player");
            
            foreach (var inventorySlot in player.GetComponent<PlayerInventory>().InventoryObject._inventory.Items)
            {
                if(inventorySlot == null) continue;
                if (inventorySlot.itemData.Id == _item.Data.Id)
                {
                    Debug.Log("Quest Completed");
                    completed();
                    
                    return;
                }
            }
            
            player.GetComponent<PlayerInventory>().OnItemPicked += o =>
            {
                if (o == _item)
                {
                    Debug.Log("Quest Completed");
                    completed();
                }
            };

        }


        public override GameObject GetAim()
        {
            return null;
        }
    }
}