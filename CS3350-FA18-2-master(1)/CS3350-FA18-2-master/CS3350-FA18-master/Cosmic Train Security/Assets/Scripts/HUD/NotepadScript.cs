using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class NotepadScript : MonoBehaviour
{
    //Canvas to activate/deactivate
    [SerializeField]
    Canvas notepadCanvas;

    //public int to change which text is shown on the notepad
    public int notepadNumber = 1;

    //Path of the csv
    //string path = Resources.Load<TextAsset>("CSVFiles/TutorialInstructions").text;

    //Array for showing the Strings
    string[] tutorialStrings = new string[13];

    private void Start()
    {
        //This converts the CSV to a string array to read in the notepad text
        CSVToArray(Application.streamingAssetsPath + "/TutorialInstructions.csv");

        /*
        tutorialStrings[0] = "I can move around using W, A, S, and D.";
        tutorialStrings[1] = "If space pirates board this ship, stay out of their line-of-sight to remain hidden.";
        tutorialStrings[2] = "My laser pistol holds enough charges to kill a space pirate or two. Note-to-self: Buy a laser rifle after this shipment.";
        tutorialStrings[3] = "I can use the ship's vents to navigate around obstacles.";
        tutorialStrings[4] = "If I'm hurt or low on evergy, I can approach these stations to heal or recharge.";
        tutorialStrings[5] = "If I'm trapped, I can blast through walls by shooting nearby explosives.";
        tutorialStrings[6] = "I can collect items by walking over them. These should come in handy during an emergency.";
        tutorialStrings[7] = "Note to Self: Tripwires make directed explosions and only destroy Grunts. I can place them down with the right mouse button.";
        tutorialStrings[8] = "Note to self: Stasis Grenades FREEZE things I throw them at. I can select it from the toolbelt using the scroll wheel or number keys, then throw one with the right mouse button.";
        tutorialStrings[9] = "Remember: Stasis Grenades can freeze people as well as inanimate objects.";
        tutorialStrings[10] = "In the event of a boarding party, reach the bridge and warp into hyperspace.";
        tutorialStrings[11] = "I can aim and shoot my pistol with the left mouse button.";
        tutorialStrings[12] = "It looks like these lockers can give me a weapon or toolbelt items to use later on.";
*/
        //Default setup in case it doesn't apply for some reason
        ChangeNotepadText(3);
    }

    /// <summary>
    /// Trigger Enter to pull up notepad UI
    /// </summary>
    /// <param name="collEnter"></param>
    private void OnTriggerEnter2D(Collider2D collEnter)
    {
        if (collEnter.gameObject.tag == "Player")
        {
            //Changes notepad text to the one stated in inspector
            ChangeNotepadText(notepadNumber);

            //Shows the notepad canvas
            notepadCanvas.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Trigger exit to close the UI
    /// </summary>
    /// <param name="collExit"></param>
    private void OnTriggerExit2D(Collider2D collExit)
    {
        if (collExit.gameObject.tag == "Player")
        {
            //Hides the notepad canvas by deactivating it
            notepadCanvas.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// takes the CSV file and dumps it into the array for use on the UI
    /// </summary>
    /// <param name="filePath"></param>
    private void CSVToArray(string filePath)
    {
        try
        {
            //creates an StreamReader to read lines
            StreamReader input = new StreamReader(filePath);

            //Temp string that we pulled from the file so we can put it in an array
            string allTextFile = input.ReadToEnd();

            //Splits lines and puts it into finalArray
            tutorialStrings = allTextFile.Split('\n');

            //Closes file
            input.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    /// <summary>
    /// Changes the Notepad Text number (this has to run after CSVToArray)
    /// </summary>
    /// <param name="theNumber"></param>
    private void ChangeNotepadText (int theNumber)
    {
        //Ensures the number is a valid one. If it's not valid it defaults to the first line
        /*if (theNumber >0 && theNumber < tutorialStrings.Length)
        {
            //Gets the text component in the Canvas and applies the string to it
            //NOTE: This will not work if a second text field gets put into NotepadCanvas prefab
            notepadCanvas.GetComponentInChildren<Text>().text = tutorialStrings[theNumber - 1];
        }
        else
        {
            //Default String if the if statement doesn't go through
            notepadCanvas.GetComponentInChildren<Text>().text = tutorialStrings[0];
        }*/
        notepadCanvas.GetComponentInChildren<Text>().text = tutorialStrings[theNumber - 1];
    }
}
