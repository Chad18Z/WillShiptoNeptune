using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWaveEffect : MonoBehaviour
{
    public float delay = 0.5f;
    public int limit = 3;
    public GameObject soundWave;
    private float time;
    private List<GameObject> soundWaves;
    //private Transform transform;

    // Use this for initialization
    void Start ()
    {
        soundWaves = new List<GameObject>();
        time = 0.5f;    // allows initial sound wave to spawn immediantly
        //transform = gameObject.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;

		if (soundWaves.Count < limit && time >= delay)
        {
            time = 0;
            soundWaves.Add(Instantiate(soundWave, transform.position, transform.rotation));
        }
	}
}
