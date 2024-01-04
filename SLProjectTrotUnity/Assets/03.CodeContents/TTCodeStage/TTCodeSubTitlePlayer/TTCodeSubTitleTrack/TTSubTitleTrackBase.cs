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

	public void InterSubTitleTrackStart(float fTrackLength)
	{
		OnSubTitleTrackStart(fTrackLength);
	}

	public void InterSubTitleTrackEnd()
	{
		OnSubTitleTrackEnd();
	}


	//---------------------------------------------------------------
	protected virtual void OnSubTitleTrackUpdate(float fTrackTime) { }
	protected virtual void OnSubTitleTrackStart(float fTrackLength) { }
	protected virtual void OnSubTitleTrackEnd() { }
}
