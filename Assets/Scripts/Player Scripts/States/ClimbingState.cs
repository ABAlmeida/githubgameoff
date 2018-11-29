using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingState : State
{
    public ClimbingState(PlayerScript playerScript) : base(StateType.eClimbing)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_WallClimb");

        BoxCollider2D box2d = m_playerScript.gameObject.GetComponent<BoxCollider2D>();
        m_currentHitBox = box2d.size;
        box2d.size = m_playerScript.Wall_Hit_Box;
        Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0.0f;
    }

    public override void onUpdate()
    {
        m_playerScript.IsTooCloseToTheWall();

        if (m_playerScript.m_timeSpentClimbing > m_playerScript.Climb_time)
        {
            m_playerScript.SetNextState(StateType.eWallSlide);
        }
        else
        {
            m_playerScript.m_timeSpentClimbing += Time.deltaTime;
        }

        float Climb = Input.GetAxis("Climb");
        Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
        Vector2 velocity = rigidbody2D.velocity;

        if (m_playerScript.IsGrapplingWall())
        {
            if (Climb > 0.0f
                && ((m_playerScript.CanClimbUpLeftWall() && m_playerScript.IsOnLeftWall())
                    || (m_playerScript.CanClimbUpRightWall() && m_playerScript.IsOnRightWall())))
            {
                m_playerScript.SetNextState(StateType.eClimbUpLedge);
            }
            else if (Climb > 0.0f
                && ((!m_playerScript.CanClimbUpLeftWall() && m_playerScript.IsOnLeftWall())
                    || (!m_playerScript.CanClimbUpRightWall() && m_playerScript.IsOnRightWall())))
            {
                velocity.y = m_playerScript.GetClimbSpeed();
            }
            else if (Climb < 0.0f)
            {
                velocity.y = -m_playerScript.GetClimbSpeed();
            }
            else
            {
                velocity = new Vector2(0.0f, 0.0f);
                m_playerScript.SetNextState(StateType.eGrapple);
            }
        }
        else
        {
            velocity = new Vector2(0.0f, 0.0f);
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

        rigidbody2D.velocity = velocity;

        m_playerScript.IsWallJumping();
    }
    public override void onFinish()
    {
        base.onFinish();

        BoxCollider2D box2d = m_playerScript.gameObject.GetComponent<BoxCollider2D>();
        box2d.size = m_currentHitBox;
        Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 1.0f;
    }

    private PlayerScript m_playerScript;
    private Vector2 m_currentHitBox;
}
