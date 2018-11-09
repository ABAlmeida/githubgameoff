using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float Seconds_To_Fade = 1.0f;
    float secondsPassed;
    bool fadingIn;
    bool fadingOut;

	// Use this for initialization
	void Start ()
    {
        fadingIn = false;
        fadingOut = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (fadingIn == true)
        {
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            secondsPassed += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, secondsPassed);

            if (secondsPassed >= 1.0f)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f);
                fadingIn = false;
            }
        }
        else if (fadingOut == true)
        {
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            secondsPassed += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1.0f - secondsPassed));

            if (secondsPassed >= 1.0f)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f);
                fadingOut = false;
            }
        }
    }

    public void FadeIn()
    {
        secondsPassed = 0.0f;
        fadingIn = true;
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0 );
    }

    public void FadeOut()
    {
        secondsPassed = 0.0f;
        fadingOut = true;
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 255);
    }
}
