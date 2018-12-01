using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Music : MonoBehaviour
{
    private AudioManager m_audioManager;
    public string background_music_string = "background_music";

	// Use this for initialization
	void Start ()
    {
        m_audioManager = FindObjectOfType<AudioManager>();
        m_audioManager.Stop(background_music_string);
        m_audioManager.Play("ingame_background");
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
