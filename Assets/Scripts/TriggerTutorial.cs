using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorial : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TutorialFade tt = gameObject.GetComponentInChildren<TutorialFade>();

            tt.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TutorialFade tt = gameObject.GetComponentInChildren<TutorialFade>();

            tt.FadeOut();
        }
    }
}
