using System;
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
            }
            else
            {
                Destroy(_spawnedCompanion.gameObject);
                _hasCompanion = false;
            }
            

        }

        
    }
}