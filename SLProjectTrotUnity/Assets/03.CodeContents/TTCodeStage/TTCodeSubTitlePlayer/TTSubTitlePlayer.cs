using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSubTitlePlayer : CMonoBase
{

	private bool m_bPlayOver = false;				public bool IsPlayOver { get { return m_bPlayOver; } }
	private bool m_bPlayStart = false;

	private float m_fCurrentBoardTime = 0;
	private float m_fBoardTimeEnd = 0;
	
	private List<TTSubTitleBoard> m_listSubTitleBoard = new List<TTSubTitleBoard>();
	//------------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		base.OnUnityAwake();
		GetComponentsInChildOneDepth(m_listSubTitleBoard);
	}

	private void FixedUpdate()
	{
		if (m_bPlayStart)
		{
			UpdateSubTitlePlayer(Time.fixedDeltaTime);
		}
	}

	//-------------------------------------------------------------------
	public void DoSubTitlePlayerStart(float fBoardTimeStart, float fBoardTimeEnd)
	{
		if (m_bPlayOver)
		{
			//Error!
			return;
		}
	
		PrivSubTitlePlayerReset();
		PrivSubTitlePlayerStart(fBoardTimeStart, fBoardTimeEnd);
	}

	//------------------------------------------------------------------
	

	//------------------------------------------------------------------
	private void PrivSubTitlePlayerReset()
	{
		m_fCurrentBoardTime = 0;
		m_fBoardTimeEnd = 0;
		
		for (int i = 0; i < m_listSubTitleBoard.Count; i++)
		{
			m_listSubTitleBoard[i].DoSubTitleBoardReset(this);
		}
	}

	private void PrivSubTitlePlayerStart(float fBoardTimeStart, float fBoardTimeEnd)
	{
		m_fCurrentBoardTime = fBoardTimeStart;
		m_fBoardTimeEnd = fBoardTimeEnd;
		m_bPlayStart = true;

		UpdateSubTitleBoard(m_fCurrentBoardTime);
	}

	private void UpdateSubTitlePlayer(float fDelta)
	{
		m_fCurrentBoardTime += fDelta;

		if (m_fCurrentBoardTime >= m_fBoardTimeEnd)
		{
			PrivSubTitlePlayOver();
		}
		else
		{
			UpdateSubTitleBoard(m_fCurrentBoardTime);
		}
	}

	private void UpdateSubTitleBoard(float fCurrentTime)
	{

	}

	

	private void PrivSubTitlePlayOver()
	{
		m_bPlayOver = true;
		m_bPlayStart = false;
	}


}
