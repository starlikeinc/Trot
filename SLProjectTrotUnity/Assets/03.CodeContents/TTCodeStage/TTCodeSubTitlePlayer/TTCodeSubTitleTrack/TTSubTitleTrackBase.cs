using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TTSubTitleTrackBase : CMonoBase
{

	private TTSubTitleBoard m_pOwnerBoard = null;
	private float m_fPrevTrackTime = 0;

	private List<TTTrackNoteBase> m_listBeatNote = new List<TTTrackNoteBase>();
	//-------------------------------------------------------------
	public void InterSubTitleTrackUpdate(float fTrackTime)
	{
		float fTrackDelta = fTrackTime - m_fPrevTrackTime;
		m_fPrevTrackTime = fTrackTime;

		OnSubTitleTrackUpdate(fTrackTime, fTrackDelta);
		PrivSubTitleUpdateNote(fTrackTime, fTrackDelta);
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
		GetComponentsInChildOneDepth(m_listBeatNote);

		for (int i = 0; i < m_listBeatNote.Count; i++)
		{
			m_listBeatNote[i].InterTrackNoteInitialize(this);
		}

		OnSubTitleTrackInitialize(pTrackOnwer);
	}
	//--------------------------------------------------------------
	protected List<TTTrackNoteBase>.Enumerator IterTrackNote()
	{
		return m_listBeatNote.GetEnumerator();
	}
	//--------------------------------------------------------------
	private void PrivSubTitleUpdateNote(float fTrackTime, float fTrackDelta)
	{
		for (int i = 0; i < m_listBeatNote.Count; i++)
		{
			m_listBeatNote[i].InterTrackNoteUpdate(fTrackTime, fTrackDelta);
		}
	}


	//---------------------------------------------------------------
	protected virtual void OnSubTitleTrackUpdate(float fTrackTime, float fTrackDelta) { }
	protected virtual void OnSubTitleTrackStart(float fTrackLength) { }
	protected virtual void OnSubTitleTrackEnd() { }
	protected virtual void OnSubTitleTrackInitialize(TTSubTitleBoard pTrackOnwer) { }
}
