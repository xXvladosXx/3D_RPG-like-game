using System;
using Controller;
using UnityEngine;

namespace Inventory
{
    public abstract class ItemObject : ScriptableObject
    {
        public Sprite UIDisplay;
        public bool Stackable;
        public ItemCategory Category;
        public ItemData Data = new ItemData();

        public virtual ItemObject IsUpgradable()
        {
            return this;
        }
        
        public abstract void EquipItem(PlayerController playerController);
        public abstract string Description { get; }
    }
    
    [Serializable]
    public class ItemData
    {
        public string Name;
        public int Id = -1;

        public ItemData()
        {
            Name = "";
            Id = -1;
        }
        public ItemData(ItemObject itemObject)
        {
            Id = itemObject.Data.Id;
            Name = itemObject.name;
        }
    }
}