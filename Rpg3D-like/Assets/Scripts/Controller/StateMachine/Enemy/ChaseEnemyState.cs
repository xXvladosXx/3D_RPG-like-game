using StateMachine.BaseStates;
using Stats;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class ChaseEnemyState : ChaseBaseState
    {
        [SerializeField] private StateDistanceConfiguration stateDistanceConfiguration;

        public bool TriggeredByDamage { get; set; }

        protected override void Awake()
        {
            base.Awake();

            _target = _player.GetComponent<Health>();
        }

        public override void RunState()
        {
            if (_health.IsDead()) return;
            if (_target == null) return;

            if (stateDistanceConfiguration.IsInRange(_player, _health,
                TriggeredByDamage
                    ? stateDistanceConfiguration.DamageChaseDistance
                    : stateDistanceConfiguration.ChaseDistance))
            {
                if (stateDistanceConfiguration.IsInRange(_player, _health, _equipment.GetCurrentWeapon.GetAttackRange))
                {
                    var nState = _stateSwitcher.SwitchState<AttackEnemyState>();
                    nState.TriggeredByDamage = TriggeredByDamage;
                }
                else
                {
                    _movement.StartMoveToAction(_player.transform.position, 0.7f);
                }
            }
            else
            {
                _movement.Cancel();
                _stateSwitcher.SwitchState<IdleEnemyState>();
                TriggeredByDamage = false;
            }
        }
    }
}