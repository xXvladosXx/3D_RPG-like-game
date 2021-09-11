using System;
using System.Collections;
using Controller;
using DefaultNamespace.Objects.Bosses;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Targeting
{
    [CreateAssetMenu(fileName = "PointTargeting", menuName = "Abilities/Targeting/PointTargeting", order = 0)]
    public class DirectionTargeting : TargetingStrategy
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _groundOffset = 1;
        
        private PlayerController _playerController;
        private BossSkills _bossSkills;

        public override void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack)
        {
            _playerController = skillData.GetUser.GetComponent<PlayerController>();

            if (_playerController == null)
            {
                _bossSkills = skillData.GetUser.GetComponent<BossSkills>();
                skillData.SetMousePosition( FindObjectOfType<PlayerController>().gameObject.transform.position);
        
                _bossSkills.StartCoroutine(Targeting(finishedAttack));
                
                return;
            }
        
            RaycastHit raycastHit;
            Ray ray = PlayerController.GetRay();
        
            if (Physics.Raycast(ray, out raycastHit, 1000, _layerMask))
            {
                skillData.SetMousePosition(raycastHit.point + ray.direction * _groundOffset /ray.direction.y);
            }
        
        
            _playerController.StartCoroutine(Targeting(finishedAttack));
        }

        private IEnumerator Targeting(Action finishedAttack)
        {
            yield return new WaitForSeconds(0.1f);
            finishedAttack();
        }
    }
}
