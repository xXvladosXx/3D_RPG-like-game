using System.Collections.Generic;
using System.Linq;
using StateMachine.BaseStates;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class EnemyStateManager: MonoBehaviour, IStateSwitcher
    {
        [SerializeField] private List<BaseState> _allStates;

        private BaseState _currentBaseState;

        private void Start()
        {
            foreach (var state in GetComponents<BaseState>())
            {
                _allStates.Add(state);
            }
            
            _currentBaseState = _allStates[2];
        }

        private void Update()
        {
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
            return (T) state;
        }
    }

}