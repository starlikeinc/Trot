using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TTStagePlayMovieNormal : TTStagePlayMovieBase
{



	//-------------------------------------------------------------------
	protected override void OnStageLoad(uint hLoadID, UnityAction delFinish, params object[] aParams)
	{
		base.OnStageLoad(hLoadID, delFinish, aParams);
	}

	protected override void OnStageReLoaded(uint hLoadID)
	{
		base.OnStageReLoaded(hLoadID);

	}
}
