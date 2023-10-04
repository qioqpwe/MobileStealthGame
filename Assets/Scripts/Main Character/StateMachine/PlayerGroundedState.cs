using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handle Gravity
public class PlayerGroundedState : PlayerBaseState {
    bool _crouchFlag = false;

    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {
        _isRootState = true;
    }

    public override void EnterState() {
        InitializeSubState();
    }

    public override void UpdateState() {
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        if (_context.IsCrouchPress && !_crouchFlag) { // Crouch flag is to ensure that we setBool on animator only once while holding crouch button
            _context.Animator.SetBool(_context.IsCrouchPressedHash, true);
            _crouchFlag = true;
        } else if (!_context.IsCrouchPress && _crouchFlag) {
            _context.Animator.SetBool(_context.IsCrouchPressedHash, false);
            _crouchFlag = false;
        }
    }

    public override void InitializeSubState() {
        if (!_context.IsMovePress) {
            SetSubState(_statesFactory.Idle());
            _statesFactory.Idle().EnterState();
        } else if (_context.IsMovePress) {
            SetSubState(_statesFactory.Walk());
            _statesFactory.Walk().EnterState();
        } else {
            SetSubState(_statesFactory.Run());
            _statesFactory.Run().EnterState();
        }
    }
}
