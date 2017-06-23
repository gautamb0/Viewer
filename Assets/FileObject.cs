using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;

public class FileObject : UIObject {
    public GameObject FlatScreen;
    public ControlToggler toggleButton;
    public ControlToggler flatScreenToggleButton;
    public AnchorToCamera sphereAnchorer;
    //MovieLoader movieLoader;
    //GCHandle _bytesHandle;
    public GameObject ModeDialog;
    public GameObject VideoControls;
    public GameObject FlatVideoControls;
    public AVProWindowsMediaMovie movie;
    public AVProWindowsMediaMovie flatMovie;
    public GameObject Sphere;
    public void Open()
    {
        if (isEnabled)
        {
            ModeDialog.SetActive(true);
            ModeDialog.GetComponent<ModeDialogHandler>().fileObject = this;
        }
    }

    public override void SetPath(string p)
    {
        path = p;
        Text uiText = GetComponentInChildren<Text>();
        uiText.text = Path.GetFileName(p);
    }



    public virtual void PlayFlat() { }
    public virtual void PlaySpherical() { }
    /*
    IEnumerator Load()
    {
        string url = "file://E:\\Users\\GautamB\\Documents\\immers3d\\1.mp4";
        WWW www = new WWW(url);
        while(!www.isDone)
        {
            yield return null;
        }
        _bytesHandle = GCHandle.Alloc(www.bytes, GCHandleType.Pinned);
        movie.LoadMovieFromMemory(false, url, _bytesHandle.AddrOfPinnedObject(), (uint)www.bytes.Length, FilterMode.Bilinear, TextureWrapMode.Clamp);
    }*/
}

/*
public class MovieLoader : ThreadedJob
{

    public AVProWindowsMediaMovie movie;

    protected override void ThreadFunction()
    {

        
    }
}*/