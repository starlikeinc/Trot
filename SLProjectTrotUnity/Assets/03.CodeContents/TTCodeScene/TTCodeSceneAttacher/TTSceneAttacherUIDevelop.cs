using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSceneAttacherUIDevelop : TTSceneAttacherBase
{
	
	//---------------------------------------------------------
	protected override void OnUnityAwake()
	{
		base.OnUnityAwake();
	}

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
			ProtSceneAttacherLoadAddressablePrefab(c_ScriptDataPrefabName, (bool bSuccess) => { 
				if (bSuccess)
				{
					ProtSceneAttacherLoadAddressablePrefab(c_SoundDataPrefabName, (bool bSuccess) =>
					{
						if (bSuccess)
						{
							PrivSceneAttacherLoadFinish();
						}
					});
				}
			});
		});
	}

	private void PrivSceneAttacherLoadFinish()
	{
		
		Destroy(gameObject);
	}
}
