using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorial : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TutorialFade[] tt = gameObject.GetComponentsInChildren<TutorialFade>(true);

            if (tt[0])
            {
                tt[0].FadeIn();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TutorialFade tt = gameObject.GetComponentInChildren<TutorialFade>();

            if (tt)
            {
                tt.FadeOut();
            }
        }
    }
}
