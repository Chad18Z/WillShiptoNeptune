using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    public float initialScaleRate = 0.03f;
    public float finalScaleRate = 0.01f;
    public float threshHold = 1;
    public float fadeRate = 0.01f;

    private float startScale = 0.05f;
    private SpriteRenderer render;

    // Use this for initialization
    void Start ()
    {
        gameObject.transform.localScale = new Vector3(startScale, startScale, 1);
        render = gameObject.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameObject.transform.localScale.x < .8)
        {
            gameObject.transform.localScale += new Vector3(initialScaleRate, initialScaleRate, 0);
        }
        else if (render.color.a > 0)
        {
            render.color -= new Color(0, 0, 0, fadeRate);
            gameObject.transform.localScale += new Vector3(finalScaleRate, finalScaleRate, 0);
        }
	}
}
