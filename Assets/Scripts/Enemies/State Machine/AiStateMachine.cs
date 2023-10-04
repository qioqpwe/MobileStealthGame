using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiStateMachine : MonoBehaviour {
    AiStateFactory _statesFactory;
    AiBaseState _currentState;
    
    
    // public Animator _animator;
    public WaypointSystem waypointSystem;
    public AiSensor aiSensor;
    public NavMeshAgent navMeshAgent;
    public Animator animator;

    [Header("Movement While UnAlerted Properties")]
    public float moveSpeed = 5f;
    public float distanceThreshold = .1f, distanceThresholdToWaypoint = .1f;
    public float rotationSpeed = .1f;
    public float timeBeforeAiRecognises = 2, timeBeforeAiForgets;
    public float timeBeforeNavmeshAgentUpdatesPath = 2;

    int _isLookingHash, _isIdleHash;

    public AiBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public int IsLookingHash { get => _isLookingHash; set => _isLookingHash = value; }
    public int IsIdleHash { get => _isIdleHash; set => _isIdleHash = value; }
    

    void Awake() {
        _isLookingHash = Animator.StringToHash("IsLooking");
        _isIdleHash = Animator.StringToHash("IsIdle");

        transform.position = waypointSystem.transform.GetChild(0).position;
        _statesFactory = new AiStateFactory(this);
        _currentState = _statesFactory.UnAlerted();
        _currentState.EnterState();
    }


    void Update() {
        _currentState.UpdateStates();

    }
}
