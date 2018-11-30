using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : State
{
    public WalkingState(PlayerScript playerScript) : base(StateType.eWalking)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();

        m_playerScript.IsTooCloseToGround();

        m_playerScript.RefreshJumps();
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_Walk");
        m_playerScript.GetComponent<Rigidbody2D>().sharedMaterial.friction = 0.4f;
        m_playerScript.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
    }

    public override void onUpdate()
    {
        m_playerScript.Walk();

        m_playerScript.IsRunning();
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
        m_playerScript.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
    }

    private PlayerScript m_playerScript;
}
