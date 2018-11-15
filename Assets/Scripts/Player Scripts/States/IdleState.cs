using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(PlayerScript playerScript) : base(StateType.eIdle)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_playerScript.RefreshJumps();
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_Idle 0");
    }

    public override void onUpdate()
    {
        base.onUpdate();

        m_playerScript.IsWalking();
        m_playerScript.IsRunning();
        m_playerScript.Falling();
        m_playerScript.IsJumping();
        if (m_playerScript.IsGrapplingWall())
        {
            m_playerScript.SetNextState(StateType.eGrapple);
        }
    }
    public override void onFinish() { base.onFinish(); }

    private PlayerScript m_playerScript;
}