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
    public override void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack)
    {
        RaycastHit raycastHit;
        Ray ray = PlayerController.GetRay();

        if (Physics.Raycast(ray, out raycastHit, 1000, _layerMask))
        {
            skillData.SetMousePosition(raycastHit.point + ray.direction * _groundOffset/ ray.direction.y);
        }
        
        finishedAttack();
    }
}
