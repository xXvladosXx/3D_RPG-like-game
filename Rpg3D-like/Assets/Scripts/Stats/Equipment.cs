using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Inventory;
using Resistance;
using SavingSystem;
using Scriptable.Stats;
using Scriptable.Weapon;
using Stats;
using UnityEngine;

public class Equipment : MonoBehaviour, IModifierStat, ISaveable
{
    [SerializeField] private InventoryObject _inventory;
    [SerializeField] private ChestScriptable _chest;
    [SerializeField] private BootsScriptable _boots;
    [SerializeField] private string _defaultWeaponName = "Sword 0";
    [SerializeField] private Transform _leftHandTransform;
    [SerializeField] private Transform _rightHandTransform;
    [SerializeField] private Transform _chestTransform;

    private Armour _armour;
    public ChestScriptable GetCurrentChest => _chest;
    public BootsScriptable GetCurrentBoots => _boots;

    public WeaponScriptable GetCurrentWeapon => _currentWeapon;
    
    public event Action OnEquipmentChanged;
    public event Action<WeaponScriptable> OnWeaponChanged;

    private WeaponScriptable _currentWeapon;
    private WeaponScriptable _defaultWeapon;
    private void Awake()
    {
        _armour = GetComponent<Armour>();

        _defaultWeapon = Resources.Load<WeaponScriptable>("Weapons/" + _defaultWeaponName);
        Equip(_defaultWeapon);
        
        foreach (var inventorySlot in _inventory._inventory.Items)
        {
            if (!(inventorySlot.ItemObject is ModifiableItem mi)) continue;
            Equip(mi);
        }
    }

    public void Equip(ModifiableItem equipment, bool unequip = false)
    {
        switch (equipment)
        {
            case ChestScriptable cs:
                EquipInternalArmour(unequip ? null : cs, (b) => this._chest = (ChestScriptable)b, () => this._chest, () => _chestTransform);
                break;
            case BootsScriptable bs:
                EquipInternalArmour(unequip ? null : bs, (b) => this._boots = (BootsScriptable)b, () => this._boots, () => _chestTransform);
                break;
            case WeaponScriptable ws:
                EquipInternalWeapon(unequip ? null : ws);
                break;
        }
    }

    private void EquipInternalArmour(ArmourObject item, Action<ArmourObject> setter, Func<ArmourObject> getter, Func<Transform> getterTrans)
    {
        if (getter() != null)
        {
            _armour.SetResistance(getter().GetDamageResistances, true);
        }

        setter(item);
        
        OnEquipmentChanged?.Invoke();
        if(getter() == null) return;
        
        getter().Spawn(getterTrans());
        _armour.SetResistance(getter().GetDamageResistances);
    }
    
    private void EquipInternalWeapon(WeaponScriptable item)
    {
        if (item == null)
        {
            item = _defaultWeapon;
        }

        _currentWeapon = item;
        
        if (TryGetComponent(out PlayerSkills playerSkills))
        {
            playerSkills.SetPlayerSkills(item.GetWeaponSkills());
        }
        
        Animator animator = GetComponent<Animator>();

        item.DestroyOldWeapon(_rightHandTransform, _leftHandTransform);
        item.Spawn(item.GetWeaponHand ? _rightHandTransform : _leftHandTransform, animator);
        OnWeaponChanged?.Invoke(item);
    }
    

    public IEnumerable<float> GetStatModifier(StatsEnum stat)
    {
        foreach (var modifiableItem in _inventory._inventory.Items)
        {
            if(modifiableItem.ItemObject == null) continue;
            
            yield return ((ModifiableItem) modifiableItem.ItemObject).GetModifiers(stat);
        }
    }
    
    public object CaptureState()
    {
        if (_currentWeapon == null)
        {
            return _defaultWeapon.name;
        }

        return _currentWeapon.name;
    }

    public void RestoreState(object state)
    {
        string weaponName = (string) state;
        WeaponScriptable weapon = Resources.Load<WeaponScriptable>("Weapons/" +  weaponName );
        Equip(weapon);
    }

}
