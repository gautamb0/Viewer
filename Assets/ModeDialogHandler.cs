using UnityEngine;
using System.Collections;

public class ModeDialogHandler : MonoBehaviour {

    [HideInInspector]
    public FileObject fileObject;

	// Use this for initialization
	void Start () {
        
	}
	
    void OnActivate()
    {
        
    }
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayFlat()
    {
       
        fileObject.PlayFlat();
        
        gameObject.SetActive(false);
    }

    public void PlaySpherical()
    {
       
        fileObject.PlaySpherical();
        
        gameObject.SetActive(false);
    }
}
