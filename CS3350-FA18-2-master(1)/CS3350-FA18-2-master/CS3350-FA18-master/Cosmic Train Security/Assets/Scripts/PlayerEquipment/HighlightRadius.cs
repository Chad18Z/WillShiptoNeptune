using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightRadius : MonoBehaviour {
    #region Fields
    //Serializefield setup for sprites
    [SerializeField]
    Sprite highlightedSprite;

    [SerializeField]
    Sprite regularSprite;

    #endregion


    #region Methods
    /// <summary>
    /// Tripwire range detection
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = highlightedSprite;

            if (this.gameObject.tag == "Locker")
            {
                AudioManager.Instance.Overlap(AudioClipName.locker_Open);
            }
            else
            {
                AudioManager.Instance.Play(AudioClipName.item_Near);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D trigLeave)
    {
        if (trigLeave.gameObject.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = regularSprite;
        }
    }
    #endregion
}


