using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AiStates {
    unAlerted,
    alerted,
    forgetting
}

public class AiStateFactory {
    AiStateMachine _context;
    Dictionary<AiStates, AiBaseState> _states = new Dictionary<AiStates, AiBaseState>();

    public AiStateFactory(AiStateMachine currentContext) {
        _context = currentContext;
        _states[AiStates.unAlerted] = new AiUnAlertedState(_context, this);
        _states[AiStates.alerted] = new AiAlertedState(_context, this);
        _states[AiStates.forgetting] = new AiForgettingState(_context, this);

    }

    public AiBaseState UnAlerted() {
        return _states[AiStates.unAlerted];
    }
    public AiBaseState Alerted() {
        return _states[AiStates.alerted];
    }
    public AiBaseState Forgetting() {
        return _states[AiStates.forgetting];
    }
}