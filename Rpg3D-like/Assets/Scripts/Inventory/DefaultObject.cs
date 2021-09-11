using Controller;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName =  "New Default Object", menuName = "Inventory System/Items/Default")]
    public class DefaultObject : ItemObject
    {
        public void Awake()
        {
            Category = ItemCategory.None;
        }

        public override void EquipItem(PlayerController playerController)
        {
            
        }

        public override string Description
        {
            get
            {
                return 12.ToString();
            }
        }
    }
}