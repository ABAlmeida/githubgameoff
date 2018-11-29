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
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_WallCling");

        BoxCollider2D box2d = m_playerScript.gameObject.GetComponent<BoxCollider2D>();
        m_currentHitBox = box2d.size;
        box2d.size = m_playerScript.Wall_Hit_Box;

        Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
        if (m_playerScript.IsOnLeftWall())
        {
            rigidbody2D.AddForce(new Vector2(-10.0f, 0.0f));
        }
        else
        {
            rigidbody2D.AddForce(new Vector2(10.0f, 0.0f));
        }
    }
    public override void onUpdate()
    {
        if (m_playerScript.m_timeSpentClimbing > m_playerScript.Climb_time)
        {
            m_playerScript.SetNextState(StateType.eWallSlide);
        }
        else
        {
            m_playerScript.m_timeSpentClimbing += Time.deltaTime;
        }

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
            m_playerScript.SetNextState(StateType.eWallSlide);
        }

        if (m_playerScript.IsOnLeftWall())
        {
            m_playerScript.FaceLeft();
        }
        else if (m_playerScript.IsOnRightWall())
        {
            m_playerScript.FaceRight();
        }

        m_playerScript.IsWallJumping();

        m_playerScript.Falling();
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
