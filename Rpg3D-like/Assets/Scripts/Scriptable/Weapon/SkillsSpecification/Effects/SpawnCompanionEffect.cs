using System;
using UI.PlayerBars.HealthBar;
using UnityEngine;

namespace Scriptable.Weapon
{

    [CreateAssetMenu(fileName = "Companion Spawning", menuName = "Abilities/SpellCastingCompanion", order = 0)]
    public class SpawnCompanionEffect : EffectStrategy
    {
        [SerializeField] private FriendlyAIController _companionToSpawn;
        [SerializeField] private GameObject _spawnEffect;
        [SerializeField] private int _maxNumberOfCompanions = 1;

        private bool _hasCompanion = false;
        private GameObject _companionBar;
        private FriendlyAIController _spawnedCompanion;
        public override void Effect(SkillData skillData, Action finished)
        {
            SingleProjectileAttack(skillData);
          
            finished();
        }

        private void SingleProjectileAttack(SkillData skillData)
        {
            
            if(_spawnedCompanion == null)
            {
                _hasCompanion = false;

                if (_hasCompanion)
                {
                    Destroy(_spawnedCompanion.gameObject);
                    _spawnedCompanion = Instantiate(_companionToSpawn, skillData.GetMousePosition,
                        UnityEngine.Quaternion.identity);
                }
                
               
                GameObject particleSystem = Instantiate(_spawnEffect, skillData.GetMousePosition,
                    UnityEngine.Quaternion.identity);

                _spawnedCompanion = Instantiate(_companionToSpawn, skillData.GetMousePosition,
                    UnityEngine.Quaternion.identity);
                
                _hasCompanion = true;
                
                _companionBar = GameObject.FindWithTag("CompanionBar");
                HealthBarCompanion healthBarCompanion = _companionBar.GetComponent<HealthBarCompanion>();
                healthBarCompanion.enabled = true;
            
                foreach (Transform child in _companionBar.transform)
                {
                    child.gameObject.SetActive(true);
                }

                Destroy(particleSystem, 1f);
            }
            else
            {
                Destroy(_spawnedCompanion.gameObject);
                
                _companionBar = GameObject.FindWithTag("CompanionBar");
                HealthBarCompanion healthBarCompanion = _companionBar.GetComponent<HealthBarCompanion>();
                healthBarCompanion.enabled = false;
            
                foreach (Transform child in _companionBar.transform)
                {
                    child.gameObject.SetActive(false);
                }
                
                _hasCompanion = false;
            }
            

        }

        
    }
}