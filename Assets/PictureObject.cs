
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
public class PictureObject : FileObject {
    Texture2D currentImage = null;


    float progress;
    // Use this for initialization
    void Start()
    {
        progress = 0.0f;

    }




    public override void PlayFlat()
    {
        string url = "file://" + path;
        Debug.Log(url);
        flatMovie.UnloadMovie();

        StartCoroutine(LoadImageCo(url, image =>
        {
            Resources.UnloadUnusedAssets();
            Material tempMaterial = FlatScreen.GetComponent<Renderer>().material;
            //1 pixel = ~ 1mm, 1000 = meter
            FlatScreen.transform.localScale = new Vector3(image.width / 1000f, image.height / 1000f, 1f );
            //If some dimension is bigger than 2000, move the flatscreen away by half that dimension so the user can see more of it. (Arbitrary)
            //Default is 1 meter away
            float distance = 1f;
            if(Mathf.Max(image.width,image.height)>2000)
            {
                distance = Mathf.Max(image.width, image.height) / 2000f;
            }
            FlatScreen.transform.localPosition = new Vector3(0, 0, distance);
            tempMaterial.mainTextureOffset = new Vector2(0, 0);
            tempMaterial.mainTextureScale = new Vector2(1, 1);
            tempMaterial.mainTexture = image;
            FlatScreen.transform.parent.gameObject.SetActive(true);
            FlatVideoControls.SetActive(false);
            flatScreenToggleButton.gameObject.SetActive(true);
            flatScreenToggleButton.Play();
            Debug.Log("sutck");
        }));
    }
        
    public override void PlaySpherical()
    {
        
        movie.UnloadMovie();
             string url = "file://" + path;
        Fader fader = Sphere.GetComponent<Fader>();

        Debug.Log(url);
             StartCoroutine(LoadImageCo(url, image =>
             {
                 fader.DoFade();
                 Resources.UnloadUnusedAssets();
                 Material tempMaterial = Sphere.GetComponent<Renderer>().material;
                 tempMaterial.mainTextureOffset = new Vector2(0, 0);
                 tempMaterial.mainTextureScale = new Vector2(1, 1);
                 tempMaterial.mainTexture = image;
                 
                 //sphereAnchorer.ResetRotation();


                 
                 VideoControls.SetActive(false);
                 toggleButton.gameObject.SetActive(true);
                 toggleButton.Play();
                 Debug.Log("stuck");
             }));
    }

    IEnumerator LoadImageCo(string url, System.Action<Texture2D> OnDone)
    {

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
            if (OnDone != null)
            {
                Texture2D temp = null;

                temp = new Texture2D(www.texture.width, www.texture.height, www.texture.format, false);

               www.LoadImageIntoTexture(temp);
                www.Dispose(); www = null;
                
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
    Coroutine LoadImage(string url)
    {
        return StartCoroutine(LoadImageCo(url, image => {
            Resources.UnloadUnusedAssets();
        }));
    }

    IEnumerator OpenImage(string url)
    {
        yield return LoadImage(url);
    }
}
