using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    eNone = 0,
    eIdle = 1,
    eJumping = 2,
    eFalling = 3,
    eDead = 4,
    eRespawn = 5,
    eWalking = 6,
    eRunning = 7,
    eGrapple = 8,
    eWallSlide = 9,
    eClimbing = 10
}

public class State
{
    public State(StateType stateType) { m_stateType = stateType; } 
    public virtual void onStart() { m_active = true; }
    public virtual void onUpdate() { }
    public virtual void onFinish() { m_active = false; }

    public StateType getStateType() { return m_stateType; }
    public bool isActive() { return m_active; }

    private readonly StateType m_stateType;
    private bool m_active = false;
}

public class IdleState : State
{
    public IdleState(PlayerScript playerScript) : base(StateType.eIdle)
    {
        m_playerScript = playerScript;
    }
    public override void onStart() { base.onStart(); m_playerScript.RefreshJumps(); }
    public override void onUpdate()
    {
        base.onUpdate();

        m_playerScript.Walk();
        m_playerScript.Running();
        m_playerScript.Falling();
        m_playerScript.Jump();
        if (m_playerScript.IsGrapplingWall())
        {
            m_playerScript.SetNextState(StateType.eGrapple);
        }
    }
    public override void onFinish() { base.onFinish(); }
    
    private PlayerScript m_playerScript;
}

public class JumpingState : State
{
    public JumpingState(PlayerScript playerScript) : base(StateType.eJumping)
    {
        m_playerScript = playerScript;
    }
    public override void onStart() { base.onStart(); }
    public override void onUpdate()
    {
        base.onUpdate();

        m_playerScript.AerialMove();

        m_playerScript.Jump();

        m_playerScript.Falling();

        if(m_playerScript.IsOnWall())
        {
            m_playerScript.SetNextState(StateType.eWallSlide);
        }
    }
    public override void onFinish() { base.onFinish(); }

    private PlayerScript m_playerScript;
}

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

        m_playerScript.Jump();

        m_playerScript.Falling();
    }
    public override void onFinish() { base.onFinish(); m_playerScript.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f; }

    private PlayerScript m_playerScript;
}

public class DeadState : State
{
    public DeadState(PlayerScript playerScript) : base(StateType.eDead)
    {
        m_playerScript = playerScript;
    }
    public override void onStart() { base.onStart(); m_deadTime = 0.0f; }
    public override void onUpdate()
    {
        m_deadTime += Time.deltaTime;

        if (m_deadTime >= m_playerScript.Dead_Time)
        {
            m_playerScript.SetNextState(StateType.eRespawn);
        }
    }
    public override void onFinish() { base.onFinish(); }

    private PlayerScript m_playerScript;
    float m_deadTime;
}

public class RespawnState : State
{
    public RespawnState(PlayerScript playerScript) : base(StateType.eRespawn)
    {
        m_playerScript = playerScript;
    }
    public override void onStart() { base.onStart(); }
    public override void onUpdate()
    {
        m_playerScript.transform.SetPositionAndRotation(new Vector3(0.0f, 1.0f), new Quaternion());
        m_playerScript.SetNextState(StateType.eFalling);
    }
    public override void onFinish() { base.onFinish(); }

    private PlayerScript m_playerScript;
}

public class WalkingState : State
{
    public WalkingState(PlayerScript playerScript) : base(StateType.eWalking)
    {
        m_playerScript = playerScript;
    }
    public override void onStart() { base.onStart(); }
    public override void onUpdate()
    {
        m_playerScript.Running();
        m_playerScript.Walk();
        m_playerScript.Jump();
        m_playerScript.Falling();
        if (m_playerScript.IsGrapplingWall())
        {
            m_playerScript.SetNextState(StateType.eGrapple);
        }
        m_playerScript.Idle();
    }
    public override void onFinish() { base.onFinish(); }

    private PlayerScript m_playerScript;
}

public class RunningState : State
{
    public RunningState(PlayerScript playerScript) : base(StateType.eRunning)
    {
        m_playerScript = playerScript;
    }
    public override void onStart() { base.onStart(); }
    public override void onUpdate()
    {
        m_playerScript.Running();
        m_playerScript.Walk();
        m_playerScript.Jump();
        m_playerScript.Falling();
        if (m_playerScript.IsGrapplingWall())
        {
            m_playerScript.SetNextState(StateType.eGrapple);
        }
        m_playerScript.Idle();
    }
    public override void onFinish() { base.onFinish(); }

    private PlayerScript m_playerScript;
}

