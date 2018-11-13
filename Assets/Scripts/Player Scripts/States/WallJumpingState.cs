﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpingState : State
{
    public WallJumpingState(PlayerScript playerScript) : base(StateType.eWallJumping)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();

        Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
        Vector2 WallJump = new Vector2(m_playerScript.Wall_Jump_Force.x, m_playerScript.Wall_Jump_Force.y);
        if (m_playerScript.IsOnRightWall())
        {
            WallJump.x *= -1.0f;
        }
        rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
        rigidbody2D.AddForce(WallJump);
        m_playerScript.m_hasReleasedJump = false;
    }
    public override void onUpdate()
    {
        m_playerScript.AerialMove();

        m_playerScript.Falling();

        if (m_playerScript.IsOnWall())
        {
            m_playerScript.SetNextState(StateType.eWallSlide);
        }
    }
    public override void onFinish() { base.onFinish(); }

    private PlayerScript m_playerScript;
}