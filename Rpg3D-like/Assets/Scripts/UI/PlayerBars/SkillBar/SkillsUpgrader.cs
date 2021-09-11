using System;
using Controller;
using Scriptable.Weapon.SkillsSpecification;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.PlayerBars.SkillBar
{
    public class SkillsUpgrader : MonoBehaviour
    {
        private Button _skillUpgrade;
        private Skill _skill;
        private PlayerSkills _playerSkills;
        private SkillBarController _skillBarController;
        private void Awake()
        {
            _skillBarController = FindObjectOfType<SkillBarController>();
            _playerSkills = FindObjectOfType<PlayerSkills>();
            _skillUpgrade = GetComponent<Button>();
            
            AddEvent(EventTriggerType.PointerEnter, delegate { OnEnter(); });
            AddEvent(EventTriggerType.PointerExit, delegate { OnExit(); });
            
            _skillUpgrade.onClick.AddListener(() => UpgradeSkill());
        }

        private void Start()
        {
            _playerSkills.OnSkillsChanged += () => {_skill = GetComponentInParent<SkillBox>().GetSkill;};
        }

        private void Update()
        {
            if (_skillBarController.GetPoints <= 0)
            {
                _skillUpgrade.interactable = false;
                return;
            }

            if (_skill == null)
            {
                _skillUpgrade.interactable = false;
                return;
            }

            if (_skill.GetNextLevelSkill == null)
            {
                _skillUpgrade.interactable = false;
                return;
            }
            
            _skillUpgrade.interactable = true;
        }

        private void UpgradeSkill()
        {
            _skillBarController.DistributePoint();
            _playerSkills.SetNewLevelSkill(_skill);
        }
        
        private void AddEvent(EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
            var eventTrigger = new EventTrigger.Entry {eventID = type};
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }
        
        private void OnEnter()
        {
            if(_skillUpgrade.interactable)
                Tooltip.EnableTooltip(_skill.name + "\n" + _skill.GetDataCollector());
        }

        private void OnExit()
        {
            Tooltip.DisableTooltip();
        }
    }
}
