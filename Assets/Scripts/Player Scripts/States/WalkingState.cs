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
        m_playerScript.RefreshJumps();
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_Walk");
    }

    public override void onUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (Math.Abs(moveX) <= 0.7f
            && Math.Abs(moveX) > 0.0f)
        {
            Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
            Vector2 Velocity = rigidbody2D.velocity;
            if (moveX > 0)
            {
                Velocity.x = m_playerScript.Walking_Speed;
            }
            else if (moveX < 0)
            {
                Velocity.x = m_playerScript.Walking_Speed * -1;
            }
            rigidbody2D.velocity = Velocity;
            m_playerScript.SetNextState(StateType.eWalking);
        }

        m_playerScript.IsRunning();
        m_playerScript.IsJumping();
        m_playerScript.Falling();
        if (m_playerScript.IsGrapplingWall())
        {
            m_playerScript.SetNextState(StateType.eGrapple);
        }
        m_playerScript.IsIdle();
    }
    public override void onFinish() { base.onFinish(); }

    private PlayerScript m_playerScript;
}
