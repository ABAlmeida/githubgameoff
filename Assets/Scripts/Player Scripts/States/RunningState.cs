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

        m_playerScript.GetComponent<Rigidbody2D>().sharedMaterial.friction = 0.4f;
    }

    public override void onUpdate()
    {
        m_playerScript.Run();

        m_playerScript.IsTooCloseToGround();

        m_playerScript.IsWalking();
        m_playerScript.IsJumping();
        m_playerScript.Falling();
        if (m_playerScript.IsGrapplingWall())
        {
            m_playerScript.SetNextState(StateType.eGrapple);
        }
        m_playerScript.IsIdle();
    }
    public override void onFinish()
    {
        base.onFinish();
        m_playerScript.m_particleSystemLeft.Stop();
        m_playerScript.m_particleSystemRight.Stop();
    }

    private PlayerScript m_playerScript;
}
