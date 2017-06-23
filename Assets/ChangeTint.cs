using UnityEngine;
using System.Collections;

public class ChangeTint : MonoBehaviour
{
    Color color;
    public float secondsToFade = 1;
    public float startTime = 10f;
    public float startTint = .5f;
    public float endTint = .078f;
    float activationTime, timeSinceActive;
    float rgba;
    bool tinting = false;
    public bool tintOnActivate = false;
    // Use this for initialization
    void Start()
    {
        if (tintOnActivate)
        {
            Tint(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceActive = Time.time - activationTime;
        if (timeSinceActive <= startTime)
        {
            rgba = startTint;
            color = new Color(rgba, rgba, rgba, 1.0f);
            GetComponent<Renderer>().material.SetColor("_Tint", color);
            GetComponent<Renderer>().material.SetColor("_Color", color);
        }
        else if (timeSinceActive > startTime && timeSinceActive < startTime + secondsToFade)
        {
            rgba = Mathf.Lerp(startTint, endTint, (timeSinceActive - startTime) / secondsToFade);
            color = new Color(rgba, rgba, rgba, 1.0f);
            GetComponent<Renderer>().material.SetColor("_Tint", color);
            GetComponent<Renderer>().material.SetColor("_Color", color);

        }

    }

    public void Tint(bool invert)
    {
        if (invert)
        {
            Debug.Log("invert");
            float temp = startTint;
            startTint = endTint;
            endTint = temp;
        }
        rgba = startTint;
        color = new Color(rgba, rgba, rgba, 1.0f);
        GetComponent<Renderer>().material.SetColor("_Tint", color);
        GetComponent<Renderer>().material.SetColor("_Color", color);
        activationTime = Time.time;

        Debug.Log("Starting to tint at " + activationTime + " invert " + invert);
    }
}
