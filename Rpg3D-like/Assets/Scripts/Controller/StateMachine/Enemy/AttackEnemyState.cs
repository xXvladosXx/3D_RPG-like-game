using StateMachine.BaseStates;
using Stats;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class AttackEnemyState : AttackBaseState
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
            if(_health.IsDead()) return;
            
            if (_target.IsDead() || !stateDistanceConfiguration.IsInRange(_player, _health, _equipment.GetCurrentWeapon.GetAttackRange))
            {
                _stateSwitcher.SwitchState<ChaseEnemyState>();
            }
            else
            {
                _combat.Attack(_target.transform);
            }   
        }
        
    }
}