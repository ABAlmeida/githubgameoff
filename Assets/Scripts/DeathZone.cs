using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        PlayerScript playerScript = collider2D.gameObject.GetComponent<PlayerScript>();

        if (playerScript)
        {
            playerScript.SetNextState(StateType.eDead);
        }
    }
}
