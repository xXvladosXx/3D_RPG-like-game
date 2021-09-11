using System;
using System.Collections;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using UnityEngine;


namespace Scriptable.Weapon.SkillsSpecification.Effects
{
    [CreateAssetMenu(fileName = "UnderEarthProjectile Spawning", menuName = "Abilities/Core/ThrowerEffect", order = 0)]

    public class ThrowInstantiator : EffectStrategy
    {
        [SerializeField] private ThrowSkill _throwSkill;
        [SerializeField] private int _maxLenght;
        [SerializeField] private float _separation;
        [SerializeField] private LayerMask _layer;
        [SerializeField] private float _timeToSpawn;

        private SkillData _skillData;
        private bool _hasSpawnedNext = false;
        private Vector3 _nextPositionToSpawn;
        private int _currentThrower = 1;
        private Quaternion _quaternion;
    
        public override void Effect(SkillData skillData, Action finished)
        {
            _skillData = skillData;
            _nextPositionToSpawn = new Vector3(_skillData.GetMousePosition.x, _skillData.GetMousePosition.y, _skillData.GetMousePosition.z);
            _quaternion = skillData.GetUser.transform.rotation;
            _hasSpawnedNext = false;
            _currentThrower = 1;
            
            
            if (!_hasSpawnedNext)
            {
                CreateThrowSkill(finished);
            }
        }

        public override void SetData(DataCollector dataCollector)
        {
            dataCollector.AddDataFromNewLine("Effects " + _maxLenght);
        }

        private void CreateThrowSkill(Action finished)
        {
            if (_currentThrower > _maxLenght)
            {
                finished();
                return;
            }
            
            var currentPosition = _nextPositionToSpawn;
            _hasSpawnedNext = true;
            var imple = Instantiate(_throwSkill,
                _nextPositionToSpawn,
                _quaternion);
            _nextPositionToSpawn = currentPosition + (imple.transform.forward * _separation);

            _currentThrower++;

            _skillData.StartCoroutine(TimeToSpawnNextImple(finished));
        }

        private IEnumerator TimeToSpawnNextImple(Action finised)
        {
            yield return new WaitForSeconds(_timeToSpawn);
            _hasSpawnedNext = false;
            CreateThrowSkill(finised);
        }

    
    }
}