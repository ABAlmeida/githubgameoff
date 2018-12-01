using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHack : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<Animator>().Play("Player_Idle");
        Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
