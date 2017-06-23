using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadImages : MonoBehaviour
{

    float progress;
    public GameObject Sphere;
    float delay = 2f;
    IEnumerator LoadImageCo(string url, System.Action<Texture2D> OnDone)
    {
        //int r = Random.Range(2, 7);
        //url = "file://C:\\Users\\VR\\Pictures\\360 Photos\\R0010282.JPG";
        // ensure new download by adding random parameter
        //url += (Random.Range(2, 7).ToString())+".JPG";
        progress = 0.0f;
        var www = new WWW(url);
        while (!www.isDone)
        {
            progress = www.progress;
            yield return null;
        }
        progress = 1.0f;
        if (string.IsNullOrEmpty(www.error))
        {
            if (OnDone  != null)
            {
                Texture2D temp = www.texture;
                www.Dispose(); www = null;
                //temp.Compress(true);
                OnDone(temp);
            }
        }
        else
        {
            if (OnDone != null)
            {
                OnDone(null);
            }
        }
    }
    void LoadImage(string url)
    {
        StartCoroutine(LoadImageCo(url, image => {
            Resources.UnloadUnusedAssets();
            Sphere.GetComponent<Renderer>().material.mainTexture = image;
        }));
    }


    void Start()
    {
        /*while (true)
        {
            yield return LoadImage("");
            yield return new WaitForSeconds(delay);
        }*/
        LoadImage("");

    }
    /*
    void OnGUI()
    {
        if (currentImage != null)
            GUILayout.Box(currentImage);
        else
            GUILayout.Label("No image");
        if (progress < 1.0f)
            GUILayout.HorizontalSlider(progress, 0f, 1f);
    }*/
}