public class GrapplingState : State
{
    public GrapplingState(PlayerScript playerScript) : base(StateType.eGrapple)
    {
        m_playerScript = playerScript;
    }
    public override void onStart() { base.onStart(); }
    public override void onUpdate()
    {
        if (m_playerScript.IsGrapplingWall())
        {
            Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
            rigidbody2D.gravityScale = 0.0f;

            if (m_playerScript.IsClimbing())
            {
                m_playerScript.SetNextState(StateType.eClimbing);
            }
        }

        m_playerScript.WallJump();

        m_playerScript.Falling();
    }
    public override void onFinish() { base.onFinish(); }

    private PlayerScript m_playerScript;
}

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

        m_playerScript.WallJump();

        m_playerScript.Falling();
    }
    public override void onFinish() { base.onFinish(); m_playerScript.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f; }

    private PlayerScript m_playerScript;
}

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

        m_playerScript.WallJump();
    }
    public override void onFinish() { base.onFinish(); }

    private PlayerScript m_playerScript;
}

public class PlayerScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        m_states = new List<State>();
        m_states.Add(new IdleState(this));
        m_states.Add(new WalkingState(this));
        m_states.Add(new RunningState(this));
        m_states.Add(new JumpingState(this));
        m_states.Add(new FallingState(this));
        m_states.Add(new DeadState(this));
        m_states.Add(new RespawnState(this));
        m_states.Add(new GrapplingState(this));
        m_states.Add(new WallSlidingState(this));
        m_states.Add(new ClimbingState(this));

        m_activeState = getState(StateType.eFalling);

        Distance_To_Ground = gameObject.GetComponent<BoxCollider2D>().bounds.extents.y;

        m_nextStateType = StateType.eNone;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_nextStateType != StateType.eNone)
        {
            State nextState = getState(m_nextStateType);
            m_activeState.onFinish();
            m_activeState = nextState;
            m_nextStateType = StateType.eNone;
        }

        if (!m_activeState.isActive())
        {
            m_activeState.onStart();
        }

        m_activeState.onUpdate();
    }

    private State getState(StateType stateType)
    {
        for(int i = 0; i < m_states.Count; ++i )
        {
            if (m_states[i].getStateType() == stateType)
            {
                return m_states[i];
            }
        }

        Debug.LogError("State not found.");
        return new State(StateType.eNone);
    }

    public bool CanJump()
    {
        if (!m_hasReleasedJump)
        {
            return false;
        }

        switch (m_activeState.getStateType())
        {
            case StateType.eNone:
            case StateType.eDead:
            case StateType.eRespawn:
                break;
            case StateType.eIdle:
            case StateType.eWalking:
            case StateType.eRunning:
                return true;
            case StateType.eJumping:
            case StateType.eFalling:
                if (m_numberOfJumpsUsed < Number_Of_Jumps)
                {
                    return true;
                }
                break;
            case StateType.eGrapple:
            case StateType.eWallSlide:
                return true;
        }

        return false;
    }
    public void Jump()
    {
        if (Input.GetButton("Jump"))
        {
            if (CanJump())
            {
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0.0f);
                rigidbody2D.AddForce(new Vector2(0.0f, Jump_Force));
                SetNextState(StateType.eJumping);
                m_hasReleasedJump = false;
                m_numberOfJumpsUsed += 1;
            }
        }
        else
        {
            m_hasReleasedJump = true;
        }
    }

    public void WallJump()
    {
        if (Input.GetButton("Jump"))
        {
            if (CanJump())
            {
                Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                rigidbody2D.gravityScale = 1.0f;
                Vector2 WallJump = new Vector2(Wall_Jump_Force.x, Wall_Jump_Force.y);
                if (IsOnRightWall())
                {
                    WallJump.x *= -1.0f;
                }
                rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
                rigidbody2D.AddForce(WallJump);
                SetNextState(StateType.eJumping);
                m_hasReleasedJump = false;
            }
        }
        else
        {
            m_hasReleasedJump = true;
        }
    }

    public void RefreshJumps()
    {
        m_numberOfJumpsUsed = 0;
    }

    public bool IsGrounded()
    {
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D collider2D = gameObject.GetComponent<BoxCollider2D>();
        RaycastHit2D isLeftGrounded = Physics2D.Raycast(new Vector2(rigidbody2D.position.x - collider2D.bounds.extents.x, rigidbody2D.position.y),
                                                        -Vector2.up,
                                                        Distance_To_Ground + 0.1f,
                                                        LayerMask.GetMask("Environment"));
        RaycastHit2D isRightGrounded = Physics2D.Raycast(new Vector2(rigidbody2D.position.x + collider2D.bounds.extents.x, rigidbody2D.position.y), 
                                                        -Vector2.up, 
                                                        Distance_To_Ground + 0.1f, 
                                                        LayerMask.GetMask("Environment"));
        if (isLeftGrounded || isRightGrounded)
        {
            return true;
        }

        return false;
    }

    public bool IsOnLeftWall()
    {
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D collider2D = gameObject.GetComponent<BoxCollider2D>();
        RaycastHit2D isLeftWall = Physics2D.Raycast(rigidbody2D.position,
                                                        Vector2.left,
                                                         collider2D.bounds.extents.x + 0.1f,
                                                        LayerMask.GetMask("Environment"));

        if (isLeftWall 
            && rigidbody2D.velocity.x <= 0.0f)
        {
            return true;
        }

        return false;
    }

    public bool IsOnRightWall()
    {
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D collider2D = gameObject.GetComponent<BoxCollider2D>();
        RaycastHit2D isRightWall = Physics2D.Raycast(rigidbody2D.position,
                                                        Vector2.right,
                                                         collider2D.bounds.extents.x + 0.1f,
                                                        LayerMask.GetMask("Environment"));

        if (isRightWall
            && rigidbody2D.velocity.x >= 0.0f)
        {
            return true;
        }

        return false;
    }

    public bool IsOnWall()
    {
        if ( (IsOnLeftWall() || IsOnRightWall()))
        {
            return true;
        }
        
        return false;
    }

    public bool IsGrapplingWall()
    {
        if (IsOnWall()
            && Input.GetAxis("Grapple") > 0.0f)
        {
            return true;
        }

        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 1.0f;
        return false;
    }

    public void Idle()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (IsGrounded() 
            && Math.Abs(moveX) == 0.0f)
        {
            SetNextState(StateType.eIdle);
        }
    }

    public void Walk()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (Math.Abs(moveX) <= 0.7f
            && Math.Abs(moveX) > 0.0f)
        {
            Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            Vector2 Velocity = rigidbody2D.velocity;
            if (moveX > 0)
            {
                Velocity.x = Walking_Speed;
            }
            else if (moveX < 0)
            {
                Velocity.x = Walking_Speed * -1;
            }
            rigidbody2D.velocity = Velocity;
            SetNextState(StateType.eWalking);
        }
    }

    public void Running()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (Math.Abs(moveX) > 0.7f)
        {
            Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            Vector2 Velocity = rigidbody2D.velocity;
            if (moveX > 0)
            {
                Velocity.x = Running_Speed;
            }
            else if (moveX < 0)
            {
                Velocity.x = Running_Speed * -1;
            }
            rigidbody2D.velocity = Velocity;
            SetNextState(StateType.eRunning);
        }
    }

    public void Falling()
    {
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        Vector2 Velocity = rigidbody2D.velocity;
        if (!IsGrounded()
            && Velocity.y < 0)
        {
            if (IsOnWall())
            {
                SetNextState(StateType.eWallSlide);
            }
            else
            {
                SetNextState(StateType.eFalling);
            }
        }

        if (IsGrounded())
        {
            switch (m_activeState.getStateType())
            {
                case StateType.eJumping:
                case StateType.eRunning:
                case StateType.eWalking:
                case StateType.eIdle:
                case StateType.eClimbing:
                    break;
                case StateType.eGrapple:
                    if (!IsGrapplingWall())
                    {
                        SetNextState(StateType.eIdle);
                    }
                    break;
                default:
                    SetNextState(StateType.eIdle);
                    break;
            }            
        }
    }

    public void AerialMove()
    {
        float moveX = Input.GetAxis("Horizontal");
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        if (moveX > 0)
        {
            rigidbody2D.AddForce(new Vector2(Aerial_Mobility, 0));
        }
        else if (moveX < 0)
        {
            rigidbody2D.AddForce(new Vector2(Aerial_Mobility * -1, 0));
        }
    }

    public bool IsClimbing()
    {
        float Climb = Input.GetAxis("Climb");
        if (Climb != 0.0f)
        {
            return true;
        }

        return false;
    }

    public void SetNextState(StateType nextStateType)
    {
        if (nextStateType != m_activeState.getStateType())
        {
            Debug.Log(nextStateType);
            m_nextStateType = nextStateType;
        }
    }

    public List<State> m_states;
    public State m_activeState;

    public float Running_Speed = 6.0f;
    public float Walking_Speed = 2.0f;
    public float Jump_Force = 20.0f;
    public float Aerial_Mobility = 5;
    public float Dead_Time = 2.0f;
    public int Number_Of_Jumps = 2;
    public Vector2 Wall_Jump_Force = new Vector2(200.0f, 20.0f);
    public float Climb_Speed = 0.3f;
    public float Fall_Scale = 2.0f;

    public float Distance_To_Ground;

    private StateType m_nextStateType;
    private int m_numberOfJumpsUsed = 0;
    private bool m_hasReleasedJump = true;
}
