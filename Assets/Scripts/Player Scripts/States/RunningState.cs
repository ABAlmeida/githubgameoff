using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : State
{
    public RunningState(PlayerScript playerScript) : base(StateType.eRunning)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_Run 0");
        if (m_playerScript.GetComponent<Rigidbody2D>().velocity.x < 0.0f)
        {
            m_playerScript.m_particleSystemLeft.Play();
        }
        else
        {
            m_playerScript.m_particleSystemRight.Play();
        }
        m_playerScript.RefreshJumps();

        m_playerScript.m_audioManager.Play("footstep_0");
    }

    public override void onUpdate()
    {
        m_playerScript.Run();

        if ( m_playerScript.IsWalking() )
        {
            m_playerScript.m_audioManager.StopAfterComplete("footstep_0");
        }
        m_playerScript.IsJumping();
        m_playerScript.Falling();
        if (m_playerScript.IsGrapplingWall())
        {
            m_playerScript.SetNextState(StateType.eGrapple);
        }
        if ( m_playerScript.IsIdle() )
        {
            m_playerScript.m_audioManager.StopAfterComplete("footstep_0");
        }
    }
    public override void onFinish()
    {
        base.onFinish();
        m_playerScript.m_particleSystemLeft.Stop();
        m_playerScript.m_particleSystemRight.Stop();

        if (!m_playerScript.m_audioManager.IsPaused("footstep_0"))
        {
            m_playerScript.m_audioManager.StopAfterComplete("footstep_0");
        }
    }

    private PlayerScript m_playerScript;
}
