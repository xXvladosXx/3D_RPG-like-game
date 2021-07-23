using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class IdleBaseState : BaseState
{
    private PlayerController _player;
    private Movement _movement;
    private IFriendlyAIStateSwitcher _stateSwitcher;
    private bool _isAggred = false;

    private void Awake()
    {
        _movement = FindObjectOfType<FriendlyAIController>().GetComponent<Movement>();
        _stateSwitcher = GetComponent<IFriendlyAIStateSwitcher>();
        _player = FindObjectOfType<PlayerController>();

        _player.OnEnemyAttacked += enemy => { _isAggred = !enemy.GetComponent<Health>().IsDead(); };

        _player.GetComponent<Health>().OnTakeDamage += damager =>
        {
            _isAggred = true;
        };
    }
    
    public override void RunState()
    {
        if(FindObjectOfType<FriendlyAIController>().GetComponent<Health>().IsDead()) return;
        
        if (_isAggred)
        {
            _stateSwitcher.SwitchState<ChaseBaseState>();
        }
        else
        {
            _movement.MoveTo(_player.transform.position, 1f);
        }
        _isAggred = false;
    }
}
    

    
