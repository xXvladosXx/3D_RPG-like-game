using UnityEngine;

namespace UI.Inventory
{
    public class ItemsSpawnManager : MonoBehaviour 
    {
        public static ItemsSpawnManager Instance { get; set; }

        [SerializeField] private WeaponPickUp _bowPickUp;
        [SerializeField] private WeaponPickUp _swordPickUp;
        
        private void Awake()
        {
            Instance = this;
        }

        public void SpawnItem(Item item, Vector3 position)
        {
            switch (item.itemType)
            {
                case Item.ItemType.Sword: Instantiate(_swordPickUp, position, Quaternion.identity); print("Spawning");
                    break;
                case Item.ItemType.Bow: Instantiate(_bowPickUp, position, Quaternion.identity); print("Spawning");
                    break;
            }
        }
    }
}