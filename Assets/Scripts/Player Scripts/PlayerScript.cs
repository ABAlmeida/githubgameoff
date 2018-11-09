using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        m_states.Add(new WallJumpingState(this));
        m_states.Add(new UncontrollableState(this));

        m_activeState = getState(StateType.eFalling);

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

        HasReleasedJump();
        IsChangingDirection();
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

    public bool IsJumping()
    {
        if (Input.GetAxis("Jump") > 0.0f
            && CanJump())
        {
            SetNextState(StateType.eJumping);
            return true;
        }

        return false;
    }

    public bool IsWallJumping()
    {
        if (Input.GetAxis("Jump") > 0.0f
            && IsOnWall()
            && m_hasReleasedJump)
        {
            SetNextState(StateType.eWallJumping);
            return true;
        }

        return false;
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

    public void RefreshJumps()
    {
        m_numberOfJumpsUsed = 0;
    }

    public bool IsGrounded()
    {
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D collider2D = gameObject.GetComponent<BoxCollider2D>();
        RaycastHit2D isLeftGrounded = Physics2D.Raycast(new Vector2(rigidbody2D.position.x - collider2D.bounds.extents.x, rigidbody2D.position.y - collider2D.bounds.extents.y),
                                                        -Vector2.up,
                                                        0.1f,
                                                        LayerMask.GetMask("Environment"));
        RaycastHit2D isRightGrounded = Physics2D.Raycast(new Vector2(rigidbody2D.position.x + collider2D.bounds.extents.x, rigidbody2D.position.y - collider2D.bounds.extents.y), 
                                                        -Vector2.up, 
                                                        0.1f, 
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
                                                         collider2D.bounds.extents.x + 0.2f,
                                                        LayerMask.GetMask("Environment"));

        if (isLeftWall)
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
                                                         collider2D.bounds.extents.x + 0.2f,
                                                        LayerMask.GetMask("Environment"));

        if (isRightWall)
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
        
        return false;
    }

    public bool IsIdle()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (IsGrounded() 
            && Math.Abs(moveX) == 0.0f)
        {
            SetNextState(StateType.eIdle);
            return true;
        }

        return false;
    }

    public bool IsWalking()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (Math.Abs(moveX) <= 0.7f
            && Math.Abs(moveX) > 0.0f)
        {
            SetNextState(StateType.eWalking);
            return true;
        }

        return false;
    }

    public bool IsRunning()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (Math.Abs(moveX) > 0.7f)
        {
            SetNextState(StateType.eRunning);
            return true;
        }

        return false;
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

    public bool HasReleasedJump()
    {
        if (Input.GetAxis("Jump") == 0.0f)
        {
            m_hasReleasedJump = true;
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

    public void IsChangingDirection()
    {
        Vector2 velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        Transform transform = gameObject.GetComponent<Transform>();
        Vector2 localScale = transform.localScale;

        if (localScale.x > 0.0f && velocity.x < 0.0f)
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        else if (localScale.x < 0.0f && velocity.x > 0.0f)
        {
            localScale.x *= -1;
            transform.localScale = localScale;
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

    private StateType m_nextStateType;
    [NonSerialized] public int m_numberOfJumpsUsed = 0;
    [NonSerialized] public bool m_hasReleasedJump = true;
}
