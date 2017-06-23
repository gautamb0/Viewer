using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour
{
    Color color;
    public float secondsToFade = 0.5f;
    public float startTime = 0f;
    public float startTint = 1;
    public float endTint = .078f;
    float activationTime, timeSinceActive;
    float rgba;
    bool fading = false;
    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        timeSinceActive = Time.time - activationTime;

        if (fading && timeSinceActive < (secondsToFade / 2))
        {
            rgba = Mathf.Lerp(startTint, endTint, 2f*timeSinceActive / secondsToFade);
            color = new Color(rgba, rgba, rgba, rgba);
        }
        else if (fading && timeSinceActive > (secondsToFade / 2))
        {
            rgba = Mathf.Lerp(endTint, startTint, 2f*(timeSinceActive / secondsToFade-.5f));
            color = new Color(rgba, rgba, rgba, 1f);
        }
        else
        {
            fading = false;
        }
        if (fading)
        {
            Debug.Log("fading-"+color);
            GetComponent<Renderer>().material.SetColor("_Tint", color);
            GetComponent<Renderer>().material.SetColor("_Color", color);
        }

    }

    public void DoFade()
    {

        rgba = startTint;
        color = new Color(rgba, rgba, rgba, 1.0f);
        GetComponent<Renderer>().material.SetColor("_Tint", color);
        GetComponent<Renderer>().material.SetColor("_Color", color);
        activationTime = Time.time;
        fading = true;
        Debug.Log("Starting to tint at " + activationTime);
    }
}
