using System;
using System.Collections;
using System.Collections.Generic;
using UI.Cursor;
using UnityEngine;

public class CombatTarget : MonoBehaviour, IRaycastable
{
    public PlayerController.CursorType GetCursorType()
    {
        return PlayerController.CursorType.Attack;
    }

    public bool HandleRaycast(PlayerController player)
    {
        if (!player.GetComponent<Combat>().CanAttack(gameObject)) return false;

        if (Input.GetMouseButton(0))
        {
            player.GetComponent<Combat>().Attack(gameObject.transform);
        }
        return true;
    }
}
