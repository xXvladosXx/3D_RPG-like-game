using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Enums;
using Saving;
using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _clickedEffect;
    public event Action<Transform> OnEnemyAttacked;
    
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    void Update()
    {
        // ShowHealthOnBar();
       
       if(_health.IsDead()) return;
       if(Attack()) return;
       if(Movement()) return ;
    }


    private bool Attack()
    {
        
        RaycastHit[] raycastHits = Physics.RaycastAll(GetRay());

        foreach (var raycastHit in raycastHits)
        {
            CombatTarget combatTarget = raycastHit.transform.GetComponent<CombatTarget>();
            if(combatTarget == null) continue;

            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<Combat>().Attack(combatTarget.transform);
                OnEnemyAttacked?.Invoke(combatTarget.transform);
            }
            return true;

        }

        return false;
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

                return true;
            }

            return false;
    }

    public static Ray GetRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}
