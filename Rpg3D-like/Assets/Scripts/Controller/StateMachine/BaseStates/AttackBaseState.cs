using Controller;
using Stats;
using UnityEngine;

namespace StateMachine.BaseStates
{
    public abstract class AttackBaseState : BaseState
    {
        protected Health _target;
        protected Combat _combat;
        protected IStateSwitcher _stateSwitcher;
        protected Health _health;
        protected PlayerController _player;
        protected Equipment _equipment;
    
        protected virtual void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
            _combat = gameObject.GetComponentInParent<Combat>();
            _stateSwitcher = GetComponent<IStateSwitcher>();
            _health = gameObject.GetComponentInParent<Health>();
            _equipment = GetComponentInParent<Equipment>();
        }
    }
}
