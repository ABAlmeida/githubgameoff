using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbUpLedgeState : State
{
    public ClimbUpLedgeState(PlayerScript playerScript) : base(StateType.eClimbUpLedge)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_Jump 0");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Environment"));

        if (m_playerScript.IsOnLeftWall())
        {
            m_playerScript.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-50.0f, 100.0f));
        }
        else
        {
            m_playerScript.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(50.0f, 100.0f));
        }
    }

    public override void onUpdate()
    {
        m_playerScript.Falling();
    }
    public override void onFinish()
    {
        base.onFinish();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Environment"), false);
    }

    private PlayerScript m_playerScript;
}
