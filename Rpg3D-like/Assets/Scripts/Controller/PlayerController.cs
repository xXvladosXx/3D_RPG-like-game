using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeMonkey.Utils;
using Controller;
using Enums;
using Saving;
using TMPro;
using UI.Cursor;
using UI.Inventory;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _clickedEffect;
    public event Action<Transform> OnEnemyAttacked;

    private Health _health;

    public enum CursorType
    {
        None,
        Attack,
        Move,
        UI,
        PickUp,
        Shop,
        Quest,
        Upgrade
    }

    [Serializable]
    struct CursorIterating
    {
        public CursorType Type;
        public Vector2 Hotspot;
        public Texture2D Texture;
    }

    [SerializeField] private CursorIterating[] _cursorIteratings;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    void Update()
    {
        if(PointerOverUI()) return;
        
        if (_health.IsDead())
        {
            SetCursor(CursorType.None);
            return;
        }

        if(InteractWithComponent()) return;
        if (Movement()) return;

        SetCursor(CursorType.None);
    }

    private bool PointerOverUI()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) return false;
        
        SetCursor(CursorType.UI);
        return true;
    }

    private bool InteractWithComponent()
    {
        RaycastHit[] hits = Physics.RaycastAll(GetRay());

        foreach (var hit in hits)
        {
            IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
            foreach (var raycastable in raycastables)
            {
                if (raycastable.HandleRaycast(this))
                {
                    OnEnemyAttacked?.Invoke(FindObjectOfType<CombatTarget>().transform);
                    SetCursor(raycastable.GetCursorType());
                    return true;
                }
            }
        }
        
        return false;
    }

    private void SetCursor(CursorType type)
    {
        CursorIterating iterating = GetCursorIterating(type);
        Cursor.SetCursor(iterating.Texture, iterating.Hotspot, CursorMode.Auto);
    }

    private CursorIterating GetCursorIterating(CursorType type)
    {
        foreach (var cursorIterating in _cursorIteratings)
        {
            if (cursorIterating.Type == type)
                return cursorIterating;
        }

        return _cursorIteratings[0];
    }

    private bool Movement()
    {
        RaycastHit raycastHit;
        bool hasHit = Physics.Raycast(GetRay(), out raycastHit);

        if (hasHit)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                ParticleSystem clickedEffect = Instantiate(_clickedEffect, raycastHit.point, Quaternion.identity);
                Destroy(clickedEffect.gameObject, 0.1f);
            }

            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                GetComponent<Movement>().StartMoveToAction(raycastHit.point, 1f);
                float checkDistance = Vector3.Distance(gameObject.transform.position, raycastHit.point);
            }

            SetCursor(CursorType.Move);
            return true;
        }

        return false;
    }

    public static Ray GetRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}