using System.Collections;
using System.Collections.Generic;
using UI.Cursor;
using UnityEngine;

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
