using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject Pause_Menu;

	// Use this for initialization
	void Start ()
    {
        UnPauseGame();
    }
	
	// Update is called once per frame
	void Update ()
    {
        bool paused = Input.GetButtonDown("Pause");

		if (paused)
        {
            if (m_isPaused)
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }
        }
	}

    public void PauseGame()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0.0f;
        Pause_Menu.SetActive(true);
        m_isPaused = true;

        EventSystem es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(es.firstSelectedGameObject);
    }

    public void UnPauseGame()
    {
        Debug.Log("Game Unpaused");
        Time.timeScale = 1.0f;
        Pause_Menu.SetActive(false);
        m_isPaused = false;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("TestMenu");
    }

    private bool m_isPaused;
    private bool m_pauseReleased;
}
