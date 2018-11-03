using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingState : State
{
    public ClimbingState(PlayerScript playerScript) : base(StateType.eClimbing)
    {
        m_playerScript = playerScript;
    }
    public override void onStart() { base.onStart(); }
    public override void onUpdate()
    {
        float Climb = Input.GetAxis("Climb");
        Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
        Vector2 velocity = rigidbody2D.velocity;

        if (m_playerScript.IsGrapplingWall())
        {
            if (Climb > 0.0f)
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
        rigidbody2D.velocity = velocity;

        m_playerScript.IsWallJumping();
    }
    public override void onFinish() { base.onFinish(); }

    private PlayerScript m_playerScript;
}
