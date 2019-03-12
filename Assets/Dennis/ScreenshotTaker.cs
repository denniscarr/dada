using UnityEngine;
using System.IO;

public class ScreenshotTaker : MonoBehaviour {
    //ScreenCapDirectory: If you want a 
    //specific directory do something like this: "C:\\Users\\YourUserNameGoesHere\\Documents\\"
    //Or if you want to use Application.persistentDataPath put it in the 
    //void Start() method and leave this string empty.
    private string screenshotDirectory;

    public string screenCapName = "CrashImage";

    public string fileType = ".png";

    // Used to get how many screenshots exist
    private int count;
    private int screenCapAmount;
    private int screenshotNumber = 1;

    private float timer = 0f;
    private const float SCREENSHOT_INTERVAL = 5f;

    void Start() {
        //Set them both to 0 at start
        count = 0;
        screenCapAmount = 0;

        // If we have to use persistentDataPath, the file path is C:\Users\YourUserNameHere\AppData\LocalLow\YourEnteredCompanyNameHere
        screenshotDirectory = Application.dataPath;
    }

    void Update() {
        //ScreenCaps: Say we have 2 files with the same name as your ScreenCapName,
        //            Well then this would just tell us 2 of those files exist.
        //            Then we add that value to our ScreenCaps number to reference later.
        screenCapAmount = FindScreenCaptures(screenshotDirectory, screenCapName);

        timer += Time.deltaTime;
        if (timer >= SCREENSHOT_INTERVAL) {
            CaptureScreenshot();
            timer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.F12)) {
            CaptureScreenshot();
        }
    }

    private void CaptureScreenshot() {
        //This is how you save the screenshot to a certain directory and a certain name
        //(ScreenCaps + 1): We reference this from above and use it for our picture name
        //                  So if we know 2 files exist we add 1 to our value so it is a new picture.
        //ScreenCapture.CaptureScreenshot(screenshotDirectory + screenCapName + (screenCapAmount + 1) + fileType);
        ScreenCapture.CaptureScreenshot(screenshotDirectory + screenCapName + screenshotNumber + fileType);
        screenshotNumber++;
        if (screenshotNumber > 5) { screenshotNumber = 1; }
    }

    //This gets all the existing files from the Directory (DirectoryPath)
    //with the FileName(FileName).
    int FindScreenCaptures(string DirectoryPath, string FileName) {
        //Set count to 0 at every run so we count up from 0 to 
        //how many files exist with the FileName entered
        count = 0;

        //This loops through the files in your entered Directory
        for (int i = 0; i < Directory.GetFiles(DirectoryPath).Length; i++) {
            //If any file has the same name as your picture
            if (Directory.GetFiles(DirectoryPath)[i].Contains(FileName)) {
                //Add 1 to the count because we need to know how many
                //files with the same name exist
                count += 1;
            }
        }
        //Once we know we return that amount
        return count;
    }
}