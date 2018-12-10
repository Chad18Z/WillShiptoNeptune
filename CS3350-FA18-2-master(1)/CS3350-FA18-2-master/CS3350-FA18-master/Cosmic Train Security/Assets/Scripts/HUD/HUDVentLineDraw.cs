using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDVentLineDraw : MonoBehaviour {
    LineRenderer ventLine;
    GameObject go;
	// Use this for initialization
	void Start () {
        EventManager.AddMouseOverVentListeners(DrawVentLine);
        EventManager.AddMouseOffVentListeners(RemoveLine);
        go = gameObject;
        ventLine = go.AddComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DrawVentLine(Vector2 origen, Vector2 target)
    {
       
        
        
       // lineRenderer.startColor=Color.blue;
        ventLine.startWidth = .1f;
        ventLine.positionCount=2;
        ventLine.SetPosition(0, origen);
            ventLine.SetPosition(1, target);
        
        
    }
    void RemoveLine()
    {
       
        ventLine.positionCount = 0;
       // Destroy(ventLine);
    }
}
