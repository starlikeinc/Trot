using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TTTrackNoteBase : CMonoBase
{
	[SerializeField]
	private float TrackTiming;
	[SerializeField]
	private float ForeShadow;

	[SerializeField]
	private List<TTNoteAnimationBase> NoteAnimation = new List<TTNoteAnimationBase>();


	private bool m_bFocusBeat = false;

	//------------------------------------------------------------------------
	internal void InterTrackNoteInitialize(TTSubTitleTrackBase pTrackOnwer)
	{

	}
}
