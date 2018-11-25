using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeFog : MonoBehaviour
{
    public GameObject Camera_With_Fog;
    float secondsPassed = Ghost.Seconds_To_Fade;
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
            float percent = 1 / Ghost.Seconds_To_Fade;
            fogPE.Color = new Color(secondsPassed * percent, secondsPassed * percent, secondsPassed * percent, 0.5f);

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
            float percent = 1 / Ghost.Seconds_To_Fade;
            fogPE.Color = new Color(secondsPassed * percent, secondsPassed * percent, secondsPassed * percent, 0.5f);

            if (secondsPassed >= Ghost.Seconds_To_Fade)
            {
                fogPE.Color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                fadingOut = false;
                secondsPassed = Ghost.Seconds_To_Fade;
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
