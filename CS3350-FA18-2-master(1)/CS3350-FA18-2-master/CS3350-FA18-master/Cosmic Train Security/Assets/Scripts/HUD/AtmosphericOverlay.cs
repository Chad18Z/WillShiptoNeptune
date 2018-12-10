using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script controlling the pulsation of the darkness overlay
/// </summary>
public class AtmosphericOverlay : MonoBehaviour
{
    // pulsation rate support variables
    CanvasGroup overlayGroup;   // reference to canvas group which overlay resides in
    int pulseDirection = -1;    // controls direction to increment / decrement alpha
    float alphaChangePerFrame;  // calculated rate by which alpha changes per second
    float tempAlpha;            // stores temporary alpha when lights flicker
    int flickerFrameCounter;    // counter to aid in flickering lights for a duration

    // declare max/min alpha and pulsation rate of overlay
    public float maxAlpha = 1f;
    public float minAlpha = 0f;

    // defines speed from min alpha to max alpha
    public float pulseRate = 1f;

    // define public flicker control variables
    public int flickerRate = 300;       // 1 in X chance of flickering lights
    public int flickerDuration = 3;     // number of frames which flicker lasts
    public float flickerAlpha = 1f;     // alpha value of flickering effect

	// Use this for initialization
	void Start ()
    {
        Debug.Log("something");
        // retrieve canvas group component of overlay and set its alpha to max
        overlayGroup = GetComponentInChildren<CanvasGroup>();
        overlayGroup.alpha = maxAlpha;

        // constrain alpha of flicker
        if (flickerAlpha > 1)
            flickerAlpha = .99f;

        // constrain min and max alpha of pulse
        if (maxAlpha > flickerAlpha)
            maxAlpha = flickerAlpha - .05f;
        if (minAlpha < 0 || minAlpha > maxAlpha)
            minAlpha = 0;

        // calculate change in alpha per frame
        alphaChangePerFrame = pulseRate * (maxAlpha - minAlpha);
	}

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        // if current alpha is that of flicker effect (i.e., lights flicker on last frame)
        if (overlayGroup.alpha == flickerAlpha)
        {
            // if flicker frame counter exceeds max
            if (flickerFrameCounter > flickerDuration)
            {
                // set alpha to old temp and reset counter
                overlayGroup.alpha = tempAlpha;
                flickerFrameCounter = 0;
            }
            // otherwise, increment frame counter
            else
                flickerFrameCounter++;
        }
        // otherwise, handle potential flickering and adjust pulsation
        else
        {
            // roll random chance to flicker lights for a frame
            int flickerRoll = Random.Range(1, flickerRate);

            // if roll succeeds
            if (flickerRoll == 1)
            {
                // store temp alpha and set overlay's alpha to 1
                tempAlpha = overlayGroup.alpha;
                overlayGroup.alpha = flickerAlpha;
            }
            // if roll fails
            else
            {
                // increment / decrement alpha of screen overlay by pulse rate
                overlayGroup.alpha += pulseDirection * Time.deltaTime * alphaChangePerFrame;

                // if alpha of overlay reaches either max or min bounds, reverse its direction
                if (overlayGroup.alpha <= minAlpha || overlayGroup.alpha >= maxAlpha)
                    pulseDirection *= -1;
            }
        }
    }

}
