using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  UI�� ����� ��� ���� �ε� ��ü�� �ε��Ѵ�. ���� ��� ���� Additive  �� �߰� �����Ѵ�. 
//  ���� ����� ���� ��ε� ���� �ʴ´�.

public class TTSceneStepMaster : TTSceneAttacherBase
{

	//---------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		base.OnUnityAwake();
	}

	protected override void OnUnityStart()
	{
		base.OnUnityStart();
		PrivSceneStepLoad();
	}
	//--------------------------------------------------------------
	private void PrivSceneStepLoad()
	{
		ProtSceneAttacherLoadUIScene(c_UIRootPrefabName, () =>
		{
			TTManagerSceneLoader.Instance.DoMgrSceneLoaderGoToLobby(() =>
			{
				StartCoroutine(CoroutineCheckUILoadFinish());
			});
		});
	}

	private void PrivSceneStepFinish()
	{
		UIManager.Instance.UIHide<UIFrameLoadingScreen>();
	}

	private void PrivSceneStepLoadScriptData()
	{
		UIManager.Instance.UIShow<UIFrameLoadingScreen>();
		ProtSceneAttacherLoadAddressablePrefab(c_ScriptDataPrefabName, (bool bSuccess) =>
		{
			if (bSuccess)
			{
				ProtSceneAttacherLoadAddressablePrefab(c_SoundDataPrefabName, (bool bSuccess) =>
				{
					if (bSuccess)
					{
						//PrivSceneStepFinish();
						Invoke("PrivSceneStepFinish", 1f);
					}
				});
			}
		});
	}

	//------------------------------------------------------------------------
	private IEnumerator CoroutineCheckUILoadFinish()
	{
		while(true)
		{
			if (UIManager.Instance.IsInitialize == true)
			{
				PrivSceneStepLoadScriptData();
				break;
			}
			else
			{
				yield return null;
			}
		}

		yield break;
	}
}
