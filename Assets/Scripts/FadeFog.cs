using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeFog : MonoBehaviour
{
    public GameObject Camera_With_Fog;
    public float Seconds_To_Fade = 1.0f;
    float secondsPassed = 1.0f;
    bool fadingIn;
    bool fadingOut;

    // Use this for initialization
    void Start()
    {
        fadingIn = false;
        fadingOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingIn == true)
        {
            UB.D2FogsPE fogPE = Camera_With_Fog.GetComponent<UB.D2FogsPE>();
            secondsPassed -= Time.deltaTime;
            fogPE.Color = new Color(secondsPassed, secondsPassed, secondsPassed, 0.5f);

            if (secondsPassed <= 0.0f)
            {
                fogPE.Color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
                fadingIn = false;
                secondsPassed = 0.0f;
            }
        }

        if (fadingOut == true)
        {
            UB.D2FogsPE fogPE = Camera_With_Fog.GetComponent<UB.D2FogsPE>();
            secondsPassed += Time.deltaTime;
            fogPE.Color = new Color(secondsPassed, secondsPassed, secondsPassed, 0.5f);

            if (secondsPassed >= 1.0f)
            {
                fogPE.Color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                fadingOut = false;
                secondsPassed = 1.0f;
            }
        }
    }

    public void FadeIn()
    {
        fadingIn = true;
        fadingOut = false;
    }

    public void FadeOut()
    {
        fadingOut = true;
        fadingIn = false;
    }
}
