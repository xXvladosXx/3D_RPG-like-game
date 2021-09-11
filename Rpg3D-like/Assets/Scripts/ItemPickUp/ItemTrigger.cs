using Controller;
using UI.Cursor;
using UnityEngine;

namespace Inventory
{
    public class ItemTrigger : MonoBehaviour, IRaycastable
    {
        [SerializeField] private ItemObject _itemObject;
        [SerializeField] private int _amount;
        public ItemObject GetItem => _itemObject;
        public int GetItemAmount => _amount;
        
        public PlayerController.CursorType GetCursorType()
        {
            return PlayerController.CursorType.PickUp;
        }

        public bool HandleRaycast(PlayerController player)
        {
            if (Input.GetMouseButton(0))
            {
                player.GetComponent<Movement>().StartMoveToAction(gameObject.transform.position, 1f);
            }
            return true;
        }
    }
}