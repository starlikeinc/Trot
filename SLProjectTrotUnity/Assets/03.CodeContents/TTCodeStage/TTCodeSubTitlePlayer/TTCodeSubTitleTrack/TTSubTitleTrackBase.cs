using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TTSubTitleTrackBase : CMonoBase
{

	private TTSubTitleBoard m_pOwnerBoard = null;
	private float m_fPrevTrackTime = 0;
	//-------------------------------------------------------------
	public void InterSubTitleTrackUpdate(float fTrackTime)
	{
		float fTrackDelta = fTrackTime - m_fPrevTrackTime;
		m_fPrevTrackTime = fTrackTime;
		OnSubTitleTrackUpdate(fTrackTime, fTrackDelta);
	}

	public void InterSubTitleTrackStart(float fTrackLength)
	{
		m_fPrevTrackTime = 0;
		OnSubTitleTrackStart(fTrackLength);
	}

	public void InterSubTitleTrackEnd()
	{
		OnSubTitleTrackEnd();
	}

	public void InterSubTitleTrackInitialize(TTSubTitleBoard pTrackOnwer)
	{
		m_pOwnerBoard = pTrackOnwer;
		OnSubTitleTrackInitialize(pTrackOnwer);
	}
	

	//---------------------------------------------------------------
	protected virtual void OnSubTitleTrackUpdate(float fTrackTime, float fTrackDelta) { }
	protected virtual void OnSubTitleTrackStart(float fTrackLength) { }
	protected virtual void OnSubTitleTrackEnd() { }
	protected virtual void OnSubTitleTrackInitialize(TTSubTitleBoard pTrackOnwer) { }
}
