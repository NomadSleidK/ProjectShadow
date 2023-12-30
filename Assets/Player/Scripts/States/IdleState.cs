using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private float rotationY = 0.0f;
    private float speedRotating = 7.0f;
    private float speedMoving = 8.5f;

    //initialization

    public GameObject _gameObject;
    public Rigidbody _rb;

    protected readonly StateMachine _stateMachine;

    public IdleState(StateMachine stateMachine, GameObject gameObject, Rigidbody rb)
    {
        _gameObject = gameObject;
        _rb = rb;
        _stateMachine = stateMachine;
    }

    //Methods

    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void UpdateState()
    {
        CharacterRotation();
        CheckNewSate();
    }

    public void FixedUpdateState()
    {

    }

    //Functions

    public void CharacterRotation() //вращение игрока и камеры по y
    {
        float delta = Input.GetAxis("Mouse X") * speedRotating;

        rotationY = _gameObject.transform.localEulerAngles.y + delta;
        _gameObject.transform.localEulerAngles = new Vector3(0, rotationY, 0);

    }

    public void CheckNewSate() //проверка на выход из состояния покоя
    {
        if (Input.GetAxis("Horizontal") * speedMoving != 0.0 || Input.GetAxis("Vertical") * speedMoving != 0.0)
        {
            _stateMachine.EnterIn<MoveState>();
        }
    }
}

