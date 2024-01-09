using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TTNoteAnimationBase : CMonoBase
{
	[SerializeField]
	private float StartOffset = 0;

	private bool m_b
	private bool m_bUpdateNoteAni = false;
	private TTTrackNoteBase m_pOwnerNote = null;
	//-----------------------------------------------------------

	internal void InterNoteAnimationInitialize(TTTrackNoteBase pOwnerNote)
	{
		m_pOwnerNote = pOwnerNote;
		OnNoteAnimationInitialize(pOwnerNote);
	}

	internal void InterNoteAnimationStart()
	{
		m_bUpdateNoteAni = true;
		OnNoteAnimationStart();
	}

	internal void InterNoteAnimationUpdate(float fNoteHiteTiming, float fTrackTime, float fTrackDelta)
	{
		if (m_bUpdateNoteAni) return;


	}

	//-------------------------------------------------------------
	protected virtual void OnNoteAnimationInitialize(TTTrackNoteBase pOwnerNote) { }
	protected virtual void OnNoteAnimationStart() { }
}
