using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnState : State
{
    bool animation_playing;
    const float m_delayTime = 2.0f;
    float m_currentDelay;

    public RespawnState(PlayerScript playerScript) : base(StateType.eRespawn)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_currentDelay = 0.0f;
        m_playerScript.RefreshJumps();
        m_playerScript.transform.SetPositionAndRotation(new Vector3(-6.400176f, -4.165f), new Quaternion());
        animation_playing = false;
    }
    public override void onUpdate()
    {
        m_currentDelay += Time.deltaTime;

        if (!animation_playing 
            &&m_currentDelay > m_delayTime)
        {
            animation_playing = true;
            m_playerScript.gameObject.GetComponent<Animator>().Play("Player_Respawn");
        }
    }
    public override void onFinish()
    {
        base.onFinish();
    }

    private PlayerScript m_playerScript;
}
