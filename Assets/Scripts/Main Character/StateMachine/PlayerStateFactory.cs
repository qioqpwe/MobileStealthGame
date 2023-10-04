using System.Collections.Generic;

enum PlayerStates {
    grounded,
    jump,
    fall,
    idle,
    walk,
    run,
    aim
}

public class PlayerStateFactory {
    PlayerStateMachine _context;
    Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext) {
        _context = currentContext;
        _states[PlayerStates.grounded] = new PlayerGroundedState(_context, this);
        _states[PlayerStates.idle] = new PlayerIdleState(_context, this);
        _states[PlayerStates.walk] = new PlayerWalkState(_context, this);
    }

    public PlayerBaseState Idle() {
        return _states[PlayerStates.idle];
    }
    public PlayerBaseState Walk() {
        return _states[PlayerStates.walk];
    }
    public PlayerBaseState Run() {
        return _states[PlayerStates.run];
    }
    public PlayerBaseState Grounded() {
        return _states[PlayerStates.grounded];
    }
    public PlayerBaseState Jump() {
        return _states[PlayerStates.jump];
    }
    public PlayerBaseState Aim() {
        return _states[PlayerStates.aim];
    }
}