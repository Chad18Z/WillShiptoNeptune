using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    bool gridSpawned;

    private void Awake()
    {
        //GridManager.Instance.Init();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (GridManager.Instance != null && !gridSpawned)
        {
            GridManager.Instance.Init();
            gridSpawned = true;
        }
	}
}
