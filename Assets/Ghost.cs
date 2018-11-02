using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    
	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis("Ghost") > 0.0f)
        {
            gameObject.SetActive(true);
        }

        if (Input.GetAxis("Ghost") == 0.0f)
        {
            gameObject.SetActive(false);
        }
    }
}
