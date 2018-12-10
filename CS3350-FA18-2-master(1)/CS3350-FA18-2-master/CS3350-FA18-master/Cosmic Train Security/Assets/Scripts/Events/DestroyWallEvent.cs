using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This is the event that is invoked whenever an explosion results in a wall being destroyed
/// </summary>
public class DestroyWallEvent : UnityEvent<Vector3>
{

}
