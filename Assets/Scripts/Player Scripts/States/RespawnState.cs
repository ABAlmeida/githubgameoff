using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnState : State
{
    bool animation_playing;

    public RespawnState(PlayerScript playerScript) : base(StateType.eRespawn)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_playerScript.RefreshJumps();
        m_playerScript.transform.SetPositionAndRotation(new Vector3(m_playerScript.Spawn_Location.x, m_playerScript.Spawn_Location.y), new Quaternion());
        animation_playing = false;
    }
    public override void onUpdate()
    {
        animation_playing = true;
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_Respawn");
    }
    public override void onFinish()
    {
        base.onFinish();
    }

    private PlayerScript m_playerScript;
}
