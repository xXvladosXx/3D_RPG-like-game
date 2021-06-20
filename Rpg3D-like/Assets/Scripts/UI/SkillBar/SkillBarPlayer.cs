using System;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SkillBar
{
    public class SkillBarPlayer : MonoBehaviour
    {
        private Dictionary<int, GameObject> _cooldownImages = new Dictionary<int, GameObject>();

        private List<int> _usedSkills = new List<int>();
        
        private CooldownSkillManager _cooldownSkillManager;
        private PlayerSkills _playerSkills;
        private void Awake()
        {
            int keyToSkill = 0;

            _cooldownSkillManager = GetComponent<CooldownSkillManager>();
            _playerSkills = GetComponent<PlayerSkills>();
            
            foreach (GameObject cooldownImage in GameObject.FindGameObjectsWithTag("Cooldown"))
            {
                _cooldownImages.Add(keyToSkill, cooldownImage);
                keyToSkill++;
            }
        }

        private void Update()
        {
            if(_playerSkills.GetPlayerSkills().Length == 0)
                return;
            
            foreach (var cooldownImage in _cooldownImages)
            {
                cooldownImage.Value.GetComponent<Image>().fillAmount =
                    _cooldownSkillManager.GetFractionOfCooldown(_playerSkills.GetPlayerSkills()[0]);
            }
        }

        public void TriggerToSetFillAmountImage(int index, Skill[] userSkills)
        {
        }

        public void FillAmountImage(int index)
        {
            _cooldownImages[index].GetComponent<Image>().fillAmount = 0;
        }
    }
}