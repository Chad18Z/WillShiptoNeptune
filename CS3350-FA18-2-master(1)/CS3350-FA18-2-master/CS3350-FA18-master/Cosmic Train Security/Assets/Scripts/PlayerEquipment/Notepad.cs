using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Notepad : MonoBehaviour
{
    //Canvas to activate/deactivate
    [SerializeField]
    Canvas notepadCanvas;

    //public int to change which text is shown on the notepad
    public int notepadNumber = 1;

    //Path of the csv


    //Array for showing the Strings
    string[] tutorialStrings;
    string notepadText;

    private void Start()
    {
        string path = Application.streamingAssetsPath + "/TutorialInstructions.csv";
        csvToArray(path);
        changeNotepadText(5);
    }

    private void OnTriggerEnter2D(Collider2D collEnter)
    {
        if (collEnter.gameObject.tag == "Player")
        {
            changeNotepadText(notepadNumber);
            notepadCanvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collExit)
    {
        if (collExit.gameObject.tag == "Player")
        {
            notepadCanvas.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// takes the CSV file and dumps it into the array for use on the UI
    /// </summary>
    /// <param name="filePath"></param>
    private void csvToArray(string filePath)
    {
        //creates an StreamReader to read lines
        StreamReader input = new StreamReader(filePath);
        string allTextFile = input.ReadToEnd();

        //Splits lines and puts it into finalArray
        tutorialStrings = allTextFile.Split('\n');

    }

    private void changeNotepadText (int theNumber)
    {
        //Ensures the number is a valid one. If it's not valid
        if (theNumber >0 && theNumber < tutorialStrings.Length)
        {
            //Gets the text component in the Canvas and applies the string to it
            notepadCanvas.GetComponentInChildren<Text>().text = tutorialStrings[theNumber - 1];
        }
        else
        {
            //Default String if the if statement doesn't go through
            notepadCanvas.GetComponentInChildren<Text>().text = tutorialStrings[0];
        }
    }
}
