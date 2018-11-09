using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingState : State
{
    public GrapplingState(PlayerScript playerScript) : base(StateType.eGrapple)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_playerScript.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
    }
    public override void onUpdate()
    {
        if (m_playerScript.IsGrapplingWall())
        {
            Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.velocity = new Vector2(0.0f, 0.0f);

            if (m_playerScript.IsClimbing())
            {
                m_playerScript.SetNextState(StateType.eClimbing);
            }
        }
        else
        {
            m_playerScript.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        }

        m_playerScript.IsWallJumping();

        m_playerScript.Falling();
    }
    public override void onFinish()
    {
        base.onFinish();
        m_playerScript.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
    }

    private PlayerScript m_playerScript;
}
