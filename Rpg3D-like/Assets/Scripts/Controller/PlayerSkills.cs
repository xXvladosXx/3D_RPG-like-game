using System;
using Scriptable.Weapon.SkillsSpecification;
using Stats;
using UI.PlayerBars.SkillBar;
using UnityEngine;

namespace Controller
{
    public class PlayerSkills : MonoBehaviour
    {
        [SerializeField] private Skill[] _playerSkills;
        public Skill[] GetPlayerSkills => _playerSkills;

        private CooldownSkillManager _cooldownSkillManager;
        private bool _isAbleToUseSkill = true;
        private Health _health;

        public event Action OnSkillsChanged;
        private void Awake()
        {
            _health = GetComponent<Health>();
            _cooldownSkillManager = GetComponent<CooldownSkillManager>();
        }

        private void Update()
        {
            if(_health.IsDead()) return;
            if(_playerSkills.Length == 0) return;
        
            if (Input.GetKeyDown(KeyCode.Alpha1) && this._playerSkills.Length >= 1 )
            {
                if(_cooldownSkillManager.GetCooldownSkill(_playerSkills[0]) > 0) return;
                InteractWithSkill(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && this._playerSkills.Length >=2 )
            {
                if(_cooldownSkillManager.GetCooldownSkill(_playerSkills[1]) > 0) return;

                InteractWithSkill(1);
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha3) && this._playerSkills.Length >= 3 )
            {
                if(_cooldownSkillManager.GetCooldownSkill(_playerSkills[2]) > 0) return;

                InteractWithSkill(2);
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha4) && this._playerSkills.Length >= 4 )
            {
                if(_cooldownSkillManager.GetCooldownSkill(_playerSkills[3]) > 0) return;

                InteractWithSkill(3);
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha5) && this._playerSkills.Length >= 5 )
            {
                if(_cooldownSkillManager.GetCooldownSkill(_playerSkills[4]) > 0) return;

                InteractWithSkill(4);
            }
        
        }

        private void InteractWithSkill(int index)
        {
            if(_cooldownSkillManager.GetCooldownSkill(_playerSkills[index]) > 0) return;
        
            CastingSkillOnIndex(_playerSkills[index]);
        }

        private void CastingSkillOnIndex(Skill skill)
        {
            skill.CastSkill(gameObject);
        }

    
        public void SetPlayerSkills(Skill[] weaponSkills)
        {
            _playerSkills = weaponSkills;
            OnSkillsChanged?.Invoke();
        }

        public void SetNewLevelSkill(Skill skill)
        {
            for (int i = 0; i < _playerSkills.Length; i++)
            {
                if (_playerSkills[i] == skill)
                    _playerSkills[i] = _playerSkills[i].GetNextLevelSkill;
            }
            
            OnSkillsChanged?.Invoke();
        }
   
    }
}
