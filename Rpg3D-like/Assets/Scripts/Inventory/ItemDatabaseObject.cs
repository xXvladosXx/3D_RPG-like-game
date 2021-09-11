using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/ItemDatabase")]
    public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public ItemObject[] Items;
        public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

        public void OnBeforeSerialize()
        {
            GetItem = new Dictionary<int, ItemObject>();
        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].Data.Id = i;
                GetItem.Add(i, Items[i]);
            }
        }
    }
}