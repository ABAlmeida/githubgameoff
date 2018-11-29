using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool m_isOpen = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
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
                        collectible.m_isUsed = true;
                        m_isOpen = true;
                        GetComponent<BoxCollider2D>().enabled = false;
                        break;
                    }
                }
            }            
        }
    }
}
