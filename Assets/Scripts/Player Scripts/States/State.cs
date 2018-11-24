using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    eNone = 0,
    eIdle = 1,
    eJumping = 2,
    eFalling = 3,
    eDead = 4,
    eRespawn = 5,
    eWalking = 6,
    eRunning = 7,
    eGrapple = 8,
    eWallSlide = 9,
    eClimbing = 10,
    eWallJumping = 11,
    eUncontrollable = 12,
    eLanding = 13,
    eClimbUpLedge = 14
}

public class State
{
    public State(StateType stateType) { m_stateType = stateType; }
    public virtual void onStart() { m_active = true; }
    public virtual void onUpdate() { }
    public virtual void onFinish() { m_active = false; }

    public StateType getStateType() { return m_stateType; }
    public bool isActive() { return m_active; }

    private readonly StateType m_stateType;
    private bool m_active = false;
}
