using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool m_isOpen = false;
    private float fadeTime = 0.5f;
    private float currentFadeTime;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (m_isOpen)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();

            currentFadeTime += Time.deltaTime;

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f - (currentFadeTime / fadeTime));

            if (currentFadeTime > fadeTime)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f);
            }
        }
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!m_isOpen
            && collision.gameObject.tag == "Player")
        {
            List<GameObject> collectibles = collision.gameObject.GetComponent<PlayerScript>().m_collectibles;
            for (int i = 0; i < collectibles.Count; ++i)
            {
                if (collectibles[i])
                {
                    Collectible collectible = collectibles[i].GetComponent<Collectible>();
                    if (collectible.m_isCollected
                        && !collectible.m_isUsed)
                    {
                        collectible.Open(gameObject);                        
                        break;
                    }
                }
            }            
        }
    }

    public void Open()
    {
        m_isOpen = true;
        currentFadeTime = 0.0f;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
