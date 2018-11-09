using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishZone : MonoBehaviour
{
    bool hasFinished;
    float timePassed;

	// Use this for initialization
	void Start ()
    {
        hasFinished = false;
        timePassed = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (hasFinished)
        {
            timePassed += Time.deltaTime;

            if (timePassed > 5.0f)
            {
                SceneManager.LoadScene("TestMenu");
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        PlayerScript playerScript = collider2D.gameObject.GetComponent<PlayerScript>();

        if (playerScript)
        {
            playerScript.SetNextState(StateType.eUncontrollable);
        }

        hasFinished = true;
    }
}
