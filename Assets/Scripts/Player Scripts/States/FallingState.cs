using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : State
{
    public FallingState(PlayerScript playerScript) : base(StateType.eFalling)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_playerScript.gameObject.GetComponent<Rigidbody2D>().gravityScale = m_playerScript.Fall_Scale;
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_Fall");
    }

    public override void onUpdate()
    {
        base.onUpdate();

        m_playerScript.AerialMove();

        m_playerScript.IsJumping();

        m_playerScript.Falling();

        if (m_playerScript.m_numberOfJumpsUsed == 0)
        {
            m_playerScript.m_numberOfJumpsUsed = 1;
        }
    }
    public override void onFinish()
    {
        base.onFinish();
        m_playerScript.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;

        if (m_playerScript.IsGrounded())
        {
            var iv = m_playerScript.m_particleSystemLeft.inheritVelocity;
            iv.enabled = false;
            iv = m_playerScript.m_particleSystemRight.inheritVelocity;
            iv.enabled = false;
            m_playerScript.m_particleSystemLeft.Emit(20);
            m_playerScript.m_particleSystemRight.Emit(20);
            iv = m_playerScript.m_particleSystemLeft.inheritVelocity;
            iv.enabled = true;
            iv = m_playerScript.m_particleSystemRight.inheritVelocity;
            iv.enabled = true;
        }
    }

    private PlayerScript m_playerScript;
}
