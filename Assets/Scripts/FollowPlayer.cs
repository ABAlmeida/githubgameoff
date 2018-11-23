using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float dampTime = 0.15f;
    private Vector3 m_velocity = Vector3.zero;
    public Transform target;
    private Camera m_camera;
    public GameObject Current_Camera_Zone;

    // Use this for initialization
    void Start ()
    {
        m_camera = gameObject.GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (target)
        {
            Vector3 point = m_camera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - m_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref m_velocity, dampTime);
        }

        Vector2 position = Current_Camera_Zone.GetComponent<Transform>().position;
        Vector2 extents = Current_Camera_Zone.GetComponent<BoxCollider2D>().bounds.extents;
        float height = 2f * m_camera.orthographicSize;
        float width = height * m_camera.aspect;
        Vector2 value = extents - (new Vector2(width, height) * 0.5f);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, position.x - value.x, position.x + value.x), 
            Mathf.Clamp(transform.position.y, position.y - value.y, position.y + value.y), 
            transform.position.z);
    }
}
