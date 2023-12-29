using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSubTitlePlayer : CMonoBase
{

	private float m_fCurrentBoardTime = 0;

	private bool m_bPlayOver = false;				public bool IsPlayOver { get { return m_bPlayOver; } }
	private TTSubTitleBoard m_pCurrentSubTitle = null;

	private List<TTSubTitleBoard> m_listSubTitleBoard = new List<TTSubTitleBoard>();
	//------------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		base.OnUnityAwake();
		GetComponentsInChildOneDepth(m_listSubTitleBoard);
	}

	//-------------------------------------------------------------------
	public void DoSubTitlePlayerStart(float fStartBoardTime)
	{
		if (m_bPlayOver)
		{
			//Error!
			return;
		}
		PrivSubTitlePlayerReset();
		PrivSubTitleBoardNext(fStartBoardTime);
	}


	//------------------------------------------------------------------
	internal void InterSubTitleNextBoard(float fEndBoardTime)
	{
		PrivSubTitleBoardNext(fEndBoardTime);
	}


	//------------------------------------------------------------------
	private void PrivSubTitlePlayerReset()
	{
		m_fCurrentBoardTime = 0;		
		m_pCurrentSubTitle = null;

		for (int i = 0; i < m_listSubTitleBoard.Count; i++)
		{
			m_listSubTitleBoard[i].DoSubTitleBoardReset(this);
		}
	}

	private void PrivSubTitleBoardNext(float fBoardTime)
	{
		//if (m_iCurrentSubTitle >= m_listSubTitleBoard.Count)
		//{
		//	PrivSubTitlePlayOver();
		//}
		//else
		//{
		//	TTSubTitleBoard pSubTitleBoard = m_listSubTitleBoard[m_iCurrentSubTitle];
		//	m_pCurrentSubTitle = pSubTitleBoard;
		//	PrivSubTitleBoardPlay(pSubTitleBoard);
		//}
	}

	private void PrivSubTitlePlayOver()
	{
		m_bPlayOver = true;
	}

	private void PrivSubTitleBoardPlay(TTSubTitleBoard pSubTitleBoard)
	{
		
	}

	private TTSubTitleBoard FindSubTitleBoard(float fBoardTime)
	{
		TTSubTitleBoard pFindBoard = null;


		return pFindBoard;
	}
}
