using System;
using Controller;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using Stats;
using UI.PlayerBars.SkillBar;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification
{
    [CreateAssetMenu(fileName = "SkillManager", menuName = "Abilities/Skill", order = 0)]
    public class Skill : ScriptableObject, IAction
    {
        [SerializeField] private float _cooldown;
        public float GetCooldown => _cooldown;
        [SerializeField] private int _manaCost;
        [SerializeField] private Sprite _image;
        public Sprite GetSkillSprite => _image;

        [SerializeField] private Skill _nextLevelSkill;
        [SerializeField] private Skill[] _previousSkills;
        public Skill GetNextLevelSkill => _nextLevelSkill;
        [SerializeField] private FilterStrategy[] _filterStrategies;
        [SerializeField] private EffectStrategy[] _effectStrategies;
        public EffectStrategy[] GetStrategies => _effectStrategies;
        [SerializeField] private TargetingStrategy _targetingStrategy;

        private GameObject _caster;
        private Animator _animator;
        private CooldownSkillManager _cooldownSkillManager;
        private SkillData _skillData;
        private DataCollector _dataCollector;
        public event Action OnSkillInteract;
        public event Action OnSkillCasted;

        public string GetDataCollector()
        {
            _dataCollector = new DataCollector("Mana cost " +_manaCost.ToString());
            _dataCollector.AddDataInSameLine(_nextLevelSkill._manaCost.ToString());

            _dataCollector.AddDataFromNewLine("Cooldown " + _cooldown.ToString());
            _dataCollector.AddDataInSameLine(_nextLevelSkill._cooldown.ToString());

            foreach (var effectStrategy in _effectStrategies)
            {
                effectStrategy.SetData(_dataCollector);
            }

            return _dataCollector.SkillData.ToString();
        }
        public void CastSkill(GameObject user)
        {
            Mana mana = user.GetComponent<Mana>();

            if (mana.GetCurrentMana() < _manaCost)
            {
                OnSkillInteract?.Invoke();
                return;
            }

            if (_animator == null)
            {
                _animator = user.GetComponent<Animator>();
            }

            if (_targetingStrategy == null)
                return;

            _cooldownSkillManager = user.GetComponent<CooldownSkillManager>();
            
            if (_cooldownSkillManager == null) return;
            
            if (_cooldownSkillManager.GetCooldownSkill(this) > 0) return;
               

            _skillData = new SkillData(user);
            
            ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();

            actionScheduler.StartAction(_skillData);

            _targetingStrategy.StartTargeting(_skillData, () => { AquireTarget(_skillData, mana); }, Cancel);
        }

        private void AquireTarget(SkillData skillData, Mana mana)
        {
            OnSkillInteract?.Invoke();

            if (skillData.IsCancelled) return;

            mana.GetComponent<Mana>().CasteSkill(_manaCost);

            _cooldownSkillManager.StartCooldown(this, _cooldown);

            foreach (var filterStrategy in _filterStrategies)
            {
                skillData.SetTargets(filterStrategy.Filter(skillData.GetTargets));
            }

            foreach (var attackTarget in _effectStrategies)
            {
                attackTarget.Effect(skillData, EffectFinished);
            }
            
            OnSkillCasted?.Invoke();
        }

        private void EffectFinished()
        {
            if (_previousSkills.Length <= 0) return;
            foreach (var skill in _previousSkills)
            {
                //An ability to cast previous level skill
                skill.CastSkill(FindObjectOfType<PlayerController>().gameObject);
            }
        }

        public void Cancel()
        {
            OnSkillInteract?.Invoke();
        }
    }
}