using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class ChaseBaseState : BaseState
{
    private float _attackRange = 10f;
    [SerializeField] private Transform _target;
    
    private Movement _movement;
    private IFriendlyAIStateSwitcher _stateSwitcher;
    private PlayerController _player;

    private void Awake()
    {
        _movement = FindObjectOfType<FriendlyAIController>().GetComponent<Movement>();
        _stateSwitcher = GetComponent<IFriendlyAIStateSwitcher>();
        _player = FindObjectOfType<PlayerController>();

        
        _player.GetComponent<Health>().OnTakeDamage += damager =>
        {
            _target = damager.transform;
        };
    }

    public override void RunState()
    {
        if(FindObjectOfType<FriendlyAIController>().GetComponent<Health>().IsDead()) return;
        
        _movement.MoveTo(_target.position, 1f);
        
        if (IsInRange())
        {
            _stateSwitcher.SwitchState<AttackBaseState>();
        }
        else
        {
            _stateSwitcher.SwitchState<IdleBaseState>();
        }

    }

    private bool IsInRange()
    {
        if (_target == null || _target.GetComponent<Health>().IsDead()) return false;
        
        return Vector3.Distance(gameObject.transform.position, _target.transform.position) < _attackRange;
    }
}
