using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private float rotationY = 0.0f;
    private float speedRotating = 7.0f;
    private float speedMoving = 8.5f;
    private float _jumpForce = 5.0f;

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
        CheckMove();
        CheckJump();
    }

    public void FixedUpdateState()
    {

    }

    //Functions

    public void CharacterRotation() //�������� ������ � ������ �� y
    {
        float delta = Input.GetAxis("Mouse X") * speedRotating;

        rotationY = _gameObject.transform.localEulerAngles.y + delta;
        _gameObject.transform.localEulerAngles = new Vector3(0, rotationY, 0);
    }

    public void CheckJump() //�������� �� ����� �� ��������� ��������
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _stateMachine.EnterIn<JumpState>();
        }
    }

    public void CheckMove() //�������� �� ����� �� ��������� �����
    {
        if (Input.GetAxis("Horizontal") * speedMoving != 0.0 || Input.GetAxis("Vertical") * speedMoving != 0.0)
        {
            _stateMachine.EnterIn<MoveState>();
        }
    }
}

