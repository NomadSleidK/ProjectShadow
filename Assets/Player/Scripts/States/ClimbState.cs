using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ClimbState : State
{
    private float rotationY = 0.0f;
    private float speedRotating = 7.0f;
    float _velocity = 3.0f;

    private float _highRange = 1.4f;
    private float _undoRange = 1.0f;
    private float _grabRayRange = 0.8f;

    private float _undoDelta = 0.0f; //оставляем 0.0f что бы проверять полное поднятие капсулы
    private float _highDelta = 2.0f;

    private LayerMask GrabWallMask;

    //initialization

    public GameObject _gameObject;
    public GameObject _visor;
    public Rigidbody _rb;
    private GrabDetector _grabDetector;

    private Ray _undo;
    private Ray _high;
    private Ray _grabRay;
    private RaycastHit _hit;

    protected readonly StateMachine _stateMachine;

    public ClimbState(StateMachine stateMachine, GameObject gameObject, Rigidbody rb, GameObject visor)
    {
        _gameObject = gameObject;
        _rb = rb;
        _stateMachine = stateMachine;
        _visor = visor;
    }

    //Methods

    public void Enter()
    {
        _rb.isKinematic = true;
        GrabWallMask = LayerMask.GetMask("GrabBlock");
        _grabDetector = _visor.GetComponent<GrabDetector>();
    }

    public void Exit()
    {
        _gameObject.transform.rotation = _visor.transform.rotation;
        _visor.transform.rotation = _gameObject.transform.rotation;
        _grabDetector.ResetGrabInfo(); //сбросить информацию в GrabDetector
    }

    public void UpdateState()
    {

    }

    public void FixedUpdateState()
    {
        VisorRotation();
        ClimbMove();
        GrabDetected();
    }

    //Functions

    public void VisorRotation() //�������� ������ � ������ �� y
    {
        float delta = Input.GetAxis("Mouse X") * speedRotating;

        rotationY = _visor.transform.localEulerAngles.y + delta;
        _visor.transform.localEulerAngles = new Vector3(0, rotationY, 0);
    }

    private void GrabDetected()
    {

        _grabRay = new Ray(_visor.transform.position, _gameObject.transform.forward);
        Debug.DrawRay(_grabRay.origin, _grabRay.direction * _grabRayRange, Color.yellow);

        if (Physics.Raycast(_grabRay, out _hit, _grabRayRange, GrabWallMask))
        {
            _stateMachine.EnterIn<GrabState>();
        }
    }

    public void ClimbMove()
    {
        //нижний луч
        _undo = new Ray(_gameObject.transform.position + new Vector3(0, _undoDelta, 0), _gameObject.transform.forward);
        Debug.DrawRay(_undo.origin, _undo.direction * _undoRange, Color.cyan);

        //верхний луч
        _high = new Ray(_gameObject.transform.position + new Vector3(0, _highDelta, 0), _gameObject.transform.forward);
        Debug.DrawRay(_high.origin, _high.direction * _highRange, Color.red);

        //если нижний луч попал, а верхний нет
        if (Physics.Raycast(_undo, _undoRange) && !Physics.Raycast(_high, _highRange))
        {
            //двигаем персонажа на верх
            _gameObject.transform.position = Vector3.MoveTowards(_gameObject.transform.position, _gameObject.transform.position + new Vector3(0, 3.0f, 0), Time.deltaTime * _velocity);
        }
        else
        {
            _rb.isKinematic = false;
            _rb.AddForce(_gameObject.transform.up * 2.0f, ForceMode.Impulse);
            _rb.AddForce(_gameObject.transform.forward * 4.0f, ForceMode.Impulse);
            _stateMachine.EnterIn<IdleState>();
        }
    }
}
