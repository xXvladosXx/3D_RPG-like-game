using System;
using Controller;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using UI.PlayerBars.HealthBar;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Effects
{

    [CreateAssetMenu(fileName = "Companion Spawning", menuName = "Abilities/Core/Companion", order = 0)]
    public class SpawnCompanionEffect : EffectStrategy
    {
        [SerializeField] private FriendlyAIController _companionToSpawn;
        [SerializeField] private GameObject _spawnEffect;
        [SerializeField] private int _maxNumberOfCompanions = 1;

        private GameObject _companionBar;
        private FriendlyAIController _spawnedCompanion;
        private CompanionChecker _companionChecker;

        public override void Effect(SkillData skillData, Action finished)
        {
            SingleProjectileAttack(skillData);
          
            finished();
        }

        public override void SetData(DataCollector dataCollector)
        {
            dataCollector.AddDataFromNewLine(_companionToSpawn.name);
        }

        private void SingleProjectileAttack(SkillData skillData)
        {
            _companionBar = GameObject.FindWithTag("CompanionBar");

            _companionChecker = skillData.GetUser.GetComponent<CompanionChecker>();
            if(!_companionChecker.HasCompanion())
            {
                GameObject particleSystem = Instantiate(_spawnEffect, skillData.GetMousePosition,
                    UnityEngine.Quaternion.identity);

                _spawnedCompanion = Instantiate(_companionToSpawn, skillData.GetMousePosition,
                    UnityEngine.Quaternion.identity);
                
                HealthBarCompanion healthBarCompanion = _companionBar.GetComponent<HealthBarCompanion>();
                healthBarCompanion.enabled = true;
            
                foreach (Transform child in _companionBar.transform)
                {
                    child.gameObject.SetActive(true);
                }
                
                _companionChecker.SetCompanion(_spawnedCompanion);
                    
                Destroy(particleSystem, 1f);
            }
            else
            {
                _companionChecker.DestroyCompanion();
                
                HealthBarCompanion healthBarCompanion = _companionBar.GetComponent<HealthBarCompanion>();
                healthBarCompanion.enabled = false;
            
                foreach (Transform child in _companionBar.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
            

        }

        
    }
}