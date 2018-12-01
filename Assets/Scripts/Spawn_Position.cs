using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Position : MonoBehaviour
{
    public Vector2 Zone_Spawn_Position = new Vector2(0.0f, 0.0f);
    public Vector2 Key_Spawn_Position = new Vector2(0.0f, 0.0f);
    public bool Change_Spawn_With_Key = false;

    public Vector2 GetSpawnPosition()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        List<GameObject> collectibles = go.GetComponent<PlayerScript>().m_collectibles;
        for (int i = 0; i < collectibles.Count; ++i)
        {
            if (collectibles[i])
            {
                Collectible collectible = collectibles[i].GetComponent<Collectible>();
                if (collectible.m_isCollected
                    && !collectible.m_isUsed)
                {
                    return Key_Spawn_Position;
                }
            }
        }

        return Zone_Spawn_Position;
    }
}
