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
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_StartFall");
        Rigidbody2D rg2d = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
        rg2d.gravityScale = m_playerScript.Fall_Scale;
        m_playerScript.m_startFallPosition = rg2d.position;

        BoxCollider2D box2d = m_playerScript.gameObject.GetComponent<BoxCollider2D>();
        m_currentHitBox = box2d.size;
        box2d.size = m_playerScript.Fall_Hit_Box;
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

        BoxCollider2D box2d = m_playerScript.gameObject.GetComponent<BoxCollider2D>();
        box2d.size = m_currentHitBox;
    }

    private PlayerScript m_playerScript;
    private Vector2 m_currentHitBox;
}
