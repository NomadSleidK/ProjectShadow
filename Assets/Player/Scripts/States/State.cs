using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State
{
    public abstract void Enter();
    public abstract void Exit();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
}
