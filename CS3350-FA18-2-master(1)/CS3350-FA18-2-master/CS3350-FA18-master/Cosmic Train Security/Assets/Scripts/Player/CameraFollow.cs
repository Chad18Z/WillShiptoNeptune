using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //the offset between the camera and player to reposition the camera every frame
    Vector3 offSet;
    GameObject player;

	public void Initialize()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		offSet = transform.position = player.transform.position;
	}
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        //grabs the offset between the player and camera position
        offSet = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //every frame the cameras position is set to the players position plus the initial offset
        //this ensures the camera is always on top-center of the player
        transform.position = player.transform.position + offSet;
	}
}
