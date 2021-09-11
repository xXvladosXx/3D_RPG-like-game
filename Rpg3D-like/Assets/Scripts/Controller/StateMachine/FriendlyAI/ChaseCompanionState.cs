
using StateMachine.BaseStates;
using Stats;
using UnityEngine;

namespace StateMachine.FriendlyAI
{
    public class ChaseCompanionState : ChaseBaseState
    {
        protected override void Awake()
        {
            base.Awake();

            _player.GetComponent<Health>().OnTakeDamage += damager =>
            {
                _target = damager.transform.GetComponent<Health>();
            };
        }

        public override void RunState()
        {
            if(_health.IsDead()) return;
            if(_target == null) return;

            _movement.MoveTo(_target.transform.position, 0.8f);
        
            if (IsInRange() && !_target.IsDead())
            {
                _stateSwitcher.SwitchState<AttackCompanionState>();
            }
            else
            {
                if (!_target.IsDead())
                {
                    _movement.StartMoveToAction(_target.transform.position, 1f);
                    return;
                }

                _stateSwitcher.SwitchState<IdleCompanionState>();
            }

        }

        private bool IsInRange()
        {
            return Vector3.Distance(gameObject.transform.position, _target.transform.position) < _equipment.GetCurrentWeapon.GetAttackRange;
        }
    }
}