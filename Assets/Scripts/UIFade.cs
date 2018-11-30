using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFade : MonoBehaviour
{
    private enum UIFadeState
    {
        In = 0,
        Stay = 1,
        Out = 2,
        End = 3
    }

    public GameObject nextItem;
    public string nextScene;

    private float FadeTime = 2.0f;
    private float CurrentFadeTime = 0.0f;
    private TMPro.TextMeshProUGUI TextMeshProLink;
    private UIFadeState m_fadeState;

	// Use this for initialization
	void Start ()
    {
        CurrentFadeTime = 0.0f;
        TextMeshProLink = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        TextMeshProLink.color = new Color(TextMeshProLink.color.r, TextMeshProLink.color.g, TextMeshProLink.color.b, 0.0f);
        m_fadeState = UIFadeState.In;
	}
	
	// Update is called once per frame
	void Update ()
    {
        CurrentFadeTime += Time.deltaTime;

		switch (m_fadeState)
        {
            case UIFadeState.In:
                if (CurrentFadeTime > FadeTime)
                {
                    CurrentFadeTime = 0.0f;
                    m_fadeState = UIFadeState.Stay;
                    TextMeshProLink.color = new Color(TextMeshProLink.color.r, TextMeshProLink.color.g, TextMeshProLink.color.b, 1.0f);
                }
                else
                {
                    TextMeshProLink.color = new Color(TextMeshProLink.color.r, TextMeshProLink.color.g, TextMeshProLink.color.b, CurrentFadeTime / FadeTime);
                }
                break;
            case UIFadeState.Stay:
                if (CurrentFadeTime > FadeTime)
                {
                    CurrentFadeTime = 0.0f;
                    m_fadeState = UIFadeState.Out;
                }
                break;
            case UIFadeState.Out:
                if (CurrentFadeTime > FadeTime)
                {
                    CurrentFadeTime = 0.0f;
                    m_fadeState = UIFadeState.End;
                    TextMeshProLink.color = new Color(TextMeshProLink.color.r, TextMeshProLink.color.g, TextMeshProLink.color.b, 0.0f);
                }
                else
                {
                    TextMeshProLink.color = new Color(TextMeshProLink.color.r, TextMeshProLink.color.g, TextMeshProLink.color.b, 1.0f - (CurrentFadeTime / FadeTime));
                }
                break;
            case UIFadeState.End:
                gameObject.SetActive(false);
                if (nextItem)
                {
                    nextItem.SetActive(true);
                }
                else
                {
                    SceneManager.LoadScene(nextScene);
                }
                break;
        }
	}
}
