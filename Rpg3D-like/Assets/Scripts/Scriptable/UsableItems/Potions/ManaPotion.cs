using Controller;
using Stats;
using UnityEngine;

namespace Scriptable.UsableItems.Potions
{
    [CreateAssetMenu(fileName = "ManaPotion", menuName = "Items/ManaPotion")]
    public class ManaPotion : Potion
    {
        public override void EquipItem(PlayerController playerController)
        {
            UseItem(playerController);
        }

        public override void UseItem(PlayerController playerController)
        {
            playerController.GetComponent<Mana>().RegenerateMana(RegenerateValue);
        }
        public override string Description => name;
    }
}