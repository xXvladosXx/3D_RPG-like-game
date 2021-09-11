using System.Linq;
using Controller;
using Inventory;
using Resistance;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scriptable.Weapon
{
    [CreateAssetMenu(fileName = "Chest", menuName = "ScriptableObjects/Chest", order = 1)]

    public class ChestScriptable : ArmourObject
    {
        public void DestroyOldChest(Transform chest)
        {
            Transform oldChest = chest.Find("Chest");

            if (oldChest == null) return;

            Destroy(oldChest.gameObject);
        }

        public override Inventory.ItemObject IsUpgradable()
        {
            return _nextChestUpgrade;
        }

        public override string Description => $"Name: {name} \n" +
                                              $"Armour: {_armour} \n" +
                                              $"Health: {_healthBonus}";

        public override void EquipItem(PlayerController playerController)
        {
            playerController.GetComponent<Equipment>().Equip(this,
                playerController.GetComponent<Equipment>().GetCurrentChest == this);
        }
    }
}