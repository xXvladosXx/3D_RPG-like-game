using System.Collections.Generic;
using StateMachine.BaseStates;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Enemy
{
    public class IdleEnemyState : IdleBaseState
    {
        [SerializeField] private Transform _pathToPatrol;
        [SerializeField] private StateDistanceConfiguration stateDistanceConfiguration;

        private List<Transform> _pointsToPatrol = new List<Transform>();
        private Vector3 _defaultStartPoint;
        private int _currentPointIndex = 0;
        private bool _aggredByDamage;
        protected override void Awake()
        {
            base.Awake();

            foreach (Transform child in _pathToPatrol)
            {
                _pointsToPatrol.Add(child);
            }
            _defaultStartPoint = transform.position;
            _health.OnTakeDamage += o => _aggredByDamage = true;
        }

        public override void RunState()
        {
            if (_health.IsDead()) return;
            _isAggred = stateDistanceConfiguration.IsInRange(_player, _health, _aggredByDamage 
                ? stateDistanceConfiguration.DamageChaseDistance 
                : stateDistanceConfiguration.ChaseDistance);   

            if (_isAggred)
            {
                var nState = _stateSwitcher.SwitchState<ChaseEnemyState>();
                nState.TriggeredByDamage = _aggredByDamage;
                _aggredByDamage = false;
            }
            else
            {
                GoToNextWaypoint();
            }
        }
        
        private void GoToNextWaypoint()
        {
            if (_pointsToPatrol.Count == 0)
            {
                _movement.StartMoveToAction(_defaultStartPoint, 0.4f);
                return;
            }
            
            _movement.StartMoveToAction(_pointsToPatrol[_currentPointIndex].position, 0.4f);

            if (Vector3Int.RoundToInt((_pointsToPatrol[_currentPointIndex].position)) == Vector3Int.RoundToInt((transform.position)))
            {
                _currentPointIndex = (_currentPointIndex + 1) % _pointsToPatrol.Count;
            }
        
        }
    }
}