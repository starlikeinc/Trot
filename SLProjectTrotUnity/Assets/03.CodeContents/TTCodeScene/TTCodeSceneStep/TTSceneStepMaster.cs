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
		ProtSceneAttacherLoadAddressablePrefab(c_ScriptDataPrefabName, (bool bSuccess) =>
		{
			if (bSuccess)
			{
				ProtSceneAttacherLoadAddressablePrefab(c_SoundDataPrefabName, (bool bSuccess) =>
				{
					if (bSuccess)
					{
						ProtSceneAttacherLoadUIScene(c_UIRootPrefabName, () => {
							PrivSceneStepFinish();
						});
					}
				});
			}
		});
	}

	private void PrivSceneStepFinish()
	{

	}
}
