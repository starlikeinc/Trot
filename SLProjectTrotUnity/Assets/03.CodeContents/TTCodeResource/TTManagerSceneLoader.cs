using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTManagerSceneLoader : CManagerSceneLoaderBase
{	public static new TTManagerSceneLoader Instance { get { return CManagerSceneLoaderBase.Instance as TTManagerSceneLoader; } }
	//---------------------------------------------------------------------------------------------
	public enum ESceneName
	{
		SceneLogin,
		SceneLobby,
	}


	//---------------------------------------------------------------------------------------------
	public void DoMgrSceneLoaderLoad(ESceneName eSceneName)
	{

	}
}
