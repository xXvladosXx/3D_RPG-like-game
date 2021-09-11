using Controller;
using Stats;
using UnityEngine;

namespace StateMachine.BaseStates
{
    public abstract class ChaseBaseState : BaseState
    {
        protected Health _target;
        protected Movement _movement;
        protected IStateSwitcher _stateSwitcher;
        protected PlayerController _player;
        protected Health _health;
        protected Equipment _equipment;

        protected virtual void Awake()
        {
            _movement =  gameObject.GetComponentInParent<Movement>();
            _stateSwitcher = GetComponent<IStateSwitcher>();
            _player = FindObjectOfType<PlayerController>();
            _health = gameObject.GetComponentInParent<Health>();
            _equipment = GetComponentInParent<Equipment>();
        }
    }
}
