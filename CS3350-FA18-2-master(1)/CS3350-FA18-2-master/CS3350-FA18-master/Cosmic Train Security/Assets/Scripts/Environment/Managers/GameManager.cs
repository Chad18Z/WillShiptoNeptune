using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    #region Fields

    // for loading in the tiles easier
    [SerializeField]
    GameObject prefabFloorTile;
    [SerializeField]
    GameObject prefabWallTile;
	[SerializeField]
	GameObject prefabCrate;
	[SerializeField]
	GameObject prefabTable;
    [SerializeField]
    GameObject prefabMoveSpot;

    // Get Camera
    [SerializeField]
	GameObject mainCamera;

    // for instantiating the player
    [SerializeField]
    GameObject prefabEnemy;
    [SerializeField]
    GameObject prefabPlayer;

    // UI Elements
    [SerializeField]
    GameObject prefabParentCanvas;

    // Audio Manager
    [SerializeField]
    GameObject prefabAudioManager;

    SpriteRenderer floorSR;

    bool uninitialized = true;

    #endregion

    #region Initalization
    void Awake()
    {
        if (uninitialized)
        {
            Initialize();
        }
        //Read all CSV files
        //LoadCSVFiles CSVData = gameObject.GetComponent<LoadCSVFiles>();

        

    }
    #endregion

    #region Methods

    /// <summary>
    /// Dynamically creates a 10x10 room for the player to walk it
    /// also dynamically instantiates the player and an enemy
    /// </summary>
    private void Initialize()
    {

        // Instantiate player and other needed prefabs
        Instantiate(prefabPlayer, new Vector3(-42, 21, 0), Quaternion.identity);
        mainCamera.GetComponentInChildren<CameraFollow>().Initialize();

        Instantiate(prefabAudioManager);
        Instantiate(prefabParentCanvas);
        // Get list of prefabs from modules folder in resources and sort them



        List<GameObject> modules = new List<GameObject>(Resources.LoadAll<GameObject>("Modules"));
        List<GameObject> act1 = new List<GameObject>(modules.FindAll(x=>x.name.StartsWith("act1")).ToList());
        List<GameObject> act2 = new List<GameObject>(modules.FindAll(x=>x.name.StartsWith("act2")).ToList());
        List<GameObject> act3 = new List<GameObject>(modules.FindAll(x=>x.name.StartsWith("act3")).ToList());
        List<GameObject> act4 = new List<GameObject>(modules.FindAll(x=>x.name.StartsWith("act4")).ToList());
        List<GameObject> act5 = new List<GameObject>(modules.FindAll(x=>x.name.StartsWith("act5")).ToList());
        List<GameObject> empty = new List<GameObject>(modules.FindAll(x => x.name.Equals("empty_traincar")).ToList());
        modules.OrderBy(x=>x.name);

        Debug.Log("Act 1 size" + act1.Count);
        Debug.Log("Act 2 size" + act2.Count);
        Debug.Log("Act 3 size " + act3.Count);
        Debug.Log("Act 4 size " + act4.Count);
        Debug.Log("Act 5 size" + act5.Count);

        // Instantiate empty train car
        Instantiate(empty[0], new Vector3(0, 0, 0), Quaternion.identity);

        // Make random modules
                // gets random values
        int first = (int)Random.Range(0f, 15f);
        int second = (int)Random.Range(0f, 15f);
        int third = (int)Random.Range(0f, 15f);
        int fourth = (int)Random.Range(0f, 15f);
        int fifth = (int)Random.Range(0f, 12f);
        int sixth = (int)Random.Range(0f, 12f);

        // for first and firth modules
        int start = (int)Random.Range(0f, 1f);
        int end = (int)Random.Range(0f, 1f);

        // Vectors for up and down.
        Vector3 up = new Vector3(0, 1, 0);
        Vector3 down = new Vector3(0, -1, 0);

        //Instantiate(act2[(int)Random.Range(0f, 4f)], new Vector3(-21.25f, 13.75f, 10), Quaternion.identity); // act 2-1
        if ((first > 3 && first <=7) || (first > 11))
        {
            Instantiate(act2[first], new Vector3(-21.25f, 13.75f, 0), Quaternion.FromToRotation(up,down)); // act 2-1
        }
        else
        {
            Instantiate(act2[first], new Vector3(-21.25f, 13.75f, 0), Quaternion.identity);// act 2-1
        }

        // Instantiate(act2[(int)Random.Range(0f, 4f)], new Vector3(-21.25f, -13.75f, 10), Quaternion.identity); // act 2-2
        if ((second <= 3) || (second <= 11 && second > 7))
        {
            Instantiate(act2[second], new Vector3(-21.25f, -13.75f, 0), Quaternion.FromToRotation(down, up)); // act 2-2
        }
        else
        {
            Instantiate(act2[second], new Vector3(-21.25f, -13.75f, 0), Quaternion.identity);// act 2-2
        }

        //Instantiate(act3[(int)Random.Range(0f, 4f)], new Vector3(0, 13.69f, 10), Quaternion.identity); // act 3-1
        if ((third > 3 && third <= 7) || (third > 11))
        {
            Instantiate(act3[third], new Vector3(0, 13.69f, 0), Quaternion.FromToRotation(up, down)); // act 3-1
        }
        else
        {
            Instantiate(act3[third], new Vector3(0, 13.69f, 0), Quaternion.identity);// act 3-1
        }

        //Instantiate(act3[(int)Random.Range(0f, 4f)], new Vector3(0, -13.69f, 10), Quaternion.identity); // act 3-2
        if ((fourth <= 3) || (fourth <= 11 && fourth > 7))
        {
            Instantiate(act3[fourth], new Vector3(0, -13.69f, 0), Quaternion.FromToRotation(down, up)); // act 3-2
        }
        else
        {
            Instantiate(act3[fourth], new Vector3(0, -13.69f, 0), Quaternion.identity); // act 3-2
        }

        //Instantiate(act4[(int)Random.Range(0f, 4f)], new Vector3(21.23f, 13.815f, 10), Quaternion.identity); // act 4-1
        if ((fifth == 4) || (fifth >= 9))
        {
            Instantiate(act4[fifth], new Vector3(21.23f, 13.815f, 0), Quaternion.FromToRotation(up, down)); // act 4-1
        }
        else
        {
            Instantiate(act4[fifth], new Vector3(21.23f, 13.815f, 0), Quaternion.identity); // act 4-1
        }

        //Instantiate(act4[(int)Random.Range(0f, 4f)], new Vector3(21.23f, -13.815f, 10), Quaternion.identity); // act 4-2
        if ((sixth <= 3) || (sixth < 9 && sixth > 4))
        {
            Instantiate(act4[sixth], new Vector3(21.23f, -13.815f, 0), Quaternion.FromToRotation(down, up)); // act 4-2
        }
        else
        {
            Instantiate(act4[sixth], new Vector3(21.23f, -13.815f, 0), Quaternion.identity); // act 4-2
        }

        Instantiate(act1[start], new Vector3(-36.21f, 0, 10), Quaternion.identity); // start module
        Instantiate(act5[end], new Vector3(40, 0, 10), Quaternion.identity); // end module

        //used in case a scene is reloaded and reference exception occurs for GridManager
        GridManager.Instance.Init();

        EventManager.AddInitNewTraincarListener(ReInitialize);

        uninitialized = false;

    }

    /// <summary>
    /// Dynamically creates a 10x10 room for the player to walk it
    /// also dynamically instantiates the player and an enemy
    /// </summary>
    private void ReInitialize()
    {

        //delete current objects
        GameObject[] acts = GameObject.FindGameObjectsWithTag("act");

        foreach (GameObject act in acts)
        {
            Destroy(act);
        }

        GameObject[] grenadeMaterials = GameObject.FindGameObjectsWithTag("Grenade Material");

        foreach (GameObject grenade in grenadeMaterials)
        {
            Destroy(grenade);
        }

        GameObject[] tripWireMaterials = GameObject.FindGameObjectsWithTag("Trip Wire Materials");

        foreach (GameObject tripWire in tripWireMaterials)
        {
            Destroy(tripWire);
        }

        GameObject[] pingMaterials = GameObject.FindGameObjectsWithTag("Ping Materials");

        foreach (GameObject ping in pingMaterials)
        {
            Destroy(ping);
        }

        // Move player to front of train car
        GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position = new Vector3(-42, 21, 0);


        // Get list of prefabs from modules folder in resources and sort them
        List<GameObject> modules = new List<GameObject>(Resources.LoadAll<GameObject>("Modules"));
        List<GameObject> act1 = new List<GameObject>(modules.FindAll(x => x.name.StartsWith("act1")).ToList());
        List<GameObject> act2 = new List<GameObject>(modules.FindAll(x => x.name.StartsWith("act2")).ToList());
        List<GameObject> act3 = new List<GameObject>(modules.FindAll(x => x.name.StartsWith("act3")).ToList());
        List<GameObject> act4 = new List<GameObject>(modules.FindAll(x => x.name.StartsWith("act4")).ToList());
        List<GameObject> act5 = new List<GameObject>(modules.FindAll(x => x.name.StartsWith("act5")).ToList());
        List<GameObject> empty = new List<GameObject>(modules.FindAll(x => x.name.Equals("empty_traincar")).ToList());
        modules.OrderBy(x => x.name);

        Debug.Log("Act 2 size" + act2.Count);
        Debug.Log("Act 3 size " + act3.Count);
        Debug.Log("Act 4 size " + act4.Count);

        // Instantiate empty train car
        Instantiate(empty[0], new Vector3(0, 0, 10), Quaternion.identity);

        // Make random modules
        // gets random values
        int first = (int)Random.Range(0f, 4f);
        int second = (int)Random.Range(0f, 4f);
        int third = (int)Random.Range(0f, 4f);
        int fourth = (int)Random.Range(0f, 4f);
        int fifth = (int)Random.Range(0f, 4f);
        int sixth = (int)Random.Range(0f, 4f);

        // Vectors for up and down.
        Vector3 up = new Vector3(0, 1, 0);
        Vector3 down = new Vector3(0, -1, 0);

        //Instantiate(act2[(int)Random.Range(0f, 4f)], new Vector3(-21.25f, 13.75f, 10), Quaternion.identity); // act 2-1
        if (first == 1 || first == 3)
        {
            Instantiate(act2[first], new Vector3(-21.25f, 13.75f, 0), Quaternion.FromToRotation(up, down)); // act 2-1
        }
        else
        {
            Instantiate(act2[first], new Vector3(-21.25f, 13.75f, 0), Quaternion.identity);// act 2-1
        }

        // Instantiate(act2[(int)Random.Range(0f, 4f)], new Vector3(-21.25f, -13.75f, 10), Quaternion.identity); // act 2-2
        if (second == 0 || second == 2)
        {
            Instantiate(act2[second], new Vector3(-21.25f, -13.75f, 0), Quaternion.FromToRotation(down, up)); // act 2-2
        }
        else
        {
            Instantiate(act2[second], new Vector3(-21.25f, -13.75f, 0), Quaternion.identity);// act 2-2
        }

        //Instantiate(act3[(int)Random.Range(0f, 4f)], new Vector3(0, 13.69f, 10), Quaternion.identity); // act 3-1
        if (third == 1 || third == 3)
        {
            Instantiate(act3[third], new Vector3(0, 13.69f, 0), Quaternion.FromToRotation(up, down)); // act 3-1
        }
        else
        {
            Instantiate(act3[third], new Vector3(0, 13.69f, 0), Quaternion.identity);// act 3-1
        }

        //Instantiate(act3[(int)Random.Range(0f, 4f)], new Vector3(0, -13.69f, 10), Quaternion.identity); // act 3-2
        if (fourth == 0 || fourth == 2)
        {
            Instantiate(act3[fourth], new Vector3(0, -13.69f, 0), Quaternion.FromToRotation(down, up)); // act 3-2
        }
        else
        {
            Instantiate(act3[fourth], new Vector3(0, -13.69f, 0), Quaternion.identity); // act 3-2
        }

        //Instantiate(act4[(int)Random.Range(0f, 4f)], new Vector3(21.23f, 13.815f, 10), Quaternion.identity); // act 4-1
        if (fifth == 1 || fifth == 3)
        {
            Instantiate(act4[fifth], new Vector3(21.23f, 13.815f, 0), Quaternion.FromToRotation(up, down)); // act 4-1
        }
        else
        {
            Instantiate(act4[fifth], new Vector3(21.23f, 13.815f, 0), Quaternion.identity); // act 4-1
        }

        //Instantiate(act4[(int)Random.Range(0f, 4f)], new Vector3(21.23f, -13.815f, 10), Quaternion.identity); // act 4-2
        if (sixth == 0 || sixth == 2)
        {
            Instantiate(act4[sixth], new Vector3(21.23f, -13.815f, 0), Quaternion.FromToRotation(down, up)); // act 4-2
        }
        else
        {
            Instantiate(act4[sixth], new Vector3(21.23f, -13.815f, 0), Quaternion.identity); // act 4-2
        }

        Instantiate(act1[0], new Vector3(-36.21f, 0, 0), Quaternion.identity); // start module
        Instantiate(act5[0], new Vector3(40, 0, 0), Quaternion.identity); // end module

        //used in case a scene is reloaded and reference exception occurs for GridManager
        // GridManager.Instance.reInit();
    }

    #endregion
}
