using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiForgettingState : AiBaseState {
    Coroutine coroutine;

    public AiForgettingState(AiStateMachine currentContext, AiStateFactory aiStateFactory) : base (currentContext, aiStateFactory) {
        _isRootState = true;
    }

    public override void EnterState() {
        // Debug.Log("Forgetting State");

        coroutine = _context.StartCoroutine(forgetOfPlayer());
    }

    public override void UpdateState() {
        if ( _context.aiSensor.ObjectsCollided.Count > 0 ) { // Player Got in the line of sight
            _context.StopCoroutine(coroutine);
            SwitchState(_statesFactory.Alerted());
        }
    }

    public override void ExitState() {
        _context.animator.SetBool(_context.IsLookingHash, false);
    }

    public override void CheckSwitchStates() {}

    public override void InitializeSubState() {}

    IEnumerator forgetOfPlayer() {
        _context.animator.SetBool(_context.IsLookingHash, true);
        yield return new WaitForSeconds(_context.timeBeforeAiForgets);
        if ( _context.aiSensor.ObjectsCollided.Count == 0 ) { // Player Got out of the line of sight
            // Forgot About the Player 
            SwitchState(_statesFactory.UnAlerted());
        }
    }
}
