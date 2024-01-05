using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TTSubTitlePlayer : CMonoBase
{

	private bool m_bPlayOver = false;				public bool IsPlayOver { get { return m_bPlayOver; } }
	private bool m_bPlayStart = false;

	private float m_fCurrentPlayTime = 0;
	private double m_fPlayTimeLength = 0;

	private UnityAction m_delFinish = null;
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
	public void DoSubTitlePlayerStart(float fBoardTimeStart, double fPlayTimeLength, UnityAction delFinish)
	{
		if (m_bPlayOver)
		{
			//Error!
			return;
		}
		m_delFinish = delFinish;
		PrivSubTitlePlayerReset();
		PrivSubTitlePlayerStart(fBoardTimeStart, fPlayTimeLength);
	}

	//------------------------------------------------------------------
	

	//------------------------------------------------------------------
	private void PrivSubTitlePlayerReset()
	{
		m_fCurrentPlayTime = 0;
		m_fPlayTimeLength = 0;
		
		for (int i = 0; i < m_listSubTitleBoard.Count; i++)
		{
			m_listSubTitleBoard[i].DoSubTitleBoardReset(this);
		}
	}

	private void PrivSubTitlePlayerStart(float fPlayTimeStart, double fPlayTimeLength)
	{
		m_fCurrentPlayTime = fPlayTimeStart;
		m_fPlayTimeLength = fPlayTimeLength;
		m_bPlayStart = true;

		UpdateSubTitleBoard(m_fCurrentPlayTime);
	}

	private void UpdateSubTitlePlayer(float fDelta)
	{
		m_fCurrentPlayTime += fDelta;

		if (m_fCurrentPlayTime >= m_fPlayTimeLength)
		{
			PrivSubTitlePlayOver();
		}
		else
		{
			UpdateSubTitleBoard(m_fCurrentPlayTime);
		}
	}

	private void UpdateSubTitleBoard(float fCurrentTime)
	{
		for (int i = 0; i < m_listSubTitleBoard.Count; i++)
		{
			m_listSubTitleBoard[i].UpdateSubTitleBoard(fCurrentTime);
		}
	}

	

	private void PrivSubTitlePlayOver()
	{
		m_bPlayOver = true;
		m_bPlayStart = false;
		m_delFinish?.Invoke();
	}


}
