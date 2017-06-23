using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class DirectoryObject : UIObject {
    public ListingHandler listingHandler;
  
    public void Show()
    {
        if (isEnabled)
            listingHandler.DisplayPath(path);
    }

    public override void SetPath(string p)
    {
        path = p;
        Text uiText = GetComponentInChildren<Text>();

        //Special directories (My Computer) won't return a proper directory name, so set the button text to the "full path"
        //For everything else, get the directory name
        if (Path.GetDirectoryName(p) != null)
            uiText.text = new DirectoryInfo(path).Name;
        else
            uiText.text = p;
    }
}