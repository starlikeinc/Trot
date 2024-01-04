using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSubTitleBoard : CMonoBase
{
	[SerializeField]
	private float BoardTimeStart = 0.0f;		
	[SerializeField]
	private float BoardTimeEnd = 0.0f;

	private bool m_bPlayStart = false;
	private bool m_bPlayOver = false; public bool IsPlayOver { get { return m_bPlayOver; } }
	private float m_fCurrentTrackTime = 0;

	private TTSubTitlePlayer m_pOwnerPlayer = null;
	private List<TTSubTitleTrackBase> m_listSubTitleTrack = new List<TTSubTitleTrackBase>();
	//----------------------------------------------------------
	public void DoSubTitleBoardReset(TTSubTitlePlayer pSubTitlePlayer)
	{
		m_pOwnerPlayer = pSubTitlePlayer;
		m_bPlayOver = false;
		m_bPlayStart = false;
	}

	public void DoSubTitleBoardPlay()
	{
		m_bPlayStart = true;
	}

	//--------------------------------------------------------
	public void UpdateSubTitleBoard(float fCurrentTime)
	{
		if (m_bPlayOver) return;

		if (m_bPlayStart)
		{
			if (CheckSubTitleBoardTimeOutside(fCurrentTime))
			{
				PrivSubTitleBoardTrackEnd();
			}
			else
			{
				UpdateSubTitleBoardTrackTime(fCurrentTime);
			}
		}
		else
		{
			if (CheckSubTitleBoardTimeInside(fCurrentTime))
			{
				PrivSubTitleBoardTrackStart(fCurrentTime);
			}
		}
	}

	//-------------------------------------------------------
	private bool CheckSubTitleBoardTimeInside(float fBoardTime)
	{
		bool bInside = false;
		if (fBoardTime >= BoardTimeStart && fBoardTime < BoardTimeEnd)
		{
			bInside = true;
		}
		return bInside;
	}

	private bool CheckSubTitleBoardTimeOutside(float fBoardTime)
	{
		bool bOutside = false;

		if (fBoardTime >= BoardTimeEnd)
		{
			bOutside = true;
		}

		return bOutside;
	}

	private void UpdateSubTitleBoardTrackTime(float fBoardTime)
	{
		m_fCurrentTrackTime = fBoardTime - BoardTimeStart;
		for (int i = 0; i < m_listSubTitleTrack.Count; i++)
		{
			m_listSubTitleTrack[i].InterSubTitleTrackUpdate(m_fCurrentTrackTime);
		}
	}

	private void PrivSubTitleBoardTrackStart(float fCurrentTime)
	{
		m_bPlayStart = true;
		float fBoardLength = BoardTimeEnd - BoardTimeStart;
		for (int i = 0; i < m_listSubTitleTrack.Count; i++)
		{
			m_listSubTitleTrack[i].InterSubTitleTrackStart(fBoardLength);
		}

		UpdateSubTitleBoardTrackTime(fCurrentTime);
	}

	private void PrivSubTitleBoardTrackEnd()
	{
		m_bPlayOver = true;
		for (int i = 0; i < m_listSubTitleTrack.Count; i++)
		{
			m_listSubTitleTrack[i].InterSubTitleTrackEnd();
		}
	}

	//---------------------------------------------------------

}
