using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable.Weapon;
using UnityEngine;

[CreateAssetMenu(fileName = "PointTargeting", menuName = "Abilities/PointTargeting", order = 0)]
public class DirectionTargeting : TargetingStrategy
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _groundOffset = 1;
    private PlayerController _playerController;

    public override void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack)
    {
        _playerController = skillData.GetUser.GetComponent<PlayerController>();
        
        RaycastHit raycastHit;
        Ray ray = PlayerController.GetRay();
        
        if (Physics.Raycast(ray, out raycastHit, 1000, _layerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction*1000, Color.green, 10f);
            skillData.SetMousePosition(raycastHit.point + ray.direction * _groundOffset /ray.direction.y);
        }
        
        
        _playerController.StartCoroutine(Targeting(skillData, _playerController, finishedAttack));
    }

    private IEnumerator Targeting(SkillData skillData, PlayerController playerController, Action finishedAttack)
    {
        yield return new WaitForSeconds(0.1f);
        finishedAttack();
    }
}
