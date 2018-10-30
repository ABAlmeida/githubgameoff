using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    eNone = 0,
    eGrounded = 1,
    eJumping = 2,
    eFalling = 3,
    eDead = 4,
    eRespawn = 5
}

public class State
{
    public State(StateType stateType) { m_stateType = stateType; } 
    public virtual void onStart() { m_activated = true; m_active = true; }
    public virtual void onUpdate() { }
    public virtual void onFinish() { m_activated = false; m_active = false; m_nextState = StateType.eNone; }

    public StateType getStateType() { return m_stateType; }
    public bool isActivated() { return m_activated; }
    public bool isActive() { return m_active; }
    public StateType GetNextState() { return m_nextState; }

    protected StateType m_nextState = StateType.eNone;
    private readonly StateType m_stateType;
    private bool m_activated = false;
    private bool m_active = false;
}

public class GroundedState : State
{
    public GroundedState(PlayerScript playerScript) : base(StateType.eGrounded)
    {
        m_playerScript = playerScript;
    }
    public override void onStart() { base.onStart(); }
    public override void onUpdate()
    {
        base.onUpdate();

        float moveX = Input.GetAxis("Horizontal");
        Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
        int Player_Speed = m_playerScript.Running_Speed;

        if ( Math.Abs(moveX) <= 0.7f )
        {
            Player_Speed =m_playerScript.Walking_Speed;
        }

        Vector2 Velocity = rigidbody2D.velocity;
        if (moveX > 0)
        {
            Velocity.x = Player_Speed;
        }
        else if (moveX < 0)
        {
            Velocity.x = Player_Speed * -1;
        }

        rigidbody2D.velocity = Velocity;

        if (Input.GetButton("Jump"))
        {
            rigidbody2D.AddForce(new Vector2(0.0f, m_playerScript.Jump_Force));
            m_nextState = StateType.eJumping;
        }

        if (!m_playerScript.IsGrounded())
        {
            m_nextState = StateType.eFalling;
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

        float moveX = Input.GetAxis("Horizontal");
        Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();
        
        if (moveX > 0)
        {
            rigidbody2D.AddForce(new Vector2(m_playerScript.Aerial_Mobility, 0));
        }
        else if (moveX < 0)
        {
            rigidbody2D.AddForce(new Vector2(m_playerScript.Aerial_Mobility * -1, 0));
        }
        
        if (rigidbody2D.velocity.y < 0.0f)
        {
            m_nextState = StateType.eFalling;
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
    public override void onStart() { base.onStart(); }
    public override void onUpdate()
    {
        base.onUpdate();

        float moveX = Input.GetAxis("Horizontal");
        Rigidbody2D rigidbody2D = m_playerScript.gameObject.GetComponent<Rigidbody2D>();

        if (moveX > 0)
        {
            rigidbody2D.AddForce(new Vector2(m_playerScript.Aerial_Mobility, 0));
        }
        else if (moveX < 0)
        {
            rigidbody2D.AddForce(new Vector2(m_playerScript.Aerial_Mobility * -1, 0));
        }
        
        if (m_playerScript.IsGrounded())
        {
            m_nextState = StateType.eGrounded;
        }
    }
    public override void onFinish() { base.onFinish(); }

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
            m_nextState = StateType.eRespawn;
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
        m_nextState = StateType.eFalling;
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
        m_states.Add(new GroundedState(this));
        m_states.Add(new JumpingState(this));
        m_states.Add(new FallingState(this));
        m_states.Add(new DeadState(this));
        m_states.Add(new RespawnState(this));

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
        else if (m_activeState.GetNextState() != StateType.eNone)
        {
            State nextState = getState(m_activeState.GetNextState());
            m_activeState.onFinish();
            m_activeState = nextState;
        }

        if (!m_activeState.isActivated())
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

        return new State(StateType.eNone);
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

    public void SetNextState(StateType nextStateType)
    {
        m_nextStateType = nextStateType;
    }

    public List<State> m_states;
    public State m_activeState;

    public int Running_Speed = 6;
    public int Walking_Speed = 2;
    public int Jump_Force = 20;
    public int Aerial_Mobility = 5;
    public float Dead_Time = 2.0f;

    public float Distance_To_Ground;

    private StateType m_nextStateType;
}
