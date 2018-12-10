using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An audio source for elements in the game
/// </summary>
public class GameAudioSource : MonoBehaviour
{
    #region Methods

    /// <summary>
    /// Called before the Start method
    /// </summary>
    private void Awake()
    {
        // keeps only one audio source in the game
        if (!AudioManager.Initialized)
        {
            // initialize audio source and persist audio source across scenes
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            AudioManager.Initialize(audioSource);
           DontDestroyOnLoad(gameObject);
        }
        else
        {
            // destroys self if duplicate object
            Destroy(gameObject);
        }
    }

    #endregion
}
