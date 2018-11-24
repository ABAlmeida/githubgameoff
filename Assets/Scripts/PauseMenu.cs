using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        UnPauseGame();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (m_pauseReleased
            && Input.GetAxis("Pause") > 0.0f)
        {
            if (m_isPaused)
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }

            m_pauseReleased = false;
        }

        if (Input.GetAxis("Pause") == 0.0f)
        {
            m_pauseReleased = true;
        }
	}

    void PauseGame()
    {
        Time.timeScale = 0.0f;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        m_isPaused = true;
    }

    void UnPauseGame()
    {
        Time.timeScale = 1.0f;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        m_isPaused = false;
    }

    private bool m_isPaused;
    private bool m_pauseReleased;
}
