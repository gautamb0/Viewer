using UnityEngine;
using System.Collections;

public class ResolutionScaler : MonoBehaviour {
    public Transform ParentTransform;
    Vector2 OriginalSizeDelta;
	// Use this for initialization
	void Start () {
        OriginalSizeDelta = GetComponent<RectTransform>().sizeDelta;
	}
	
	// Update is called once per frame
	void Update () {
	    if(ParentTransform.localScale!=Vector3.one)
        {
            
            float scaleX = ParentTransform.localScale.x * ParentTransform.transform.parent.gameObject.transform.localScale.x;
            float scaleY = ParentTransform.localScale.y * ParentTransform.transform.parent.gameObject.transform.localScale.y;
            gameObject.transform.localScale = new Vector3(.001f / scaleX, .001f / scaleY, .001f / ParentTransform.localScale.z);
            GetComponent<RectTransform>().sizeDelta = new Vector2(OriginalSizeDelta.x * scaleX, OriginalSizeDelta.y * scaleY);
        }
	}
}
