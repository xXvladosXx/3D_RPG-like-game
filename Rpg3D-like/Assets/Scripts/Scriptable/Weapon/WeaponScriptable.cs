using System;
using System.Linq;
using Controller;
using Extensions;
using Inventory;
using Resistance;
using Scriptable.Arrows;
using Scriptable.Weapon.SkillsSpecification;
using UnityEngine;

namespace Scriptable.Weapon
{
    [Serializable]
    [CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
    public class WeaponScriptable : ModifiableItem
    {
        [SerializeField] private WeaponScriptable _previousWeaponUpgrade;
        [SerializeField] private WeaponScriptable _nextWeaponUpgrade;
        [SerializeField] private GameObject _prefabWeapon;
        [SerializeField] private AnimatorOverrideController _weaponOverrideController;

        public SerializableDictionary<DamageType, float>
            _damage = new SerializableDictionary<DamageType, float>();
        [SerializeField] private ArrowScriptable _arrow;
        [SerializeField] private Skill[] _weaponSkills;
    
        private Skill[] GetPreviousWeaponSkills()
        {
            return _previousWeaponUpgrade!=null ? _previousWeaponUpgrade.GetCurrentWeaponSkills() : null;
        }

        private Skill[] GetCurrentWeaponSkills()
        {
            return _weaponSkills;
        }
        public Skill[] GetWeaponSkills()
        {
            return GetPreviousWeaponSkills() == null ? GetCurrentWeaponSkills() : GetCurrentWeaponSkills().Concat(GetPreviousWeaponSkills()).ToArray();
        }
        
        [SerializeField] private float _attackRange = 1f;
        public float GetAttackRange => _attackRange;

        [SerializeField] private bool _isRightHanded = false;
        public bool GetWeaponHand => _isRightHanded;
    
        [SerializeField] private float _attackSpeed = 1f;
        public float GetAttackSpeed => _attackSpeed;
        
        private const string _weaponName = "Weapon";
    

        public void Spawn(Transform position, Animator animator)
        {
            if (_prefabWeapon != null)
            {
                GameObject weapon = Instantiate(_prefabWeapon, position);

                weapon.name = _weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (_weaponOverrideController != null)
            {
                animator.runtimeAnimatorController = _weaponOverrideController;
            }else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        public void DestroyOldWeapon(Transform righthand, Transform lefthand)
        {
            Transform oldWeapon = righthand.Find("Weapon");

            if (oldWeapon == null)
            {
                oldWeapon = lefthand.Find("Weapon");
            }
        
            if(oldWeapon == null) return;
        
            Destroy(oldWeapon.gameObject);
        }
   
        public void SpawnProjectile(Transform target, Transform damager, SerializableDictionary<DamageType, float> damage)
        {
            _arrow.LoadProjectile(target, damager, damage);
        }

        public ArrowScriptable GetProjectile()
        {
            return _arrow;
        }

        public override ItemObject IsUpgradable()
        {
            return _nextWeaponUpgrade == null ? null : _nextWeaponUpgrade;
        }

        public override void EquipItem(PlayerController playerController)
        {
            playerController.GetComponent<Equipment>().Equip(this, playerController.GetComponent<Equipment>().GetCurrentWeapon == this);
        }

        public override string Description => $"Name: {name} \n" + 
                                              $"Damage: \n{_damage.Format()}" +
                                              $"Attack Range: {_attackRange} \n" +
                                              $"Speed Attack: {_attackSpeed}";
        
        
    }
}
