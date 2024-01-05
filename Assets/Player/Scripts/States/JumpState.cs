using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class JumpState : State
{
    private float rotationY = 0.0f;
    private float speedRotating = 7.0f;

    private float _highRange = 1.4f; //длина луча
    private float _undoRange = 1.0f;
    private float _grabRayRange =0.9f;

    private float _undoDelta = 0.3f; //высота поднятия луча
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
        GrabWallMask = LayerMask.GetMask("GrabBlock");
        _grabDetector = _visor.GetComponent<GrabDetector>();
    }

    public void Exit()
    {
        _grabDetector.ResetGrabInfo(); //сбросить информацию в GrabDetector
    }

    public void UpdateState()
    {
        IsGround();
        GrabDetector();
    }

    public void FixedUpdateState()
    {
        VisorRotation();
        CheckClimb();
    }

    //Functions

    public void VisorRotation() //�������� ������ � ������ �� y
    {
        float delta = Input.GetAxis("Mouse X") * speedRotating;

        rotationY = _visor.transform.localEulerAngles.y + delta;
        _visor.transform.localEulerAngles = new Vector3(0, rotationY, 0);
    }

    private void GrabDetector()
    {

        _grabRay = new Ray(_visor.transform.position, _visor.transform.forward);
        Debug.DrawRay(_grabRay.origin, _grabRay.direction * _grabRayRange, Color.yellow);

        if (_grabDetector.GetGrabing())
        {
            if(Physics.Raycast(_grabRay, out _hit, _grabRayRange, GrabWallMask))
            {
                Ray _garbNormal = new Ray(_visor.transform.position, -_hit.normal);

                if (Physics.Raycast(_garbNormal, _grabRayRange, GrabWallMask))
                {
                    Quaternion rot = _gameObject.transform.rotation;
                    _gameObject.transform.rotation = Quaternion.FromToRotation(-_gameObject.transform.forward, _hit.normal) * _gameObject.transform.rotation;
                    _visor.transform.rotation = rot;

                    _stateMachine.EnterIn<GrabState>();
                }
            }
            else
            {
                _gameObject.transform.Translate(0, 2.0f * Time.deltaTime, 0);
            }
        }
    }

    public void CheckClimb() //проверка на возможность залезть на уступ
    {
        //нижний луч
        _undo = new Ray(_gameObject.transform.position + new Vector3(0, _undoDelta, 0), _gameObject.transform.forward);
        Debug.DrawRay(_undo.origin, _undo.direction * _undoRange, Color.cyan);

        //верхний луч
        _high = new Ray(_gameObject.transform.position + new Vector3(0, _highDelta, 0), _gameObject.transform.forward);
        Debug.DrawRay(_high.origin, _high.direction * _highRange, Color.red);

        if (Physics.Raycast(_undo, out _hit, _undoRange) && !Physics.Raycast(_high, _highRange))
        {
            //луч нормаль к поверхности уступа
            Ray _climbNormal = new Ray(_gameObject.transform.position + new Vector3(0, _undoDelta, 0), -_hit.normal);

            //если нормаль существкет и достаёт до поверхности
            if (Physics.Raycast(_climbNormal, _undoRange))
            {
                //переходим в состояние ClimbState

                Quaternion rot = _gameObject.transform.rotation;
                _gameObject.transform.rotation = Quaternion.FromToRotation(-_gameObject.transform.forward, _hit.normal) * _gameObject.transform.rotation;
                _visor.transform.rotation = rot;

                _stateMachine.EnterIn<ClimbState>();
            }
        }
    }

    public void IsGround() //�������� ������
    {     
        if (_rb.velocity.y == 0)
        {
            _gameObject.transform.rotation = _visor.transform.rotation;
            _visor.transform.rotation = _gameObject.transform.rotation;
            _stateMachine.EnterIn<IdleState>();
        }
    }
}