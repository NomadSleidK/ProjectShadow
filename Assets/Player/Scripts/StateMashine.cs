using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private Dictionary<Type, State> _states;
    private State _currentState;

    public GameObject _gameObject;
    public Rigidbody _rb;

    public StateMachine(GameObject gameObject, Rigidbody rb)
    {
        _gameObject = gameObject;
        _rb = rb;

        _states = new Dictionary<Type, State>()
        {
            [typeof(IdleState)] = new IdleState(this, _gameObject, _rb),
            [typeof(MoveState)] = new MoveState(this, _gameObject, _rb),
        };
    }

    public void EnterIn<TState>() where TState : State
    {
        if (_states.TryGetValue(typeof(TState), out State state))
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState?.Enter();
        }
    }

    public void Update()
    {
        _currentState.UpdateState();
    }

    public void FixedUpdate()
    {
        _currentState.FixedUpdateState();
    }
}
