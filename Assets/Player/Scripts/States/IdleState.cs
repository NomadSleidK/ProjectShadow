using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IdleState : State
{
    private float rotationY = 0.0f;
    private float speedRotating = 7.0f;
    private float speedMoving = 6.0f;
    private float _jumpForce = 6.0f;

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
        CheckMove();
    }

    public void FixedUpdateState()
    {
        CharacterRotation();
        CheckJump();
    }

    //Functions

    public void test() //получаем луч перпендикулярно поверхности _dn
    {
        float _range = 1.0f;
        RaycastHit hit;

        Ray _undo = new Ray(_gameObject.transform.position + new Vector3(0, 1.0f, 0), _gameObject.transform.forward);
        Debug.DrawRay(_undo.origin, _undo.direction * _range, Color.red);
        if(Physics.Raycast(_undo, out hit, _range))
        {
            Ray _dn = new Ray(_gameObject.transform.position + new Vector3(0, 1.0f, 0), hit.normal);
            Debug.DrawRay(_dn.origin, -_dn.direction, Color.green);
            _gameObject.transform.rotation = Quaternion.Euler(hit.normal);
        }
    }

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
            float moveZ = Input.GetAxis("Vertical") * speedMoving;

            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _rb.AddForce(_gameObject.transform.forward * moveZ, ForceMode.Impulse);
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

