using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles mouse-over and click sounds for menu buttons
/// </summary>
public class ButtonNoises : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    /// <summary>
    /// Called when player mouses over button
    /// Note: Method must be public to function
    /// </summary>
    public void OnPointerEnter(PointerEventData ped)
    {
        Debug.Log("Highlighted");

        // Play highlight sound effect
        AudioManager.Instance.Play(AudioClipName.button_Highlight);
    }

    /// <summary>
    /// Called when player clicks on button
    /// Note: Again, method must be public to function
    /// </summary>
    /// <param name="ped"></param>
    public void OnPointerDown(PointerEventData ped)
    {
        Debug.Log("Clicked");

        // play selected sound effect
        AudioManager.Instance.Play(AudioClipName.button_Select);
    }
}
