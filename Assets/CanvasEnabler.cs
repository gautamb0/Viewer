using UnityEngine;
using System.Collections;


public class CanvasEnabler : MonoBehaviour {
    public Canvas CanvasToToggle;
    public GameObject CapsuleHand_L;
    public GameObject CapsuleHand_R;
    public float TimeUntilDisable = 1f; //Amount of time for the hands to be out of sight before disabling the canvas
    public float TimeUntilEnable = 0.5f;
    public bool DisableChildren;
    float handsGoneTime, handsActiveTime;
    bool handsGone;
    bool canvasEnabled;
    bool initialized;
    // Use this for initialization
    void Start () {
        handsGoneTime = 0;
        handsActiveTime = 0;
        canvasEnabled = false;
        CanvasToToggle.enabled = canvasEnabled;
        handsGone = true;
        initialized = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Check whether leap motion hands are active
        if (CapsuleHand_L.activeInHierarchy || CapsuleHand_R.activeInHierarchy)
        {
            if(handsGone) //If the hands just became active, start keeping count of the duration that they have been active for
            {
                handsGone = false;
                handsActiveTime = Time.time;
                handsGoneTime = 0f;
            }
           
            if (!canvasEnabled && Time.time - handsActiveTime > TimeUntilEnable)
            {
                EnableCanvas();
                if(!initialized)
                {
                    GetComponent<AnchorToCamera>().ResetRotation(); //Reset rotation on the first enable, so that the canvas ends up in a good position
                    initialized = true;
                }
                    
            }
        }
        else
        {   
            //Keep track of the amount of time that the hands are gone
            if(!handsGone)
            {
                handsGone = true;
                handsActiveTime = 0f;
                handsGoneTime = Time.time;
            }

            //Disable the canvas if the leap motion hands have been off the screen for the given amount
            if (canvasEnabled && Time.time - handsGoneTime > TimeUntilDisable) 
                DisableCanvas();
            
        }
    }

    void DisableCanvas()
    {
        canvasEnabled = false;
        CanvasToToggle.enabled = false;
        if (DisableChildren)
        {
            foreach (Transform child in transform) //Disable the elements in the canvas
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    void EnableCanvas()
    {
        canvasEnabled = true;
        CanvasToToggle.enabled = true;
        if (DisableChildren)
        {
            foreach (Transform child in transform) //Enable the elements in the canvas
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
