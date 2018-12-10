using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LoadCSVFiles
{

    public static List<string> enemyQuotesList;
    public static string enemyQuote;


    // Use this for initialization
    public static void Initialize()
    {
        LoadEnemyQuotes();
    }


    public static void LoadEnemyQuotes()
    {
        LoadCSV(Application.streamingAssetsPath +"/EnemyQuotes.csv");
    }

    private static void LoadCSV(string path)
    {
        // initialize csv file directory path and array to store files
        //DirectoryInfo rootDirectory = new DirectoryInfo("CSVFiles");
        // FileInfo[] files = null;
        enemyQuotesList = new List<string>();

        ////add each file in the directory
        //try
        //{
        //    files = rootDirectory.GetFiles("*.*");
        //}
        //catch (IOException e)
        //{
        //    if (e.Source != null)
        //        Debug.Log(e.Message);
        //    throw;
        //}

        ////iterate through each file in Resource/CSVFiles
        //foreach (System.IO.FileInfo fi in files)
        //{
        if (enemyQuotesList != null)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path/*fi.Extension*/))
                {
                    List<string> lines = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        //save each line of file
                        lines.Add(reader.ReadLine());
                    }

                    string[] words;

                    for (int i = 0; i < lines.Count; i++)
                    {
                        words = lines[i].Split(',');

                        foreach (string word in words)
                        {
                            //if (fi.FullName == "EnemyQuotes")
                            enemyQuotesList.Add(word);
                            //else if( file equals some other csv){....}
                        }
                    }

                }
            }
            catch (IOException e)
            {
                Debug.Log(e.Message);
            }
            //}
        }
    }


    public static string GetEnemyQuote
    {
        get
        {
            int percentChance = 100, randomQuoteIndex;

            if (enemyQuotesList == null || enemyQuotesList.Count == 0)
            {
                //Repopulate list
                LoadEnemyQuotes();
            }

            //Frequency of message displayed
            if (Random.Range(1, 100) <= percentChance)
            {
                //set random quote
                randomQuoteIndex = Random.Range(0, (enemyQuotesList.Count - 1));
                enemyQuote = enemyQuotesList[randomQuoteIndex];

                Debug.Log(enemyQuotesList[randomQuoteIndex]);
                //remove quote 
                enemyQuotesList.RemoveAt(randomQuoteIndex);
            }

            return enemyQuote;
        }


    }
}
