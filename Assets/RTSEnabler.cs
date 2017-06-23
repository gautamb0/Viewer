using UnityEngine;
using System.Collections;
using Leap.Unity;

public class RTSEnabler : MonoBehaviour {
    public LeapRTS RTS;
    public RTSToggler Toggler;
    [HideInInspector]
    public int index;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


	}

    public void DisableRTS()
    {
        RTS.enabled = false;
    }

    public void EnableRTS()
    {
        if (RTS.enabled)
            return;
        RTS.enabled = true;
        Toggler.EnableIndex(index);
    }
}
