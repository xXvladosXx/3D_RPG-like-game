using StateMachine.BaseStates;
using Stats;

namespace StateMachine.FriendlyAI
{
    public class IdleCompanionState : IdleBaseState
    {
        protected override void Awake()
        {
            base.Awake();

            _player.OnEnemyAttacked += enemy => { _isAggred = !enemy.GetComponent<Health>().IsDead(); };

            _player.GetComponent<Health>().OnTakeDamage += damager => { _isAggred = true; };
        }

        public override void RunState()
        {
            if (_health.IsDead()) return;

            if (_isAggred)
            {
                _stateSwitcher.SwitchState<ChaseCompanionState>();
            }
            else
            {
                _movement.MoveTo(_player.transform.position, 1f);
            }

            _isAggred = false;
        }
    }
}