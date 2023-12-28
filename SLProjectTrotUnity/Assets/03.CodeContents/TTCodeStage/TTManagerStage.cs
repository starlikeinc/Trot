using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TTManagerStage : CManagerStageBase
{	public static new TTManagerStage Instance { get { return CManagerStageBase.Instance as TTManagerStage; } }
	//---------------------------------------------------------------------
	public void DoStageStart()
	{
		ProtMgrStageStart();
	}

	public void DoStageLoad(uint hStageID, UnityAction delFinish)
	{
		ProtMgrStageLoad(hStageID, delFinish);
	}


}
