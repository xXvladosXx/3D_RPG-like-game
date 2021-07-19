using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    [SerializeField] private float _chaseDistance = 10f;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Transform _pathToPatrol;

    [SerializeField] private float _waitingTime = 2f;
    [SerializeField] private float _tolerants = 1f;
    
    private FindStat _statFinder;
    private GameObject _player;
    private List<Transform> _listPathToPatrol;
    private NavMeshAgent _navMeshAgent;
    private Combat _combat;
    private Movement _movement;
    private Health _health;
    private float _timeSinceVisitedPointl = Mathf.Infinity;
    private int _currentWaypointIndex = 0;
    private bool _wasTriggered = false;
    private void Awake()
    {
        _listPathToPatrol = new List<Transform>();
        
        _statFinder = GetComponent<FindStat>();
        _combat = GetComponent<Combat>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _movement = GetComponent<Movement>();
        _health = GetComponent<Health>();
        _player = GameObject.FindWithTag("Player");
        _health.OnTakeDamage += TriggerAttackDamager;

        foreach (Transform child in _pathToPatrol)
        {
            _listPathToPatrol.Add(child);
        }
    }

    private void TriggerAttackDamager()
    {
        _wasTriggered = true;
    }

    void Update()
    {
        if (_health.IsDead())
            return;
        
        if ((IsInRangeTo(_chaseDistance) && _combat.CanAttack(_player)) || _wasTriggered )
        {
           _combat.Attack(_player.transform);
        }
        else if(!_wasTriggered)
        {
            if(_navMeshAgent.remainingDistance < 0.5f);
            {
                GoToNextWaypoint();
            }
        }

        _timeSinceVisitedPointl += Time.deltaTime;
    }

    private void GoToNextWaypoint()
    {
        _movement.StartMoveToAction(_listPathToPatrol[_currentWaypointIndex].position, 0.6f);

        if (Vector3Int.RoundToInt((_listPathToPatrol[_currentWaypointIndex].position)) == Vector3Int.RoundToInt((transform.position)))
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _listPathToPatrol.Count;
        }
        
    }

    private bool IsInRangeTo(float distance)
    {
        return Vector3.Distance(_player.transform.position, gameObject.transform.position) < distance;
    }
}
