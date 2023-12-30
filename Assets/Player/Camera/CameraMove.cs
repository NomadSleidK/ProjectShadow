using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float rotationX = 0.0f;
    private float speedRotating = 7.0f;

    void Start()
    {
        
    }

    void Update()
    {
        Move();
    }

    public void Move() //вращаем камеру по x т.к по y камера вращается вместе с телом игрока
    {
        float mouseY = Input.GetAxis("Mouse Y") * speedRotating;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        transform.localEulerAngles = new Vector3(rotationX, 0, 0);
    }
}
