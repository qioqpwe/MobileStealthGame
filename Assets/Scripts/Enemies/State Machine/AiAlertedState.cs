using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAlertedState : AiBaseState {
    float timer, recognitionTimer;

    public AiAlertedState(AiStateMachine currentContext, AiStateFactory aiStateFactory) : base (currentContext, aiStateFactory) {
        _isRootState = true;
    }

    public override void EnterState() {
        // Debug.Log("Alerted State");

        _context.navMeshAgent.autoBraking = true;
        _context.navMeshAgent.enabled = true;
        recognitionTimer = _context.timeBeforeAiRecognises;
        _context.animator.SetBool(_context.IsLookingHash, true);
        
    }

    public override void UpdateState() {
        if (RecognitionOfPlayer()) {
            timer -= Time.deltaTime;
            if (timer < 0f) {
                if ( _context.aiSensor.ObjectsCollided.Count > 0 ) { 
                    _context.navMeshAgent.destination = _context.aiSensor.ObjectsCollided[0].transform.position;
                    timer = _context.timeBeforeNavmeshAgentUpdatesPath;
                } else { // Player Got out of the line of sight
                    // Arrives at last known position of the player
                    if ((_context.transform.position - _context.navMeshAgent.destination ).magnitude < _context.distanceThreshold) {
                        _context.navMeshAgent.enabled = false;
                        SwitchState(_statesFactory.Forgetting());
                    }
                }
            }
        }
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {}

    public override void InitializeSubState() {}

    bool RecognitionOfPlayer() {
        if (recognitionTimer > 0 ) {
            if (_context.aiSensor.ObjectsCollided.Count == 0) {
                SwitchState(_statesFactory.Forgetting());
            }
            recognitionTimer -= Time.deltaTime;
            return false;
        }
        _context.animator.SetBool(_context.IsLookingHash, false);
        _context.animator.SetBool(_context.IsIdleHash, false);
        return true;
    }
}
