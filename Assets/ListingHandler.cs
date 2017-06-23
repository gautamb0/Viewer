using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Text;
using Leap.Unity.InputModule;
using System.Collections.Generic;
using System.Linq;

public enum EntryType { video, picture, executable, drive, directory };
public struct Entry : IComparable
{
    public string name;
    public EntryType type;
    public Entry(string n, EntryType t)
    {
        name = n;
        type = t;
    }

    public int CompareTo(object Item)
    {
        Entry that = (Entry)Item;
        return name.CompareTo(that.name);

    }

}

public class ListingHandler : MonoBehaviour {
    FileListHandler fileListHandler;
    public UIObject videoUIObject;
    public UIObject pictureUIObject;
    public UIObject directoryUIObject;
    public UIObject driveUIObject;
    public GameObject upButton;
    public Text pathText;
    float lastNavigateUp;
    string currentPath;
	// Use this for initialization
	void Start () {
        lastNavigateUp = 0f;
        DisplayPath("C:\\Users\\Gautam\\Documents\\weddingvr");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            
        }

        if (fileListHandler != null)
        {
            if(fileListHandler.Update())
            {
                DisplayUIObjects(fileListHandler.directoryNames);
                DisplayUIObjects(fileListHandler.fileNames);
                StartCoroutine(Slideshow(fileListHandler.fileNames));
                fileListHandler = null;
            }
        }
    }

    public void NavigateUp()
    {
        if (Time.time - lastNavigateUp < .4f)
            return;
        lastNavigateUp = Time.time;
        if (currentPath == "Desktop") //Don't do anything if on the desktop
            return;

        if (currentPath == "This PC") //This PC is a special case and has no DirectoryInfo, go to Desktop
        {
            DisplayPath("Desktop");
            return;
        }
        if (currentPath == "Documents"|| 
            currentPath == "Downloads"||
            currentPath == "Pictures" ||
            currentPath == "Videos") //Special folders go to This PC
        {
            DisplayPath("This PC");
            return;
        }
        var info = new DirectoryInfo(currentPath);
        if (info.Parent != null)
            DisplayPath(info.Parent.FullName);
        else //If the current path has no parent, it is a drive, so display the This PC listing. Drives do have a valid DirectoryInfo
            DisplayPath("This PC");
    }
    
    public void DisplayPath(string path)
    {
        /*if (path == "Desktop")
        {
            //upButton.GetComponent<CompressibleUI>().Expand();
            //upButton.GetComponent<CompressibleUI>().enabled = false;
            upButton.GetComponent<Button>().interactable = false; //Cannot navigate above Desktop, so disable the up button

        }
            
        else
        {
            upButton.GetComponent<Button>().interactable = true;
            upButton.GetComponent<CompressibleUI>().enabled = true;
        }*/
            
        ClearUIObjects(); //Clear the panel of all the objects from the current directory
        fileListHandler = new FileListHandler(path);
        fileListHandler.Start();
        currentPath = path; 
        pathText.text = currentPath;
    }

    
    /*Takes an ArrayList of object to be displayed (files, directories, or drives), and a GameObject to correspond with the items.
     * Places these items on the UI Panel.
     */
    void DisplayUIObjects(ArrayList displayedObjects, UIObject uiObject)
    {

        foreach (string objectName in displayedObjects)
        {
            var currentObject = Instantiate(uiObject);
            //Size, scale and rotation are not set gracefully automatically, set them appropropriately:
            currentObject.transform.SetParent(this.transform);
            currentObject.transform.localScale = Vector3.one;
            currentObject.transform.localRotation = Quaternion.identity;
            Vector3 tempPos = currentObject.transform.localPosition;
            tempPos.z = 0;
            currentObject.transform.localPosition = tempPos;

            //Set the object's path and set the GameObject active
            currentObject.SetPath(objectName);
            currentObject.gameObject.SetActive(true);
        }
    }

    void DisplayUIObjects(ArrayList displayedObjects)
    {
        UIObject uiObject;
        foreach (Entry entry in displayedObjects )
        {
            string objectName = entry.name;
            if (entry.type == EntryType.picture)
            {
                uiObject = pictureUIObject;
       
            }
            else if (entry.type == EntryType.video)
                uiObject = videoUIObject;
            else if (entry.type == EntryType.drive)
                uiObject = driveUIObject;
            else
                uiObject = directoryUIObject;
            var currentObject = Instantiate(uiObject);
            //Size, scale and rotation are not set gracefully automatically, set them appropropriately:
            currentObject.transform.SetParent(this.transform);
            currentObject.transform.localScale = Vector3.one;
            currentObject.transform.localRotation = Quaternion.identity;
            Vector3 tempPos = currentObject.transform.localPosition;
            tempPos.z = 0;
            currentObject.transform.localPosition = tempPos;

            //Set the object's path and set the GameObject active
            currentObject.SetPath(objectName);
            currentObject.gameObject.SetActive(true);



        }
    }

    IEnumerator Slideshow(ArrayList displayedObjects)
    {
        while (true)
        {
            foreach (FileObject file in this.GetComponentsInChildren<FileObject>())
            {
                file.PlaySpherical();
                yield return new WaitForSeconds(12f);
            }
            /*foreach (VideoObject video in this.GetComponentsInChildren<VideoObject>())
            {
                Debug.Log("start");
                //picture.PlayFlat();
                video.PlaySpherical();
                //while(video.movie.MovieInstance.PositionSeconds < video.movie.MovieInstance.DurationSeconds)
                  //  yield return null;
                yield return new WaitForSeconds(15f);
                Debug.Log("end");
            }
            foreach (PictureObject picture in this.GetComponentsInChildren<PictureObject>())
            {
                Debug.Log("start");
                //picture.PlayFlat();
                picture.PlaySpherical();
                yield return new WaitForSeconds(4f);
                Debug.Log("end");
            }*/

        }

    }

    void ClearUIObjects()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}



