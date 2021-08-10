using UnityEngine;

namespace Inventories
{
    public class ItemsSpawnManager : MonoBehaviour 
    {
        public static ItemsSpawnManager Instance { get; set; }

        [SerializeField] private ItemPickUp.ItemInfo[] _items;
        private void Awake()
        {
            Instance = this;
        }
        
        public Item ItemTypeSwitcher(ItemType itemType, int amount = 1)
        {
            switch (itemType)
            {
                case ItemType.Sword0: return (new Item{IItem = new Sword0(), Amount = amount});
                    break;
                case ItemType.Sword1: return(new Item{IItem = new Sword1(), Amount = amount});
                    break;
                case ItemType.Sword2: return(new Item{IItem = new Sword2(), Amount = amount});
                    break;
                case ItemType.Sword3: return(new Item{IItem = new Sword3(), Amount = 1});
                    break;
                case ItemType.Sword4: return(new Item{IItem = new Sword4(), Amount = 1});
                    break;
                case ItemType.Bow0: return(new Item{IItem = new Bow0(), Amount = 1});;
                    break;
                case ItemType.Bow1: return (new Item{IItem = new Bow1(), Amount = 1});;
                    break;
                case ItemType.Bow2: return(new Item{IItem = new Bow2(), Amount = 1});;
                    break;
                case ItemType.Bow3: return(new Item{IItem = new Bow3(), Amount = 1});;
                    break;
                case ItemType.Bow4: return(new Item{IItem = new Bow4(), Amount = 1});;
                    break;
                case ItemType.HealthPotion: return(new Item{IItem = new HealthPotion(), Amount = amount});;
                    break;
                default:
                    return null;
            }
        }

        public void SpawnItem(ItemType itemType, Vector3 position)
        {
            foreach (var itemInfo in _items)
            {
                if (itemType == itemInfo.Item.ItemType)
                {
                    print(itemInfo.ItemPrefab);
                    Instantiate(itemInfo.ItemPrefab, position, Quaternion.identity);
                }
            }
        }
    }
}