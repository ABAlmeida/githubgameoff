using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeTransform : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            return;
        }

        Transform transform = gameObject.GetComponent<Transform>();
        transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector3(worldScreenWidth / width * Scale.x, worldScreenHeight / height * Scale.y, 1);
        transform.localPosition = new Vector3((worldScreenWidth * 0.5f) - (worldScreenWidth * Position.x), (worldScreenHeight * 0.5f) - (worldScreenHeight * Position.y), 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public Vector2 Position;
    public Vector2 Scale;
}
