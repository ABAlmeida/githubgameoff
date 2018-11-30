using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlidingState : State
{
    public WallSlidingState(PlayerScript playerScript) : base(StateType.eWallSlide)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_WallCling");

        Rigidbody2D rb2d = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
        rb2d.gravityScale = m_playerScript.Fall_Scale;

        BoxCollider2D box2d = m_playerScript.gameObject.GetComponent<BoxCollider2D>();
        m_currentHitBox = box2d.size;
        box2d.size = m_playerScript.Wall_Hit_Box;

        if (m_playerScript.IsOnLeftWall())
        {
            m_playerScript.m_particleSystemLeft.Play();
        }
        else if (m_playerScript.IsOnRightWall())
        {
            m_playerScript.m_particleSystemRight.Play();
        }
    }
    public override void onUpdate()
    {
        m_playerScript.IsTooCloseToTheWall();

        if (m_playerScript.IsGrapplingWall())
        {
            m_playerScript.SetNextState(StateType.eGrapple);
        }

        if (m_playerScript.IsOnLeftWall())
        {
            m_playerScript.FaceLeft();
        }
        else if (m_playerScript.IsOnRightWall())
        {
            m_playerScript.FaceRight();
        }

        m_playerScript.AerialMove();

        m_playerScript.IsWallJumping();

        m_playerScript.Falling();
    }
    public override void onFinish()
    {
        base.onFinish();
        m_playerScript.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        m_playerScript.m_particleSystemLeft.Stop();
        m_playerScript.m_particleSystemRight.Stop();

        BoxCollider2D box2d = m_playerScript.gameObject.GetComponent<BoxCollider2D>();
        box2d.size = m_currentHitBox;
    }

    private PlayerScript m_playerScript;
    private Vector2 m_currentHitBox;
}
