using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StateMachine;
using UnityEngine;

public class StateManager : MonoBehaviour, IFriendlyAIStateSwitcher
{
    [SerializeField] private FriendlyAIController _friendlyAIController;
    
    private BaseState _currentBaseState;
    private List<BaseState> _allStates;

    private void Start()
    {
        _allStates = new List<BaseState>
        {
            gameObject.AddComponent<IdleBaseState>(),
            gameObject.AddComponent<ChaseBaseState>(),
            gameObject.AddComponent<AttackBaseState>()
        };
        
        _currentBaseState = _allStates[0];
    }

    private void Update()
    {
        _currentBaseState.RunState();
    }

    public void SwitchState<T>() where T : BaseState
    {
        var state = _allStates.FirstOrDefault(s => s is T);
        _currentBaseState = state;
    }
}
