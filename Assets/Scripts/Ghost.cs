using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private bool m_isActive = false;
    private GameObject[] m_ghostObjects;
    private GameObject[] m_realObjects;

    // Use this for initialization
    void Start ()
    {
        m_ghostObjects = GameObject.FindGameObjectsWithTag("Ghost");
        m_realObjects = GameObject.FindGameObjectsWithTag("Real");

        SpriteRenderer sr;
        foreach (GameObject _gameObject in m_ghostObjects)
        {
            _gameObject.SetActive(false);
            sr = _gameObject.GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        float ghosting = Input.GetAxis("Ghost");

        if (!m_isActive
            && ghosting > 0.0f)
        {
            SetFadeOnArray(m_ghostObjects, true);
            SetFadeOnArray(m_realObjects, false);

            m_isActive = true;
        }

        if (m_isActive
            && ghosting == 0.0f)
        {
            SetFadeOnArray(m_ghostObjects, false);
            SetFadeOnArray(m_realObjects, true);

            m_isActive = false;
        }
    }

    void SetFadeOnArray(GameObject[] objectArray, bool value)
    {
        foreach (GameObject _gameObject in objectArray)
        {
            if (value)
            {
                _gameObject.GetComponent<Fade>().FadeIn();
                _gameObject.SetActive(true);
            }
            else
            {
                _gameObject.GetComponent<Fade>().FadeOut();
            }
        }
    }
    void SetActiveOnArray(GameObject[] objectArray, bool value)
    {
        foreach (GameObject _gameObject in objectArray)
        {
            _gameObject.SetActive(value);
        }
    }

}
