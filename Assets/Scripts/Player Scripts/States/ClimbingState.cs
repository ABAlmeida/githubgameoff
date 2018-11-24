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
    }

    public override void onUpdate()
    {
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
                velocity.y = m_playerScript.Climb_Speed;
            }
            else if (Climb < 0.0f)
            {
                velocity.y = -m_playerScript.Climb_Speed;
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
        else
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
    }

    private PlayerScript m_playerScript;
    private Vector2 m_currentHitBox;
}
