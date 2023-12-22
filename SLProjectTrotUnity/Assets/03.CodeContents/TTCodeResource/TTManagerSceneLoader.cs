using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TTManagerSceneLoader : CManagerSceneLoaderBase
{	public static new TTManagerSceneLoader Instance { get { return CManagerSceneLoaderBase.Instance as TTManagerSceneLoader; } }
	public const string c_MasterSceneName = "TTSceneMaster";
	//---------------------------------------------------------------------------------------------
	public enum ESceneName
	{
		None,		
		TTSceneLobby,
	}

	private ESceneName m_eCurrentScene = ESceneName.None; 
	//---------------------------------------------------------------------------------------------
	public void DoMgrSceneLoaderGoToMaster(UnityAction delFinish)
	{
		ProtSceneLoaderSingle(c_MasterSceneName, delFinish);
	}

	public void DoMgrSceneLoaderGoToLobby(UnityAction delFinish)
	{
		PrivMgrSceneLoaderGoToSubScene(ESceneName.TTSceneLobby, delFinish);

	}

	//---------------------------------------------------------------------------------------------
	private void PrivMgrSceneLoaderGoToSubScene(ESceneName eSceneName, UnityAction delFinish)
	{
		UIManager.Instance.UIShow<UIFrameLoadingScreen>();
		if (m_eCurrentScene != ESceneName.None)
		{
			ProtSceneLoaderAdditiveUnload(m_eCurrentScene.ToString(), (string strLoadedScene) => {
				PrivMgrSceneLoaderLoadSubScene(eSceneName, delFinish);
			});
		}
		else
		{
			PrivMgrSceneLoaderLoadSubScene(eSceneName, delFinish);
		}
	}

	private void PrivMgrSceneLoaderLoadSubScene(ESceneName eSceneName, UnityAction delFinish)
	{
		m_eCurrentScene = eSceneName;
		ProtSceneLoaderAdditive(m_eCurrentScene.ToString(), (string strLoadedScene) => {
			UIManager.Instance.UIHide<UIFrameLoadingScreen>();
			delFinish?.Invoke();
		});
	}


}
