using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MoveState : State
{
    private float rotationY = 0.0f;
    private float speedRotating = 7.0f;
    private float speedMoving = 6.0f;
    private float _jumpForce = 6.0f;

    //initialization

    public GameObject _gameObject;
    public Rigidbody _rb;

    protected readonly StateMachine _stateMachine;

    public MoveState(StateMachine stateMachine, GameObject gameObject, Rigidbody rb)
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
        CheckIdle();
        //CheckFall();
    }

    public void FixedUpdateState()
    {
        CharacterRotation();
        CharacterMove();
        CheckJump();
    }

    //Functions

    public void CharacterRotation() //�������� ������ � ������ �� y
    {
        float delta = Input.GetAxis("Mouse X") * speedRotating;

        rotationY = _gameObject.transform.localEulerAngles.y + delta;
        _gameObject.transform.localEulerAngles = new Vector3(0, rotationY, 0);
        
    }
    
    public void CharacterMove() //�������� ������
    {
        float moveX = Input.GetAxis("Horizontal") * speedMoving;
        float moveZ = Input.GetAxis("Vertical") * speedMoving;
        _gameObject.transform.Translate(moveX * Time.deltaTime, 0, moveZ * Time.deltaTime);
    }

    public void CheckJump() //�������� �� ����� �� ��������� ��������
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            float moveX = Input.GetAxis("Horizontal") * speedMoving;
            float moveZ = Input.GetAxis("Vertical") * speedMoving;

            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _rb.AddForce(_gameObject.transform.forward * moveZ, ForceMode.Impulse);
            _rb.AddForce(_gameObject.transform.right * moveX, ForceMode.Impulse);
            _stateMachine.EnterIn<JumpState>();
        }
    }

    public void CheckIdle() //�������� �� ����� �� ��������� ��������
    {
        if (Input.GetAxis("Horizontal") * speedMoving == 0.0 && Input.GetAxis("Vertical") * speedMoving == 0.0)
        {
            _stateMachine.EnterIn<IdleState>();
        }
    }

    private void CheckFall()
    {
        if (_rb.velocity.y != 0.0f)
        {
            _stateMachine.EnterIn<JumpState>();
        }
    }
}