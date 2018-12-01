using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnState : State
{
    public RespawnState(PlayerScript playerScript) : base(StateType.eRespawn)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
        m_playerScript.RefreshJumps();
        Vector2 spawn = m_playerScript.GetSpawnLocation();

        m_playerScript.transform.SetPositionAndRotation(new Vector3(spawn.x, spawn.y), new Quaternion());
    }
    public override void onUpdate()
    {
        m_playerScript.gameObject.GetComponent<Animator>().Play("Player_Respawn");
    }
    public override void onFinish()
    {
        base.onFinish();
    }

    private PlayerScript m_playerScript;
}
