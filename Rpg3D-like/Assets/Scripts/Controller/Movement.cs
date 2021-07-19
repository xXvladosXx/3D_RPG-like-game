using System;
using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour, IAction, ISaveable
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
        _navMeshAgent.isStopped = true;
    }

    public object CaptureState()
    {
        return new SerializableVector(transform.position);
    }

    public void RestoreState(object state)
    {
         SerializableVector position =(SerializableVector)state;

         _navMeshAgent.enabled = false;
         transform.position = position.ToVector();
         _navMeshAgent.enabled = true;
        
         GetComponent<ActionScheduler>().Cancel();

    }
}
