using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    float secondsPassed = 0.0f;
    bool fadingIn = false;
    bool fadingOut = false;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		if (fadingIn == true)
        {
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            secondsPassed += Time.deltaTime;
            float percent = 1 / Ghost.Seconds_To_Fade;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, secondsPassed * percent);

            if (secondsPassed >= Ghost.Seconds_To_Fade)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f);
                fadingIn = false;
                secondsPassed = Ghost.Seconds_To_Fade;
            }
        }

        if (fadingOut == true)
        {
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            secondsPassed -= Time.deltaTime;
            float percent = 1 / Ghost.Seconds_To_Fade;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, secondsPassed);

            if (percent <= 0.8f)
            {
                setPhysicsActive(false);
            }

            if (secondsPassed <= 0.0f)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f);
                fadingOut = false;
                secondsPassed = 0.0f;
                gameObject.SetActive(false);
            }
        }
    }

    public void FadeIn()
    {
        fadingIn = true;
        fadingOut = false;

        setPhysicsActive(true);
    }

    public void FadeOut()
    {
        fadingOut = true;
        fadingIn = false;
    }

    public void setPhysicsActive(bool value)
    {
        BoxCollider2D bc2d = gameObject.GetComponent<BoxCollider2D>();
        EdgeCollider2D ec2d = gameObject.GetComponent<EdgeCollider2D>();

        if (bc2d)
        {
            bc2d.enabled = value;
        }

        if (ec2d)
        {
            ec2d.enabled = value;
        }
    }
}
