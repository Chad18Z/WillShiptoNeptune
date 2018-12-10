using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingHighlight : MonoBehaviour
{
    #region Fields
    public float transitionRate = 0.0075f;  // rate that the alpha fades in and out
    private bool fadeOut = true;    
    private SpriteRenderer render;
    #endregion

    #region Methods
    // Use this for initialization
    void Start ()
    {
        render = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Color newColor;
        float alpha = render.color.a;

        if (fadeOut)
        {
            newColor = new Color(render.color.r,
                render.color.g,
                render.color.b,
                alpha - transitionRate);

            if (alpha < 0.5f)
            {
                fadeOut = false;
            }
        }
        else
        {
            newColor = new Color(render.color.r,
                render.color.g,
                render.color.b,
                alpha + transitionRate);

            if (alpha == 1)
            {
                fadeOut = true;
            }
        }

        render.color = newColor;
        
	}
    #endregion
}
