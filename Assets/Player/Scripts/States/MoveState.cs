using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    private float rotationY = 0.0f;
    private float speedRotating = 7.0f;
    private float speedMoving = 8.5f;

    //initialization

    public GameObject _gameObject;
    public Rigidbody _rb;

    protected readonly StateMachine _stateMachine;

    public MoveState(StateMachine stateMachine, GameObject gameObject, Rigidbody rb)
    {
        _gameObject = gameObject;                                //получаем _gameObject игрока
        _rb = rb;                                               //получаем Rigidbody игрока
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
        CharacterMove();
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
    
    public void CharacterMove() //движение игрока
    {
        float moveX = Input.GetAxis("Horizontal") * speedMoving;
        float moveZ = Input.GetAxis("Vertical") * speedMoving;
        _gameObject.transform.Translate(moveX * Time.deltaTime, 0, moveZ * Time.deltaTime);
    }

    public void CheckNewSate() //проверка на выход из состояния движения
    {
        if (Input.GetAxis("Horizontal") * speedMoving == 0.0 && Input.GetAxis("Vertical") * speedMoving == 0.0)
        {
            _stateMachine.EnterIn<IdleState>();
        }
    }       
}