using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDetector : MonoBehaviour
{
    public bool _isGrabing;
    public bool _lockGrab;
    private GameObject LastObject;
    private GameObject ActiveObject; 

    void Start()
    {
        _isGrabing = false;
        _lockGrab = false;
        LastObject = null;
        ActiveObject = null;
    }

    void Update()
    {
        
    }

    public bool GetGrabing()
    {
        return _isGrabing;
    }

    public bool GetLock()
    {
        return _lockGrab;
    }

    public void ResetLock()
    {
        _lockGrab = true;
    }

    public void ResetGrabInfo()
    {
        LastObject = null;
        ActiveObject = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GrabBlock")
        {
            LastObject = ActiveObject;
            ActiveObject = other.gameObject;
            if (LastObject != ActiveObject)
            {
                _isGrabing = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "GrabBlock")
        {
            _lockGrab = false;
            _isGrabing = false;
        }
    }
}
