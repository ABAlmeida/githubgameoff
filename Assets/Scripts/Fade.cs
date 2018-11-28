using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
            TilemapRenderer tilemap = GetComponentInChildren<TilemapRenderer>();

            secondsPassed += Time.deltaTime;
            float percent = 1 / Ghost.Seconds_To_Fade;
            Color color = tilemap.material.color;
            color.a = secondsPassed * percent;

            if (secondsPassed >= Ghost.Seconds_To_Fade)
            {
                color = new Color(color.r, color.g, color.b, 1.0f);
                fadingIn = false;
                secondsPassed = Ghost.Seconds_To_Fade;
            }

            tilemap.material.color = color;
        }

        if (fadingOut == true)
        {
            TilemapRenderer tilemap = GetComponentInChildren<TilemapRenderer>();

            secondsPassed -= Time.deltaTime;
            float percent = 1 / Ghost.Seconds_To_Fade;
            Color color = tilemap.material.color;
            color.a = secondsPassed * percent;

            if (secondsPassed <= 0.0f)
            {
                color = new Color(color.r, color.g, color.b, 0.0f);
                fadingOut = false;
                secondsPassed = 0.0f;
                gameObject.SetActive(false);
            }

            tilemap.material.color = color;

            if (percent <= 0.8f)
            {
                setPhysicsActive(false);
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
        TilemapCollider2D tmc2d = gameObject.GetComponent<TilemapCollider2D>();

        if (bc2d)
        {
            bc2d.enabled = value;
        }

        if (ec2d)
        {
            ec2d.enabled = value;
        }

        if (tmc2d)
        {
            tmc2d.enabled = value;
        }
    }
}
