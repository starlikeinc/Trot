using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSubTitleBoard : CMonoBase
{
	[SerializeField]
	private float BoardTimeStart = 0.0f;
	[SerializeField]
	private float BoardTimeEnd = 0.0f;


	private TTSubTitlePlayer m_pOwnerPlayer = null;

	private bool m_bPlayStart = false;
	private bool m_bPlayOver = false;				public bool IsPlayOver { get { return m_bPlayOver; } } 
	//----------------------------------------------------------
	public void DoSubTitleBoardReset(TTSubTitlePlayer pSubTitlePlayer)
	{
		m_pOwnerPlayer = pSubTitlePlayer;
		m_bPlayOver = false;
		m_bPlayStart = false;
	}

	public void DoSubTitleBoardPlay(float fBoardTime)
	{
		m_bPlayStart = true;
	}


	//---------------------------------------------------------

}
