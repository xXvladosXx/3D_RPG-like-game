using StateMachine.BaseStates;
using Stats;
using UnityEngine;

namespace StateMachine.FriendlyAI
{
    public class AttackCompanionState : AttackBaseState, ISwitchListener
    {
        public override void RunState()
        {
            if(_health.IsDead()) return;

            print(_target);
            if (_target == null || _target.IsDead() || Vector3.Distance(_target.transform.position, transform.position) > _equipment.GetCurrentWeapon.GetAttackRange)
            {
                _stateSwitcher.SwitchState<ChaseCompanionState>();
            }
            else
            {
                transform.LookAt(_target.transform);
                _combat.Attack(_target.transform);
            }
        }

        public void OnSwitch()
        {
            if(_target != null && !_target.IsDead()) return;
            
            CombatTarget[] targets = FindObjectsOfType<CombatTarget>();
            float minDistance = 30f;

            foreach (CombatTarget target in targets)
            {
                float distanceToTarget = Vector3.Distance(_combat.gameObject.transform.position, target.transform.position);

                if (!(minDistance > distanceToTarget)) continue;
            
                minDistance = distanceToTarget;
                _target = target.GetComponent<Health>();
            }
        }
    }
}