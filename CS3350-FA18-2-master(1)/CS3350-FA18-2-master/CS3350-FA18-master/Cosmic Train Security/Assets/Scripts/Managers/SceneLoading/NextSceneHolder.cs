using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The loading screen uses this simple manager to determine which scene to load next
/// When switching scenes, you should feed ChangeScene what the next scene is.
/// </summary>
/// <typeparam name="NextSceneHolder"></typeparam>
public class NextSceneHolder : Singleton<NextSceneHolder> 
{
	private SceneHolderEnum currentScene;
	public SceneHolderEnum CurrentScene {get {return currentScene;}}

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public void ChangeToNextScene(SceneHolderEnum next)
	{
		currentScene = next;
	}
}
