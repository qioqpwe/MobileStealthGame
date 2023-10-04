using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class PlayerBaseState {
    protected bool _isRootState = false;
    protected PlayerStateMachine _context;
    protected PlayerStateFactory _statesFactory;
    protected PlayerBaseState _currentSubState;
    protected PlayerBaseState _currentSuperState;

    private bool _groundCheckFlag;

    protected bool GroundCheckFlag { get => _groundCheckFlag; set => _groundCheckFlag = value; }
    public PlayerBaseState CurrentSubState { get => _currentSubState; }

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) {
        _context = currentContext;
        _statesFactory = playerStateFactory; 
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

    protected void SwitchState(PlayerBaseState newState) {
        ExitState();

        newState.EnterState();

        if (_isRootState)
            _context.CurrentState = newState;
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState); 
    }

    protected void SetSuperState(PlayerBaseState newSuperState) {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState) {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
    #endregion

    #region Common Methods
    // protected void velocityVerletIntegration(float velocity) {
    //     float previousYVelocity = _context.CurrentMovementY;
    //     float newYVelocity = previousYVelocity + velocity;
    //     _context.CurrentMovementY = newYVelocity;
    //     float nextYvelocity = (previousYVelocity + newYVelocity) * .5f;
    //     _context.AppliedMovementY = nextYvelocity;
    // }
    #endregion
}