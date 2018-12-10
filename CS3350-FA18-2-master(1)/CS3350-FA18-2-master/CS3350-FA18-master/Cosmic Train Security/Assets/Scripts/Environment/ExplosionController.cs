using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private float fadeRate = 0.02f;
    private SpriteRenderer render;

	// Use this for initialization
	void Start ()
    {
        render = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Color newColor = new Color(render.color.r - fadeRate,
                render.color.g - fadeRate,
                render.color.b - fadeRate,
                render.color.a - fadeRate);
        render.color = newColor;
	}

    public void SetFadeRate(float rate)
    {
        fadeRate = rate;
    }


}
