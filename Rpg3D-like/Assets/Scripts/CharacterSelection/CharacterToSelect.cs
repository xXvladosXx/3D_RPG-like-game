using Controller;
using UI.Cursor;
using UnityEngine;

namespace CharacterSelection
{
    public class CharacterToSelect : MonoBehaviour, IRaycastable
    {
        public PlayerController.CursorType GetCursorType()
        {
            return PlayerController.CursorType.Shop;
        }

        public bool HandleRaycast(PlayerController player = null)
        {
            return true;
        }
    }
}
