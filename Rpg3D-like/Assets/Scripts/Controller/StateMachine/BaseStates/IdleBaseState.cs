using Controller;
using Stats;

namespace StateMachine.BaseStates
{
    public abstract class IdleBaseState : BaseState
    {
        protected PlayerController _player;
        protected Movement _movement;
        protected Health _health;
        protected IStateSwitcher _stateSwitcher;
        protected bool _isAggred = false;

        protected virtual void Awake()
        {
            _movement = gameObject.GetComponentInParent<Movement>();
            _stateSwitcher = GetComponent<IStateSwitcher>();
            _player = FindObjectOfType<PlayerController>();
            _health = gameObject.GetComponentInParent<Health>();
        }
    }
}
    

    
