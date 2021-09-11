using System;
using Controller;
using Scriptable.Weapon.SkillsSpecification;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PlayerBars.SkillBar
{
    public class SkillBox : MonoBehaviour
    {
        [SerializeField] private Image _skillImage;
        [SerializeField] private Image _skillCooldownImage;
        [SerializeField] private Skill _skill;
        public Skill GetSkill => _skill;
        [SerializeField] private int _skillIndex;

        private PlayerController _player;
        private Sprite _defaultSprite;
        private PlayerSkills _playerSkills;
        private bool _wasCasted;
        private float _skillCooldown;
        
        private void Awake()
        {
            _defaultSprite = _skillImage.sprite;
            _player = FindObjectOfType<PlayerController>();
            _playerSkills = _player.GetComponent<PlayerSkills>();
            _playerSkills.OnSkillsChanged += Start;
        }

        private void Start()
        {
            ResetBox();

            if (_playerSkills.GetPlayerSkills.Length <= _skillIndex ||
                _playerSkills.GetPlayerSkills[_skillIndex] == null) return;
            
            _skill = _playerSkills.GetPlayerSkills[_skillIndex];
            _skillImage.sprite = _skill.GetSkillSprite;
                
            _skill.OnSkillCasted += () =>
            {
                _skillCooldown = _skill.GetCooldown;
                _skillCooldownImage.fillAmount = 1;
                _wasCasted = true;
            };
        }

        private void ResetBox()
        {
            _skillImage.sprite = _defaultSprite;
            _skill = null;
        }

        private void Update()
        {
            if (!_wasCasted) return;
            
            _skillCooldown -= Time.deltaTime;
            _skillCooldownImage.fillAmount = _skillCooldown / _skill.GetCooldown; 
                
            if (_skillCooldown <= 0)
            {
                _wasCasted = false;
            }
        }
    }
}
