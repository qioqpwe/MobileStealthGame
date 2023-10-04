using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour {
    PlayerStateFactory _statesFactory;
    PlayerBaseState _currentState;
    PlayerInput _input;
    Animator _animator;
    Camera _camera;
    CharacterController _characterController;

    Vector2 _currentMovementInput;
    Vector3 _currentMovement, _appliedMovement;
    Matrix4x4 _matrix;


    bool _isMovePressed;
    [SerializeField]
    bool _isCrouchPressed;
    int _velocity1DHash, _isCrouchPressedHash;

    float _velocity1D;
    float _currentMaxVelocity = 1f;

    public float movementAnimationTransitionAcceleration = 5f;
    public float walkSpeed = 5f;
    public float rotationFactorPerFrame = 5f;

    #region Properties
    public PlayerStateFactory StatesFactory { get => _statesFactory; set => _statesFactory = value; }
    public CharacterController CharacterController { get => _characterController; }
    public PlayerBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public Animator Animator { get => _animator; }
    public bool IsMovePress { get => _isMovePressed;  }
    public bool IsCrouchPress { get => _isCrouchPressed; }
    public int Velocity1DHash { get => _velocity1DHash; }
    public int IsCrouchPressedHash { get => _isCrouchPressedHash; }
    public float Velocity1D { get => _velocity1D; set => _velocity1D = value; }
    public float CurrentMovementX{ get => _currentMovement.x; set => _currentMovement.x = value; }
    public float CurrentMovementZ { get => _currentMovement.z; set => _currentMovement.z = value; }
    public Vector3 CurrentMovement { get => _currentMovement; }
    public float CurrentMaxVelocity { get => _currentMaxVelocity; set => _currentMaxVelocity = value; }
    public float AppliedMovementX { get => _appliedMovement.x; set => _appliedMovement.x = value; }
    public float AppliedMovementY { get => _appliedMovement.y; set => _appliedMovement.y = value; }
    public float AppliedMovementZ { get => _appliedMovement.z; set => _appliedMovement.z = value; }
    public Matrix4x4 Matrix { get => _matrix; }
    #endregion

    void Awake() {
        _input = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _velocity1DHash = Animator.StringToHash("Velocity1D");
        _isCrouchPressedHash = Animator.StringToHash("IsCrouchPressed");


        _matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 60, 0));

        _statesFactory = new PlayerStateFactory(this);
        _currentState = _statesFactory.Grounded();
        _currentState.EnterState();

        setUpInputControls();
    }

    void OnEnable() {
        _input.CharacterControls.Enable();
    }

    void OnDisable() {
        _input.CharacterControls.Disable();
    }

    void setUpInputControls() {
        _input.CharacterControls.Move.started += ctx => onMovement(ctx);
        _input.CharacterControls.Move.canceled += ctx => onMovement(ctx);
        _input.CharacterControls.Move.performed += ctx => onMovement(ctx);

        _input.CharacterControls.Crouch.started += ctx => onCrouch(ctx);
    }

    void onMovement(InputAction.CallbackContext ctx) {
        _currentMovementInput = ctx.ReadValue<Vector2>();
        // Velocity variables (Move) set to constant number means no accelearation
        _currentMovement.x = _currentMovementInput.x; _currentMovement.z = _currentMovementInput.y;
        _isMovePressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    void onCrouch(InputAction.CallbackContext ctx) {
        _isCrouchPressed = !_isCrouchPressed;
    }

    void LateUpdate() {
        _currentState.UpdateStates();

        _animator.SetFloat(_velocity1DHash, _velocity1D);

        _characterController.Move(_appliedMovement);

    }
}
