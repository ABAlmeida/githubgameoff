using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncontrollableState : State
{

    public UncontrollableState(PlayerScript playerScript) : base(StateType.eUncontrollable)
    {
        m_playerScript = playerScript;
    }
    public override void onStart()
    {
        base.onStart();
    }
    public override void onUpdate()
    {
        
    }
    public override void onFinish()
    {
        base.onFinish();
    }

    private PlayerScript m_playerScript;
}
