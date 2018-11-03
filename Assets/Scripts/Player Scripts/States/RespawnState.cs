using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnState : State
{
    public RespawnState(PlayerScript playerScript) : base(StateType.eRespawn)
    {
        m_playerScript = playerScript;
    }
    public override void onStart() { base.onStart(); m_playerScript.RefreshJumps(); }
    public override void onUpdate()
    {
        m_playerScript.transform.SetPositionAndRotation(new Vector3(0.0f, 1.0f), new Quaternion());
        m_playerScript.SetNextState(StateType.eFalling);
    }
    public override void onFinish() { base.onFinish(); }

    private PlayerScript m_playerScript;
}
