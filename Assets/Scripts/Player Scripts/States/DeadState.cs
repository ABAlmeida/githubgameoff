using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    public DeadState(PlayerScript playerScript) : base(StateType.eDead)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_deadTime = 0.0f;
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_Death");

        foreach (GameObject _gameObject in m_playerScript.m_collectibles)
        {
            _gameObject.GetComponent<Collectible>().Reset();
        }
    }
    public override void onUpdate()
    {
        m_deadTime += Time.deltaTime;

        if (m_deadTime >= m_playerScript.Dead_Time)
        {
            m_playerScript.SetNextState(StateType.eRespawn);
        }
    }
    public override void onFinish()
    {
        base.onFinish();
    }

    private PlayerScript m_playerScript;
    float m_deadTime;
}
