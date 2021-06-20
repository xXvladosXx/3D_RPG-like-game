using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour, IAction
{
    [SerializeField] private float _maxSpeed = 6f;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private ActionScheduler _actionScheduler;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _actionScheduler = GetComponent<ActionScheduler>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = _navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        float speed = localVelocity.z;
        
        _animator.SetFloat("speed", speed);
    }

    public void StartMoveToAction(Vector3 destination, float speedFraction)
    {
        _actionScheduler.Cancel();
        _actionScheduler.StartAction(this);
        
        MoveTo(destination, speedFraction);
    }
    
    public void MoveTo(Vector3 destination, float speedFraction)
    {
        _navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
        _navMeshAgent.destination = destination;
        _navMeshAgent.isStopped = false;    
    }

    public void Cancel()
    {
        _navMeshAgent.speed = 0f;
        _navMeshAgent.isStopped = true;
    }

}
