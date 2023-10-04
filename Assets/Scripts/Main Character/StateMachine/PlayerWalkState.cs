using UnityEngine;

public class PlayerWalkState : PlayerBaseState {

    Vector3 posToLookAt;

    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) {}

    public override void EnterState() {
        _context.CurrentMaxVelocity = 1f;
        // previousVelocity = _context.transform.position;
        // InitializeSubState();
    }

    public override void UpdateState() {

        posToLookAt.x = _context.CurrentMovementX;
        posToLookAt.z = _context.CurrentMovementZ;

        posToLookAt = _context.Matrix.MultiplyPoint3x4(posToLookAt); // 3x4 * 4x4 = 3x4 -> Returns Vector3 of point transformed by currenttransformation matrix
        
        _context.AppliedMovementX = posToLookAt.x * Time.deltaTime * _context.walkSpeed;
        _context.AppliedMovementZ = posToLookAt.z * Time.deltaTime * _context.walkSpeed;

        if (_context.CurrentMovementX == 0 && _context.CurrentMovementZ == 0) { // Player released WASD // Joystick -> decelerate 
            if (_context.Velocity1D > -0.05f && _context.Velocity1D < 0.05f) {
                _context.Velocity1D = 0f;
            } else if (_context.Velocity1D > 0f) {
                _context.Velocity1D -= Time.deltaTime * _context.movementAnimationTransitionAcceleration;
            } else {
                _context.Velocity1D += Time.deltaTime * _context.movementAnimationTransitionAcceleration;
            }
        // } else if (_context.Velocity1D > _context.CurrentMaxVelocity) { // player coming from running state
        //     _context.Velocity1D -= Time.deltaTime * _context.movementAnimationTransitionAcceleration;
        } else {
            _context.Velocity1D = Mathf.Sqrt(Mathf.Pow(_context.CurrentMovementX, 2) + Mathf.Pow(_context.CurrentMovementZ, 2));
            // _context.Velocity1D += Time.deltaTime * _context.movementAnimationTransitionAcceleration;
            _context.Velocity1D = Mathf.Clamp(_context.Velocity1D, 0, _context.CurrentMaxVelocity);

            Quaternion targetRotation = Quaternion.LookRotation(posToLookAt);

            _context.transform.rotation = Quaternion.Slerp(_context.transform.rotation, targetRotation, _context.rotationFactorPerFrame * Time.deltaTime);
        }

        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates() {
        if (!_context.IsMovePress) {
            SwitchState(_statesFactory.Idle());
        }
    }

    public override void InitializeSubState() {}

}

// public override void UpdateState() {

//     _context.AppliedMovementX = _context.CurrentMovementX * Time.deltaTime * _context.walkSpeed;
//     _context.AppliedMovementZ = _context.CurrentMovementZ * Time.deltaTime * _context.walkSpeed;

//     if (_context.CurrentMovementX == 0 && _context.CurrentMovementZ == 0) { // Player released WASD // Joystick -> decelerate 
//         if (_context.Velocity1D > -0.05f && _context.Velocity1D < 0.05f) {
//             _context.Velocity1D = 0f;
//         } else if (_context.Velocity1D > 0f) {
//             _context.Velocity1D -= Time.deltaTime * _context.movementAnimationTransitionAcceleration;
//         } else {
//             _context.Velocity1D += Time.deltaTime * _context.movementAnimationTransitionAcceleration;
//         }
//     } else {
//         _context.Velocity1D = _context.CurrentMovement.sqrMagnitude;
//         _context.Velocity1D = Mathf.Clamp(_context.Velocity1D, 0, _context.CurrentMaxVelocity);

//         posToLookAt.x = _context.CurrentMovementX;
//         posToLookAt.z = _context.CurrentMovementZ;
//         Quaternion targetRotation = Quaternion.LookRotation(posToLookAt);
//         _context.transform.rotation = Quaternion.Slerp(_context.transform.rotation, targetRotation, _context.rotationFactorPerFrame * Time.deltaTime);
//     }

//     CheckSwitchStates();
// }