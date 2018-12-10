using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyYourself : MonoBehaviour {

	private float timer;
	[SerializeField] private float maxLifetime = 1;
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if (timer >= maxLifetime)
		{
			Destroy(gameObject);
		}
	}
}
