using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TTSubTitleTrackBeatBase : TTSubTitleTrackBase
{

	
	//------------------------------------------------------------------------
	protected override void OnSubTitleTrackStart(float fTrackLength)
	{
		base.OnSubTitleTrackStart(fTrackLength);

	}

	protected override void OnSubTitleTrackUpdate(float fTrackTime, float fTrackDelta)
	{
		

	}

	protected override void OnSubTitleTrackInitialize(TTSubTitleBoard pTrackOnwer)
	{
		base.OnSubTitleTrackInitialize(pTrackOnwer);
		
	}


	//-----------------------------------------------------------------------------

}
