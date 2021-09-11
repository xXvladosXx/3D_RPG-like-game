using Controller;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName =  "New Potion Object", menuName = "Inventory System/Items/Potion")]
    public class PotionObject : ItemObject
    {
        [SerializeField] private float _healthRestoring;
        private void Awake()
        {
            Category = ItemCategory.Potion;
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