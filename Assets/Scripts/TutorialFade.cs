﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFade : MonoBehaviour
{
    private GameObject player;
    private Transform playerTf;
    private SpriteRenderer sr;
    private Transform tf;
    private float currentFadeTime;
    private float fadeTime = 0.5f;

    bool fadingIn;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTf = player.GetComponent<Transform>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        tf = gameObject.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        tf.position = new Vector3(playerTf.position.x, playerTf.position.y + 0.5f, playerTf.position.z);

        if (fadingIn)
        {
            currentFadeTime += Time.deltaTime;
            if (currentFadeTime > fadeTime)
            {
                currentFadeTime = fadeTime;
            }

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, currentFadeTime / fadeTime);
        }
        else
        {
            currentFadeTime -= Time.deltaTime;
            if (currentFadeTime < 0.0f)
            {
                gameObject.SetActive(false);
            }

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, currentFadeTime / fadeTime);
        }
	}

    public void FadeIn()
    {
        gameObject.SetActive(true);
        fadingIn = true;
        currentFadeTime = 0.0f;
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f);
    }

    public void FadeOut()
    {
        fadingIn = false;
    }
}
