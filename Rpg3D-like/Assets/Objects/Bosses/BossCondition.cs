using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Controller;
using UnityEngine;
using UnityEngine.AI;

public class BossCondition : Conditional
{
    protected PlayerController _playerController;
    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    protected Rigidbody _rigidbody;
    protected Movement _movement;

    public override void OnAwake()
    {
        _playerController = gameObject.GetComponent<PlayerController>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _animator = gameObject.GetComponent<Animator>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _movement = gameObject.GetComponent<Movement>();
    }
}
