using System.Collections.Generic;
using System.Linq;
using StateMachine.BaseStates;
using UnityEngine;

namespace StateMachine.FriendlyAI
{
    [RequireComponent(typeof(IdleCompanionState), typeof(ChaseCompanionState), typeof(AttackCompanionState))]
    public class CompanionStateManager : MonoBehaviour, IStateSwitcher
    {
        private BaseState _currentBaseState;
        [SerializeField] private List<BaseState> _allStates;

        private void Start()
        {
            print(GetComponent<IdleCompanionState>());
            foreach (var state in GetComponents<BaseState>())
            {
                _allStates.Add(state);
            }
            
            _currentBaseState = _allStates[0];
        }

        private void Update()
        {
            print(_currentBaseState);

            _currentBaseState.RunState();
        }

        public T SwitchState<T>() where T : BaseState
        {
            var state = _allStates.FirstOrDefault(s => s is T);
            _currentBaseState = state;
            if (state is ISwitchListener listener)
            {
                listener.OnSwitch();
            }
            return (T)state;
        }
    }
}
