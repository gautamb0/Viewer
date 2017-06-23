using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeRotation : MonoBehaviour {
    public GameObject ObjectToRotate;
    public Slider rotationSlider;
    Quaternion originalRotation;

    // Use this for initialization
    void Start () {
        Reset();
    }
	
    void OnEnable()
    {
        
    }
	// Update is called once per frame
	void Update () {
	
	}

    public void OnSliderChange()
    {
        float offset = rotationSlider.value - 180f;
        //Debug.Log("Current y rotation: " + ObjectToRotate.transform.eulerAngles.y);
       // Debug.Log("Original y rotation: " + originalRotation.eulerAngles.y);
        ObjectToRotate.transform.eulerAngles = new Vector3(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y-offset, originalRotation.eulerAngles.z);
    }

    public void Reset()
    {
        originalRotation = ObjectToRotate.transform.rotation;
        rotationSlider.value = 180;
    }
}
