using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FriendlyAIController : MonoBehaviour
{
    [SerializeField] private float _chaseDistance = 10f;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Transform[] _pointsToPatrol;
    [SerializeField] private float _waitingTime = 2f;
    [SerializeField] private float _tolerants = 1f;
    
    private FindStat _statFinder;
    private GameObject _player;
    private NavMeshAgent _navMeshAgent;
    private Combat _combat;
    private Movement _movement;
    private Health _health;
    private float _timeSinceVisitedPointl = Mathf.Infinity;
    private int _currentWaypointIndex = 0;
    private bool _wasTriggered = false;
    private Transform _target;
    private void Awake()
    {
        _statFinder = GetComponent<FindStat>();
        _combat = GetComponent<Combat>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _movement = GetComponent<Movement>();
        _health = GetComponent<Health>();
        _player = GameObject.FindWithTag("Player");

        _health.OnTakeDamage += TriggerAttackDamager;
        _player.GetComponent<PlayerController>().OnEnemyAttacked += SetTarget;
    }

    private void SetTarget()
    {
        _target = _player.GetComponent<Combat>().GetTarget;
        TriggerAttackDamager();
    }

    private void TriggerAttackDamager()
    {
        _wasTriggered = true;
    }

    void Update()
    {
        if(_health.IsDead()) return;
        
        if(!_wasTriggered)
        {
            _movement.StartMoveToAction(_player.transform.position, 1f);
        }
        else if((_combat.CanAttack(_target.gameObject) && _wasTriggered ))
        {
            if (_target.GetComponent<Health>().IsDead())
            {
                _movement.StartMoveToAction(_player.transform.position, 1f);
            }
            _combat.Attack(_target);
        }else if(!_combat.CanAttack(_target.gameObject))
        {
            _movement.StartMoveToAction(_player.transform.position, 1f);
        }
        

        _timeSinceVisitedPointl += Time.deltaTime;
    }
}
