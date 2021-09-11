using Stats;
using UnityEngine;

namespace LootSystem
{
    public class LootSystem : MonoBehaviour
    {

        [SerializeField] private GameObject _lootBag; 
        
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();

            _health.OnDied += InstantiateLoot;
        }

        private void InstantiateLoot()
        {
            GameObject loot = Instantiate(_lootBag, transform.parent);
            var position = gameObject.transform.position;
            loot.transform.position = new Vector3(position.x, position.y+0.5f, position.z);
        }
    }
}
