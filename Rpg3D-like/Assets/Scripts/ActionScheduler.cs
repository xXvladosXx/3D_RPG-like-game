using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionScheduler : MonoBehaviour, IAction
{
    private IAction _currentAction;

    public void StartAction(IAction action)
    {
        if(_currentAction == action) return;

        if (_currentAction != null)
        {
            _currentAction.Cancel();
        }

        _currentAction = action;
    }

    public IAction GetCurrentAction()
    {
        return _currentAction;
    }
    
    public void Cancel()
    {
        StartAction(null);
    }

}