/*This class handles reading the file and directory listing of directories in a separate thread, so as to not hang up rendering. */
public class FileListHandler : ThreadedJob
{
    public ArrayList videoNames;
    public ArrayList directoryNames;
    public ArrayList driveNames;
    public ArrayList pictureNames;
    public ArrayList fileNames;
    string path;
    [DllImport("shfolder.dll", CharSet = CharSet.Auto)]
    private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);
    private const int MAX_PATH = 260;
    private const int CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019;

    public FileListHandler(string p)
    {
        path = p;
        directoryNames = new ArrayList();
        fileNames = new ArrayList();
    }
    //John Koemer- http://stackoverflow.com/questions/14572041/list-everything-in-desktop-folder
    public static string GetAllUsersDesktopFolderPath()
    {
        StringBuilder sbPath = new StringBuilder(MAX_PATH);
        SHGetFolderPath(IntPtr.Zero, CSIDL_COMMON_DESKTOPDIRECTORY, IntPtr.Zero, 0, sbPath);
        return sbPath.ToString();
    }

    protected override void ThreadFunction()
    {
        //TODO: Low priority - EnumerateFiles instead of GetFiles, monitor pulse, no array (ideally)
        
        if (path == "Desktop") //Desktop is a special case. There are two desktop folders, and This PC needs to be added.
        {

            var currentUserInfo = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)); //The current user's desktop items
            var allUserInfo = new DirectoryInfo(GetAllUsersDesktopFolderPath()); //All user's desktop items

            directoryNames.Add(new Entry("This PC", EntryType.directory));
            AddDirectories(currentUserInfo);
            AddDirectories(allUserInfo);
            AddVideos(currentUserInfo);
            AddVideos(allUserInfo);
            AddPictures(currentUserInfo);
            AddPictures(allUserInfo);

        }
        else if(path == "This PC")
        {

            //directoryNames.Add("Downloads"); //Downloads is annoying; TODO
            //directoryNames.Add("Videos"); //videos annoying; TODO
            directoryNames.Add(new Entry("Desktop", EntryType.directory));
            directoryNames.Add(new Entry("Documents", EntryType.directory));
            directoryNames.Add(new Entry("Pictures", EntryType.directory));
            

           foreach (string drive in Directory.GetLogicalDrives())
           {
               directoryNames.Add(new Entry(drive, EntryType.drive));
           }
        }
        else if(path == "Documents")
        {
            var info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)); //The current user's documents
            AddDirectories(info);
            AddVideos(info);
            AddPictures(info);
        }
        else if (path == "Pictures")
        {
            var info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)); //The current user's documents
            AddDirectories(info);
            AddVideos(info);
            AddPictures(info);
        }
        else
        {
            var info = new DirectoryInfo(path);
            AddDirectories(info);
            AddVideos(info);
            AddPictures(info);
        }
        fileNames.Sort();


    }

    void AddDirectories(DirectoryInfo info)
    {
        var directoryInfo = info.GetDirectories();

        foreach (DirectoryInfo directory in directoryInfo)
        {
            //Debug.Log(directory.FullName);
            directoryNames.Add(new Entry(directory.FullName, EntryType.directory));
        }
    }

    void AddVideos(DirectoryInfo info)
    {
        //Only support mp4 files at the moment

        //TODO: Medium priority - Handle shortcuts to mp4's and folders
        var fileInfo = info.GetFiles("*.mp4");

        foreach (FileInfo file in fileInfo)
        {
            //Debug.Log(file.FullName);
         
            fileNames.Add(new Entry(file.FullName, EntryType.video));
        }
    }

    void AddPictures(DirectoryInfo info)
    {
        //Only support mp4 files at the moment

        //TODO: Medium priority - Handle shortcuts to mp4's and folders
        var fileInfo = GetFilesByExtensions(info, "*.jpg", "*.jpeg", "*.png");

        foreach (FileInfo file in fileInfo)
        {
            //Debug.Log(file.FullName);
           
            fileNames.Add(new Entry(file.FullName, EntryType.picture));
        }
    }


    public IEnumerable<FileInfo> GetFilesByExtensions(DirectoryInfo dir, params string[] extensions)
    {
        if (extensions == null)
            throw new ArgumentNullException("Requires a list of extensions!");
        IEnumerable<FileInfo> files = Enumerable.Empty<FileInfo>();
        foreach (string ext in extensions)
        {
            files = files.Concat(dir.GetFiles(ext));
        }
        return files;
    }

    protected override void OnFinished()
    {

        //Debug.Log("Finished");
    }
}
