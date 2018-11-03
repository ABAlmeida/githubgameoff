using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlidingState : State
{
    public WallSlidingState(PlayerScript playerScript) : base(StateType.eWallSlide)
    {
        m_playerScript = playerScript;
    }
    public override void onStart() { base.onStart(); m_playerScript.gameObject.GetComponent<Rigidbody2D>().gravityScale = m_playerScript.Fall_Scale; }
    public override void onUpdate()
    {
        if (m_playerScript.IsGrapplingWall())
        {
            m_playerScript.SetNextState(StateType.eGrapple);
        }

        m_playerScript.AerialMove();

        m_playerScript.IsWallJumping();

        m_playerScript.Falling();
    }
    public override void onFinish() { base.onFinish(); m_playerScript.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f; }

    private PlayerScript m_playerScript;
}
