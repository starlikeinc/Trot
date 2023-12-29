using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSceneAttacherStageNormal : TTSceneAttacherBase
{
	protected override void OnUnityStart()
	{
		base.OnUnityStart();
		PrivSceneAttacherLoad();
	}

	//------------------------------------------------------
	private void PrivSceneAttacherLoad()
	{
		ProtSceneAttacherLoadResourcePrefab(c_ManagerPrefabPath, c_ManagerPrefabName, () =>
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
								PrivSceneAttacherLoadFinish();
							});
						}
					});
				}
			});
		});
	}

	private void PrivSceneAttacherLoadFinish()
	{
		TTManagerStage.Instance.DoStageLoad(0, () => {
		//	TTManagerStage.Instance.DoStageStart();
			Destroy(gameObject);
		});
	}
}
