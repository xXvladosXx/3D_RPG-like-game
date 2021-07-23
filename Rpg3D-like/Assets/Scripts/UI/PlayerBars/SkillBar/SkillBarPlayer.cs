using System;
using System.Collections.Generic;
using Controller;
using Scriptable.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SkillBar
{
    public class SkillBarPlayer : MonoBehaviour
    {
        [SerializeField] private Sprite _sprite;

        private Dictionary<int, GameObject> _childSkillsList;
        
        private PlayerSkills _playerSkills;
        private int _skillIndex = 0;
        private void Awake()
        {
            _playerSkills = FindObjectOfType<PlayerSkills>();
            _childSkillsList = new Dictionary<int, GameObject>();

            foreach (Transform child in transform)
            {
                _childSkillsList.Add(_skillIndex, child.gameObject);
                _skillIndex++;
            }

            _playerSkills.OnSkillsChanged += () =>
            {
                foreach (var skillBox in _childSkillsList.Values)
                {
                    skillBox.GetComponent<Image>().sprite = _sprite;
                }
                
                _skillIndex = 0;
                foreach (var skill in _playerSkills.GetPlayerSkills)
                {
                    var skillGetSkillSprite = _childSkillsList[_skillIndex].GetComponent<Image>();
                    skillGetSkillSprite.sprite = skill.GetSkillSprite;
                    _skillIndex++;
                }
            };
        }

        
        public void TriggerCastingSkill(int index)
        {
            foreach (var skillBox in _childSkillsList)
            {
                if (skillBox.Key == index)
                {
                    skillBox.Value.GetComponent<SkillBox>().SetCasted(true);
                }
            }
        }
    }
}