using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiBaseState {
    protected bool _isRootState = false;
    protected AiStateMachine _context;
    protected AiStateFactory _statesFactory;
    protected AiBaseState _currentSubState;
    protected AiBaseState _currentSuperState;
    
    public AiBaseState(AiStateMachine currentContext, AiStateFactory aiStateFactory) {
        _context = currentContext;
        _statesFactory = aiStateFactory; 
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();

    #region State Managers
    public void UpdateStates() {
        UpdateState();
        if (_currentSubState != null) {            
           _currentSubState.UpdateStates();
        }
    }

    public void ExitStates() {
        ExitState();
        if (_currentSubState != null) {
            _currentSubState.ExitStates();
        }
    }

    protected void SwitchState(AiBaseState newState) {
        ExitState();

        newState.EnterState();

        if (_isRootState)
            _context.CurrentState = newState;
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState); 
    }

    protected void SetSuperState(AiBaseState newSuperState) {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(AiBaseState newSubState) {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
    #endregion
}
