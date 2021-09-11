using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;


public class DefaultAttack : BossAction
{
    public string AnimationTriggerName;
    public float BuildUpTime;
    public float AvoidTime;
    
    private bool _hasPlayed = false;
    private Tween _buildupTween;
    private Tween _avoidTween;
    private Vector3 _randomPoint;
    private float _radius = 5f;

    public override void OnStart()
    {
        _buildupTween = DOVirtual.DelayedCall(BuildUpTime, StartAvoid, false);
        
        _animator.SetTrigger(AnimationTriggerName);
    }

    private void StartAvoid()
    {
        _equipment.GetCurrentWeapon.SpawnProjectile(_playerController.transform, transform, _equipment.GetCurrentWeapon._damage);
        
        _avoidTween = DOVirtual.DelayedCall(AvoidTime, () =>
        {
            _hasPlayed = true;
        }, false);
    }

    public override TaskStatus OnUpdate()
    {
        if(_health.IsDead()) return TaskStatus.Failure;

        transform.LookAt(_playerController.transform);
        
        return _hasPlayed ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        _buildupTween?.Kill();
        _avoidTween?.Kill();
        _hasPlayed = false;
    }
    
    
}
