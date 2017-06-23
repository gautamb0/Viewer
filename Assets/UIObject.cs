using Leap.Unity;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public abstract class UIObject : MonoBehaviour {
    protected string path;
    public abstract void SetPath(string p);
    public float ActivationDelay = 0.5f; //how long to wait till activating the button. This is to mitigate against accidental double-clicks and clicking on buttons immediately after the canvas is enabled.
    public PinchDetector PinchDetectorA;
    public PinchDetector PinchDetectorB;
    bool delayComplete;
    protected bool isEnabled;
    float activationTime;
    public void OnEnable()
    {
        isEnabled = true;
        delayComplete = false;
        activationTime = Time.time;
        //StartCoroutine("ButtonEnable");
    }

    IEnumerator ButtonEnable()
    {

        yield return new WaitForSeconds(ActivationDelay);
        delayComplete = true;
    }

    void Update()
    {

        /*if(Time.time - activationTime > ActivationDelay)
        {
            isEnabled = true;
        }
        else
        {
            isEnabled = false;
        }
        if (PinchDetectorA.IsPinching | PinchDetectorB.IsPinching)
        {
            isEnabled = false;
            activationTime = Time.time;
        }*/
    }

}
