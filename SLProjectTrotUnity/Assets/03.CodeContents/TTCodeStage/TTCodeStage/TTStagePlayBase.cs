using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class TTStagePlayBase : CStageBase
{
	[SerializeField]
	private TTSubTitlePlayer SubTitlePlayer = null;
	
	 
	//-------------------------------------------------------------------
	protected override void OnStageLoad(uint hLoadID, UnityAction delFinish, params object[] aParams)
	{
		base.OnStageLoad(hLoadID, delFinish, aParams);
	}

	protected override void OnStageReLoaded(uint hLoadID)
	{
		base.OnStageReLoaded(hLoadID);
	}

	protected override void OnStageStart(params object[] aParams)
	{
		base.OnStageStart(aParams);


	}
}
