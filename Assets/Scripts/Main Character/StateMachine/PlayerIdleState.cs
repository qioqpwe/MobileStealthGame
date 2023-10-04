using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState {
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {}

    public override void EnterState() {
        // InitializeSubState(); 
    }

    public override void UpdateState() {
        if (_context.Velocity1D > -0.05f && _context.Velocity1D < 0.05f) {
            _context.Velocity1D = 0f;
        } else if (_context.Velocity1D > 0f) {
            _context.Velocity1D -= Time.deltaTime * _context.movementAnimationTransitionAcceleration;
        } else {
            _context.Velocity1D += Time.deltaTime * _context.movementAnimationTransitionAcceleration;
        }

        CheckSwitchStates();               
    }

    public override void ExitState() {
    }

    public override void CheckSwitchStates() {
        if (_context.IsMovePress) {
            SwitchState(_statesFactory.Walk());
        }
    }

    public override void InitializeSubState() {}
}