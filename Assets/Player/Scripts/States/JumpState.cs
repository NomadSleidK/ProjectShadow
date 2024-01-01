using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : State
{
    private float rotationY = 0.0f;
    private float speedRotating = 7.0f;
    private float speedMoving = 8.5f;

    private float moveX;
    private float moveZ;

    //initialization

    public GameObject _gameObject;
    public GameObject _visor;
    public Rigidbody _rb;

    protected readonly StateMachine _stateMachine;

    public JumpState(StateMachine stateMachine, GameObject gameObject, Rigidbody rb, GameObject visor)
    {
        _gameObject = gameObject;                        
        _rb = rb;                                               
        _stateMachine = stateMachine;
        _visor = visor;
    }

    //Methods

    public void Enter()
    {
        moveX = Input.GetAxis("Horizontal") * speedMoving;
        moveZ = Input.GetAxis("Vertical") * speedMoving;
    }

    public void Exit()
    {
        _gameObject.transform.rotation = _visor.transform.rotation;
        _visor.transform.rotation = _gameObject.transform.rotation;
    }

    public void UpdateState()
    {
        VisorRotation();
        JumpMove();
    }

    public void FixedUpdateState()
    {

    }

    //Functions

    public void VisorRotation() //�������� ������ � ������ �� y
    {
        float delta = Input.GetAxis("Mouse X") * speedRotating;

        rotationY = _visor.transform.localEulerAngles.y + delta;
        _visor.transform.localEulerAngles = new Vector3(0, rotationY, 0);

    }

    public void JumpMove() //�������� ������
    {
        _gameObject.transform.Translate(moveX * Time.deltaTime, 0, moveZ * Time.deltaTime);
        if(_rb.velocity.y == 0)
        {
            _stateMachine.EnterIn<IdleState>();
        }
    }
}