using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StateMachine;
using UnityEngine;

public class AttackBaseState : BaseState
{
    [SerializeField] private Transform _target;
    private Combat _combat;
    private IFriendlyAIStateSwitcher _stateSwitcher;
    private void Awake()
    {
        _combat = FindObjectOfType<FriendlyAIController>().GetComponent<Combat>();
        _stateSwitcher = GetComponent<IFriendlyAIStateSwitcher>();

        CombatTarget[] targets = FindObjectsOfType<CombatTarget>();
        float minDistance = 30f;

        foreach (CombatTarget target in targets)
        {
            float distanceToTarget = Vector3.Distance(_combat.gameObject.transform.position, target.transform.position);

            if (minDistance > distanceToTarget)
            {
                minDistance = distanceToTarget;
                _target = target.transform;
            }
        }
    }

    public override void RunState()
    {
        if(FindObjectOfType<FriendlyAIController>().GetComponent<Health>().IsDead()) return;
        

        if (_target == null || _target.GetComponent<Health>().IsDead())
        {
            _stateSwitcher.SwitchState<ChaseBaseState>();
        }
        else
        {
            _combat.Attack(_target);
        }
    }
}
