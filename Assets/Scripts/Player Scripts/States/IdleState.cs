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
        Rigidbody2D rg2d = m_playerScript.GetComponent<Rigidbody2D>();
        Vector2 velocity = rg2d.velocity;
        velocity.x = 0.0f;
        rg2d.velocity = velocity;
        m_playerScript.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
    }

    public override void onUpdate()
    {
        base.onUpdate();

        m_playerScript.IsTooCloseToGround();

        m_playerScript.IsWalking();
        m_playerScript.IsRunning();
        m_playerScript.Falling();
        m_playerScript.IsJumping();
        if (m_playerScript.IsGrapplingWall())
        {
            m_playerScript.SetNextState(StateType.eGrapple);
        }
    }
    public override void onFinish()
    {
        base.onFinish();
        m_playerScript.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
    }

    private PlayerScript m_playerScript;
}