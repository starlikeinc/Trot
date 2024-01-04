using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TTSubTitleTrackBase : CMonoBase
{



	//-------------------------------------------------------------
	public void InterSubTitleTrackUpdate(float fTrackTime)
	{ 
		OnSubTitleTrackUpdate(fTrackTime);
	}

	public void InterSubTitleTrackStart()
	{

	}


	//---------------------------------------------------------------
	protected virtual void OnSubTitleTrackUpdate(float fTrackTime) { }
	protected virtual void OnSubTitleTrackStart() { }
	protected virtual void OnSubTitleTrackEnd() { }
}
