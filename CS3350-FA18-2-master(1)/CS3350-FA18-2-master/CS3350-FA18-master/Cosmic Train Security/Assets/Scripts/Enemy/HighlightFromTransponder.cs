using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightFromTransponder : MonoBehaviour {

    [SerializeField]
    GameObject prefabHighlight;
    GameObject highlightInstance;
    public bool highlighted = false;
	// Use this for initialization
	void Start () {
        highlighted = false;
	}
    /// <summary>
    /// Used to make sure highlight stays over Enemywhen moving when withing ping radius
    /// </summary>
    private void Update()
    {
        if(highlighted)
        {
            highlightInstance.transform.position = transform.position;
        }
    }

    public void highlightEnemy()
    {
        highlightInstance = Instantiate(prefabHighlight, transform.position, transform.rotation);
        highlighted = true;
    }
    public void disableHighlight()
    {
        Destroy(highlightInstance);
        highlighted = false;

    }

    
}
