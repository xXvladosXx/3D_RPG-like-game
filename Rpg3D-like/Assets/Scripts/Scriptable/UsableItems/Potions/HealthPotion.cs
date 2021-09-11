using Controller;
using Stats;
using UnityEngine;

namespace Scriptable.UsableItems.Potions
{
    [CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/HealthPotion")]
    public class HealthPotion : Potion
    {
        public override void EquipItem(PlayerController playerController)
        {
            UseItem(playerController);
        }
        
        public override void UseItem(PlayerController playerController)
        {
            
            playerController.GetComponent<Health>().RegenerateHealth(RegenerateValue);
        }
        
        public override string Description => name;
        
    }
}