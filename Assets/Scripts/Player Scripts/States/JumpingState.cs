using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : State
{
    public JumpingState(PlayerScript playerScript) : base(StateType.eJumping)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_Jump 0");

        Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0.0f);
        rigidbody2D.AddForce(new Vector2(0.0f, m_playerScript.Jump_Force));
        m_playerScript.m_hasReleasedJump = false;
        m_playerScript.m_numberOfJumpsUsed += 1;
    }
    public override void onUpdate()
    {
        base.onUpdate();

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
