using System;
using System.Collections.Generic;
using Controller;
using DefaultNamespace.Objects.Bosses;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Targeting
{
    [CreateAssetMenu(fileName = "TargetingSkillBoss", menuName = "Abilities/Targeting/TargetingSkillBoss", order = 0)]
    public class TargetingEnemy : TargetingStrategy, IAction
    {
        [SerializeField] private float _skillRadius;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _distanceToCastSkill;
        [SerializeField] private Transform _skillRendererRadius;
        
        private PlayerController _playerController;
        private Transform _skillRendererRadiusInstance;

        public override void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack)
        {
            _playerController = FindObjectOfType<PlayerController>();
            var playerPosition = _playerController.transform.position;
            
            if (_skillRendererRadius != null)
            {
                if (_skillRendererRadiusInstance == null)
                {
                    _skillRendererRadiusInstance = Instantiate(_skillRendererRadius);
                }
                else
                {
                    _skillRendererRadiusInstance.gameObject.SetActive(true);
                }

                _skillRendererRadiusInstance.localScale = new Vector3(_skillRadius * 2, 1, _skillRadius * 2);


                _skillRendererRadiusInstance.position = playerPosition;
            }

            skillData.SetMousePosition(playerPosition);
            skillData.SetTargets(GetGameObjectsInRadius(playerPosition));
            skillData.GetUser.transform.LookAt(playerPosition);
            skillData.SetRenderer(_skillRendererRadiusInstance);
            
            finishedAttack();
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            RaycastHit[] hits = Physics.SphereCastAll(point, _skillRadius, Vector3.up, 0);
            foreach (var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }

        public void Cancel()
        {
            
        }
    }
}