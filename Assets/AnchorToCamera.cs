using System.Collections;
using UnityEngine;


public class AnchorToCamera : MonoBehaviour {
    public GameObject LMHeadMountedRig;
    public GameObject CenterEyeAnchor;
    public ChangeRotation RotationChanger;
    Vector3 originalPosition;
    Quaternion originalRotation;
   
	// Use this for initialization
	IEnumerator Start () {
        originalPosition = gameObject.transform.localPosition;
        originalRotation = gameObject.transform.localRotation;
        yield return new  WaitForSeconds(1f); //Allow a second for the user to put on the headset
        ResetRotation();
    }

    // Update is called once per frame
    void Update () {

	}
       
    public void ResetRotation()
    {
        /*  First, set the position and rotation to one originally set in the editor.
         * Second, set the parent to the camera to bring the panel directly in front.
         * Third, set the parent to the camera's parent to detach it from the camera.
         * Fourth, zero out the x and z rotations, as we want the panel to be upright.
         */
        gameObject.transform.SetParent(CenterEyeAnchor.transform, false);
        gameObject.transform.localPosition = originalPosition;
        gameObject.transform.localRotation = originalRotation;
        gameObject.transform.SetParent(LMHeadMountedRig.gameObject.transform,true);
        float currentYrotation = gameObject.transform.rotation.eulerAngles.y;
        gameObject.transform.eulerAngles = new Vector3(0, currentYrotation, 0);
        if(RotationChanger!=null)
            RotationChanger.Reset();
    }


}
