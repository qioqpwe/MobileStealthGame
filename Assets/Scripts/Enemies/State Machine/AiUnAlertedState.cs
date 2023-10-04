using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiUnAlertedState : AiBaseState {
    Transform firstWaypoint;
    Transform moveToward;
    Coroutine coroutine;
    int currentIndex = 0;
    float awaitTimeAtWaypoint;


    public AiUnAlertedState(AiStateMachine currentContext, AiStateFactory aiStateFactory) : base (currentContext, aiStateFactory) {
        _isRootState = true;
    }

    public override void EnterState() {
        // Debug.Log("Unalerted State");
        _context.navMeshAgent.autoBraking = false;

        moveToward = firstWaypoint = _context.waypointSystem.GetCurrentWaypoint(currentIndex);
        
        coroutine = _context.StartCoroutine(returnalToWaypoints());
    }

    public override void UpdateState() {
        if (_context.navMeshAgent.enabled == false) {
            if (_context.waypointSystem.isSingleWaypoint) {
                _context.animator.SetBool(_context.IsIdleHash, true);
                _context.transform.rotation = Quaternion.Slerp(_context.transform.rotation, firstWaypoint.rotation, _context.rotationSpeed * Time.deltaTime);  
            } else {
                if (awaitTimeAtWaypoint > 0) {
                    _context.animator.SetBool(_context.IsIdleHash, true);
                    awaitTimeAtWaypoint -= Time.deltaTime;
                } else {
                    _context.animator.SetBool(_context.IsIdleHash, false);
                    handleMovement();     
                    handleRotation(); 
                }
            }
        }
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        if ( _context.aiSensor.ObjectsCollided.Count > 0 ) {
            if (coroutine != null) {
                _context.StopCoroutine(coroutine);
                _context.navMeshAgent.enabled = false;
            }
            SwitchState(_statesFactory.Alerted());
        }
    }

    public override void InitializeSubState() {}

    void handleMovement() {
        _context.transform.position = Vector3.MoveTowards(_context.transform.position, moveToward.position, _context.moveSpeed * Time.deltaTime);
        if (Vector3.Distance(_context.transform.position, moveToward.position) < _context.distanceThreshold) {
            awaitTimeAtWaypoint = _context.waypointSystem.GetCurrentAwaitTime(currentIndex);
            moveToward = _context.waypointSystem.GetNextWaypoint(ref currentIndex);
        } 
    }

    void handleRotation() {
        Quaternion rotation = Quaternion.LookRotation( (moveToward.position - _context.transform.position).normalized, _context.transform.up);
        _context.transform.rotation = Quaternion.Slerp(_context.transform.rotation, rotation, _context.rotationSpeed * Time.deltaTime);
    }

    IEnumerator returnalToWaypoints() {
        _context.navMeshAgent.enabled = true;
        _context.navMeshAgent.destination = firstWaypoint.position;
        while ((_context.transform.position - firstWaypoint.position ).magnitude > _context.distanceThresholdToWaypoint) {
            CheckSwitchStates();
            yield return null;
        }   
        _context.navMeshAgent.enabled = false;
    }
}