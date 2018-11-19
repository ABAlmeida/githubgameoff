using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : State
{
    public LandingState(PlayerScript playerScript) : base(StateType.eLanding)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_Landing");

        ParticleSystem.InheritVelocityModule iv = m_playerScript.m_particleSystemLeft.inheritVelocity;
        iv.enabled = false;
        iv = m_playerScript.m_particleSystemRight.inheritVelocity;
        iv.enabled = false;
        m_playerScript.m_particleSystemLeft.Emit(20);
        m_playerScript.m_particleSystemRight.Emit(20);
        iv = m_playerScript.m_particleSystemLeft.inheritVelocity;
        iv.enabled = true;
        iv = m_playerScript.m_particleSystemRight.inheritVelocity;
        iv.enabled = true;
    }

    public override void onUpdate()
    {
        base.onUpdate();

        m_playerScript.Walk();
        m_playerScript.Run();

        if (m_playerScript.m_landingFinished)
        {
            m_playerScript.SetNextState(StateType.eIdle);
        }
    }
    public override void onFinish()
    {
        base.onFinish();
        m_playerScript.m_landingFinished = false;
    }

    private PlayerScript m_playerScript;
}
