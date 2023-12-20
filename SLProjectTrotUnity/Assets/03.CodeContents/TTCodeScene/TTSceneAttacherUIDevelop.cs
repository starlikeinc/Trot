using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSceneAttacherUIDevelop : CSceneAttacherBase
{
	[SerializeField]
	private string ManagerPrefabPath = "FrontPrefab";
	[SerializeField]
	private string ManagerPrefabName = "TTPrefabManager";
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
		ProtSceneAttacherLoadResourcePrefab(ManagerPrefabPath, ManagerPrefabName, () =>
		{
			PrivSceneAttacherLoadFinish();
		});
	}

	private void PrivSceneAttacherLoadFinish()
	{
		
		Destroy(gameObject);
	}
}
