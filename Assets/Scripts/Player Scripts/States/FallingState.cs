using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : State
{
    public FallingState(PlayerScript playerScript) : base(StateType.eFalling)
    {
        m_playerScript = playerScript;
    }
    public override void onStart() { base.onStart(); m_playerScript.gameObject.GetComponent<Rigidbody2D>().gravityScale = m_playerScript.Fall_Scale; }
    public override void onUpdate()
    {
        base.onUpdate();

        m_playerScript.AerialMove();

        m_playerScript.IsJumping();

        m_playerScript.Falling();
    }
    public override void onFinish() { base.onFinish(); m_playerScript.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f; }

    private PlayerScript m_playerScript;
}
