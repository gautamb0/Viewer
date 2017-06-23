using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VideoObject : FileObject {

    public override void PlayFlat()
    {
        flatMovie._filename = path;
        flatMovie.LoadMovie(true);
        FlatVideoControls.SetActive(true);
        //1 pixel = ~ 1mm, 1000 = meter

        //If some dimension is bigger than 2000, move the flatscreen away by half that dimension so the user can see more of it. (Arbitrary)
        //Default is 1 meter away
        float distance = 1f;
        FlatScreen.transform.localScale = new Vector3(flatMovie.GetWidth() / 1000f, flatMovie.GetHeight() / 1000f, 1f);
        if (Mathf.Max(flatMovie.GetWidth(), flatMovie.GetHeight()) > 2000)
        {
            distance = Mathf.Max(flatMovie.GetWidth(), flatMovie.GetHeight()) / 2000f;
        }
        FlatScreen.transform.localPosition = new Vector3(0, 0, distance);
        FlatScreen.transform.parent.gameObject.SetActive(true);
        flatScreenToggleButton.gameObject.SetActive(true);
        flatScreenToggleButton.Play();
    }

    public override void PlaySpherical()
    {
        Fader fader = Sphere.GetComponent<Fader>();
        fader.DoFade();

        movie._filename = path;
        movie.LoadMovie(true);
        VideoControls.SetActive(true);
        toggleButton.gameObject.SetActive(true);
        toggleButton.Play();
        sphereAnchorer.ResetRotation();
    }

    //TODO: High priority - Load video smoothly
 
}
