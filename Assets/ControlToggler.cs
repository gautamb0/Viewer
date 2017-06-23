using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Leap.Unity;

public class ControlToggler : MonoBehaviour {
    public Text text;
    public GameObject defaultObject;
    public GameObject otherObject;
    public string defaultText;
    public string otherText;
    bool state;


    public void Play()
    {
        state = false;
        Toggle();
    }
    public void Toggle()
    {
        if(state)
        {
            if(otherObject!=null)
                otherObject.SetActive(true);
            defaultObject.SetActive(false);
            text.text = otherText;
            state = false;
        }
        else
        {
            defaultObject.SetActive(true);
            if (otherObject != null)
                otherObject.SetActive(false);
            text.text = defaultText;
            state = true;
        }
    }

}
