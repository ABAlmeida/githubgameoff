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
        m_states.Add(new LandingState(this));
        m_states.Add(new ClimbUpLedgeState(this));

        m_activeState = getState(StateType.eRespawn);

        m_nextStateType = StateType.eNone;

        m_audioManager = FindObjectOfType<AudioManager>();

        m_collectibles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Collectible"));

        m_cameraZones = new List<GameObject>();
        m_cameraZones.Add(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>().Current_Camera_Zone);
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
            case StateType.eLanding:
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
        RaycastHit2D isLeftGrounded = Physics2D.Raycast(new Vector2(rigidbody2D.position.x - collider2D.bounds.extents.x - 0.01f, rigidbody2D.position.y - collider2D.bounds.extents.y),
                                                        -Vector2.up,
                                                        0.1f,
                                                        LayerMask.GetMask("Environment"));
        RaycastHit2D isRightGrounded = Physics2D.Raycast(new Vector2(rigidbody2D.position.x + collider2D.bounds.extents.x + 0.01f, rigidbody2D.position.y - collider2D.bounds.extents.y), 
                                                        -Vector2.up, 
                                                        0.1f, 
                                                        LayerMask.GetMask("Environment"));
        if (isLeftGrounded || isRightGrounded)
        {
            return true;
        }

        //if (rigidbody2D.velocity.y == 0.0f)
        //{
        //    return true;
        //}

        return false;
    }

    public bool IsFullyGrounded()
    {
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D collider2D = gameObject.GetComponent<BoxCollider2D>();
        RaycastHit2D isLeftGrounded = Physics2D.Raycast(new Vector2(rigidbody2D.position.x - collider2D.bounds.extents.x - 0.01f, rigidbody2D.position.y - collider2D.bounds.extents.y),
                                                        -Vector2.up,
                                                        0.1f,
                                                        LayerMask.GetMask("Environment"));
        RaycastHit2D isRightGrounded = Physics2D.Raycast(new Vector2(rigidbody2D.position.x + collider2D.bounds.extents.x + 0.01f, rigidbody2D.position.y - collider2D.bounds.extents.y),
                                                        -Vector2.up,
                                                        0.1f,
                                                        LayerMask.GetMask("Environment"));
        if (isLeftGrounded && isRightGrounded)
        {
            return true;
        }

        //if (rigidbody2D.velocity.y == 0.0f)
        //{
        //    return true;
        //}

        return false;
    }

    public bool IsOnLeftWall()
    {
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D collider2D = gameObject.GetComponent<BoxCollider2D>();
        RaycastHit2D isLeftWall = Physics2D.Raycast(rigidbody2D.position,
                                                        Vector2.left,
                                                         collider2D.bounds.extents.x + 0.05f,
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
                                                         collider2D.bounds.extents.x + 0.05f,
                                                        LayerMask.GetMask("Environment"));

        if (isRightWall)
        {
            return true;
        }

        return false;
    }

    public bool CanClimbUpRightWall()
    {

        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D collider2D = gameObject.GetComponent<BoxCollider2D>();
        RaycastHit2D isRightWall = Physics2D.Raycast(new Vector2(rigidbody2D.position.x, rigidbody2D.position.y + 0.01f),
                                                        Vector2.right,
                                                         collider2D.bounds.extents.x + 0.1f,
                                                        LayerMask.GetMask("Environment"));

        if (isRightWall)
        {
            return false;
        }

        return true;
    }

    public bool CanClimbUpLeftWall()
    {

        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        BoxCollider2D collider2D = gameObject.GetComponent<BoxCollider2D>();
        RaycastHit2D isLeftWall = Physics2D.Raycast(new Vector2(rigidbody2D.position.x, rigidbody2D.position.y + 0.01f),
                                                        Vector2.left,
                                                         collider2D.bounds.extents.x + 0.1f,
                                                        LayerMask.GetMask("Environment"));

        if (isLeftWall)
        {
            return false;
        }

        return true;
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
            && m_activeState.getStateType() != StateType.eClimbUpLedge)
        {
            if (IsOnWall()
                && (m_activeState.getStateType() != StateType.eGrapple
                    && m_activeState.getStateType() != StateType.eClimbing))
            {
                SetNextState(StateType.eWallSlide);
            }
            else if (m_activeState.getStateType() != StateType.eGrapple)
            {
                SetNextState(StateType.eFalling);
            }
        }

        if (IsGrounded())
        {
            switch (m_activeState.getStateType())
            {
                case StateType.eJumping:
                case StateType.eWallJumping:
                    if (Velocity.y == 0)
                    {
                        SetNextState(StateType.eFalling);
                    }
                    break;
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
                    if ((IsOnWall() && IsFullyGrounded())
                        || (IsGrounded() && !IsOnWall()))
                    SetNextState(StateType.eLanding);
                    break;
            }            
        }
    }

    public void FinishedSpawning()
    {
        SetNextState(StateType.eIdle);
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
        
        if (velocity.x < 0.0f) // Change to Face Left
        {
            FaceLeft();
        }
        else if (velocity.x > 0.0f) // Change to Face right
        {
            FaceRight();
        }
    }

    public void FaceLeft()
    {
        Transform transform = gameObject.GetComponent<Transform>();
        Vector2 localScale = transform.localScale;
        if (localScale.x > 0.0f) // Change to Face Left
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
    public void FaceRight()
    {
        Transform transform = gameObject.GetComponent<Transform>();
        Vector2 localScale = transform.localScale;
        if (localScale.x < 0.0f) // Change to Face Right
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    public void LoopFallAnimation()
    {
        gameObject.GetComponent<Animator>().Play("Player_Fall");
    }

    public void LandingFinished()
    {
        m_landingFinished = true;
    }

    public void Run()
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
            Velocity.y += 0.2f;
            rigidbody2D.velocity = Velocity;
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
            Velocity.y += 0.2f;
            rigidbody2D.velocity = Velocity;
            SetNextState(StateType.eWalking);
        }
    }

    public void PlayWalkSoundEffect()
    {
        m_audioManager.Play("footstep_walk");
    }

    public void PlayRunSoundEffect()
    {
        m_audioManager.Play("footstep_run");
    }

    public Vector2 GetSpawnLocation()
    {
        GameObject go = GameObject.FindGameObjectWithTag("MainCamera");

        return go.GetComponent<FollowPlayer>().Current_Camera_Zone.GetComponent<Spawn_Position>().Zone_Spawn_Position;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Camera")
        {
            bool zone_found = false;

            foreach (GameObject go in m_cameraZones)
            {
                if (go == collision.gameObject)
                {
                    zone_found = true;
                }
            }

            if (!zone_found)
            {
                m_cameraZones.Add(collision.gameObject);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Camera")
        {
            for (int i = 0; i < m_cameraZones.Count; ++i)
            {
                if (m_cameraZones[i] == collision.gameObject)
                {
                    m_cameraZones.Remove(m_cameraZones[i]);
                    break;
                }
            }

            if (m_cameraZones.Count == 1)
            {
                GameObject go = GameObject.FindGameObjectWithTag("MainCamera");

                go.GetComponent<FollowPlayer>().Current_Camera_Zone = m_cameraZones[0];
            }
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
    public float Nightmare_Running_Speed = 6.0f;
    public float Nightmare_Walking_Speed = 2.0f;
    public float Nightmare_Jump_Force = 20.0f;
    public float Nightmare_Aerial_Mobility = 5;
    public float Nightmare_Dead_Time = 2.0f;
    public int Nightmare_Number_Of_Jumps = 2;
    public Vector2 Nightmare_Wall_Jump_Force = new Vector2(200.0f, 20.0f);
    public float Nightmare_Climb_Speed = 0.3f;
    public float Fall_Scale = 2.0f;
    public Vector2 Wall_Hit_Box = new Vector2(2.0f, 2.0f);
    public Vector2 Fall_Hit_Box = new Vector2(2.0f, 2.0f);
    public ParticleSystem m_particleSystemLeft;
    public ParticleSystem m_particleSystemRight;

    private StateType m_nextStateType;
    [NonSerialized] public int m_numberOfJumpsUsed = 0;
    [NonSerialized] public bool m_hasReleasedJump = true;
    [NonSerialized] public bool m_landingFinished = false;
    [NonSerialized] public Vector2 m_startFallPosition;
    [NonSerialized] public AudioManager m_audioManager;
    [NonSerialized] public List<GameObject> m_collectibles;

    private List<GameObject> m_cameraZones;
}
