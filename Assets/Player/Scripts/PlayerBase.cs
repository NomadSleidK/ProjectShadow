using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private StateMachine _stateMachine;
    public GameObject _gameObject;
    public Rigidbody _rb;

    void Start()
    {
        _stateMachine = new StateMachine(_gameObject, _rb);
        _stateMachine.EnterIn<IdleState>();
    }

    void Update()
    {
        _stateMachine.Update();
    }

    void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }
}
