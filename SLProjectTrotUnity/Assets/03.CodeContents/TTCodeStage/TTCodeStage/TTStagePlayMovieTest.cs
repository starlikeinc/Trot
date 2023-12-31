using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TTStagePlayMovieTest : TTStagePlayMovieNormal
{
	[SerializeField]
	private uint StageID = 0;
	[SerializeField]
	private float BoardTime = 0;

	//-------------------------------------------------------------------
	protected override void OnStageLoad(uint hLoadID, UnityAction delFinish, params object[] aParams)
	{
		if (StageID == 0)
		{
			base.OnStageLoad(hLoadID, delFinish, aParams);
		}
		else
		{
			base.OnStageLoad(StageID, delFinish, aParams);
		}
	}

	protected override void OnStageStart(params object[] aParams)
	{
	//	BoardTime = 10f;
		base.OnStageStart(BoardTime);
	}

}
