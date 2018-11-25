using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool m_isCollected;
    public bool m_isUsed;
    public GameObject Player_Object;
    private Vector2 m_spawnLocation;
    private Transform m_transform;
    private Transform m_target;
    private float dampTime = 0.4f;
    private Vector3 m_velocity = Vector3.zero;
    private float m_offset = 0.1f;

    // Use this for initialization
    void Start ()
    {
        m_isCollected = false;
        m_isUsed = false;
        m_transform = GetComponent<Transform>();
        m_spawnLocation = m_transform.position;
        m_target = Player_Object.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (m_isCollected)
        {
            Vector3 point = m_target.position;
            if (m_target.localScale.x > 0.0f)
            {
                point.x -= m_offset;
            }
            else
            {
                point.x += m_offset;
            }

            Vector3 delta = point - m_transform.position;
            Vector3 destination = point + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref m_velocity, dampTime);

            if (m_isUsed)
            {
                Destroy(gameObject);
            }
        }
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_isCollected
            && collision.GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            m_isCollected = true;
        }
    }

    public void Reset()
    {
        if (!m_isUsed)
        {
            m_transform.position = m_spawnLocation;
            m_isCollected = false;
        }
    }
}
