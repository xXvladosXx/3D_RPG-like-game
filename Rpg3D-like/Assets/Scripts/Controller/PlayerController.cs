using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Enums;
using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _clickedEffect;
    [SerializeField] private UIInventory _uiInventory;

    public event Action OnEnemyAttacked;
    
    private WeaponScriptable _weapon;
    private Health _health;
    private Inventory _playerInventory;
    private bool _isOpened = false;
    private GameObject _gameManager;
    
    private void Awake()
    {
        _playerInventory = new Inventory(UseItem);
        _health = GetComponent<Health>();
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion: _playerInventory.UsePotion(PotionEnum.Health, _health);
                break;
            case Item.ItemType.Bow: _playerInventory.EquipWeapon(WeaponEnum.Bow); ItemsSpawnManager.Instance.SpawnItem(item, gameObject.transform.position);
                break;
            case Item.ItemType.Sword: _playerInventory.EquipWeapon(WeaponEnum.Sword); ItemsSpawnManager.Instance.SpawnItem(item, gameObject.transform.position);
                break;
        }
    }

    private void Start()
    {
        _uiInventory.SetInventory(_playerInventory);
    }

    void Update()
    {
        // ShowHealthOnBar();
       
       if(_health.IsDead()) return;
       
       if (Input.GetKeyDown(KeyCode.I))
       {
           PausingGame(Convert.ToSingle(_isOpened));

           _isOpened = !_isOpened;

           _uiInventory.InteractWithUI(_uiInventory.transform, _isOpened);
       }
       
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
                if (OnEnemyAttacked != null) OnEnemyAttacked();
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

    public void SetWeapon(WeaponScriptable weapon)
    {
        _weapon = weapon;
        
        _playerInventory.AddItem(new Item {itemType = _weapon.GetItemType(),amount = 1});
    }

    public void InventoryPlacer(ItemPickUp item)
    {
        _playerInventory.AddItem(new Item{itemType = item.GetItemType(), amount = 1});
    }

    void PausingGame(float isPaused)
    {
        Time.timeScale = isPaused;
    }

    public void DropItem(Item item, Vector3 playerPositionPosition)
    {
        Vector3 randomDirection = new Vector3(playerPositionPosition.x + 10, playerPositionPosition.y, playerPositionPosition.z);
    }
}
