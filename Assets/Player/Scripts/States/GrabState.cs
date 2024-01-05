using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrabState : State
{
    private float rotationY = 0.0f;
    private float speedRotating = 7.0f;
    private float _grabDropSpeed = 5.0f;

    private float _grabRayRange = 0.9f;

    //initialization

    public GameObject _gameObject;
    public GameObject _visor;
    public Rigidbody _rb;
    private GrabDetector _grabDetector;

    private LayerMask GrabWallMask;
    private Ray _grabRay;

    protected readonly StateMachine _stateMachine;

    public GrabState(StateMachine stateMachine, GameObject gameObject, Rigidbody rb, GameObject visor)
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
        _rb.isKinematic = true;
        Debug.Log("Grab");
        _grabDetector = _visor.GetComponent<GrabDetector>();
    }

    public void Exit()
    {
        _gameObject.transform.rotation = _visor.transform.rotation;
        _visor.transform.rotation = _gameObject.transform.rotation;
        _grabDetector.ResetLock();
    }

    public void UpdateState()
    {

    }

    public void FixedUpdateState()
    {
        VisorRotation();
        GrabDrop();
        GrabMove();
        CheckGrab();
    }

    //Functions

    public void VisorRotation() //�������� ������ � ������ �� y
    {
        float delta = Input.GetAxis("Mouse X") * speedRotating;

        rotationY = _visor.transform.localEulerAngles.y + delta;
        _visor.transform.localEulerAngles = new Vector3(0, rotationY, 0);
    }

    public void GrabMove()
    {
        float moveX = Input.GetAxis("Horizontal") * _grabDropSpeed;
        _gameObject.transform.Translate(moveX * Time.deltaTime, 0, 0);
    }

    public void CheckGrab()
    {
        _grabRay = new Ray(_visor.transform.position, _gameObject.transform.forward);
        Debug.DrawRay(_grabRay.origin, _grabRay.direction * _grabRayRange, Color.red);

        if (!Physics.Raycast(_grabRay, _grabRayRange, GrabWallMask))
        {
            _rb.isKinematic = false;
            _stateMachine.EnterIn<JumpState>();
        }
    }

    public void GrabDrop()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            float moveX = Input.GetAxis("Horizontal") * _grabDropSpeed;
            float moveZ = Input.GetAxis("Vertical") * _grabDropSpeed;

            _gameObject.transform.rotation = _visor.transform.rotation;
            _visor.transform.rotation = _gameObject.transform.rotation;
            _rb.isKinematic = false;

            _rb.AddForce(Vector3.up * 8.0f, ForceMode.Impulse);
            _rb.AddForce(_gameObject.transform.forward * moveZ, ForceMode.Impulse);
            _rb.AddForce(_gameObject.transform.right * moveX, ForceMode.Impulse);
            _stateMachine.EnterIn<JumpState>();
        }        
    }
}
