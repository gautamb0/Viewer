using UnityEngine;
using System.Collections;

public class RTSToggler : MonoBehaviour {

    public RTSEnabler[] RTSArray;
    int lastEnabled;
	// Use this for initialization
	void Start () {
	    for(int i = 0; i<RTSArray.Length;i++)
        {
            RTSArray[i].index = i;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EnableIndex(int index)
    {
        if (index == lastEnabled)
            return;
        RTSArray[lastEnabled].DisableRTS();
        lastEnabled = index;
    }
}
