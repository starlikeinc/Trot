using UnityEngine;
using System.Collections.Generic;
using System.Collections;

abstract public class CFiniteStateMachineBase : CAssistUnitBase
{
	public enum EStateEnterType
	{
		Enter,          //새로운 스테이트는 큐에 대기
		Interrupt,      //새로운 스테이트는 스텍에 보존되며 새로운 스테이트가 활성화된다.
		EnterForce,     //새로운 스테이트는 즉시 활성화되며 모든 큐와 스텍이 강제 종료된다.
	}

	private Stack<CStateBase> m_stackInterruptedState = new Stack<CStateBase>();
	private Queue<CStateBase> m_queueState = new Queue<CStateBase>();

	private CStateBase m_pStateCurrent = null;  protected CStateBase GetFSMCurrentState() { return m_pStateCurrent; }
	private CStateBase m_pStatePrev = null;
	
	//-------------------------------------------------------------
	private void LateUpdate()
	{   //일반 업데이트가 끝난 이후 스테이트 교체가 발생
		if (m_pStateCurrent == null)
		{
			PrivLateUpdateAction();
		}
	}

	internal void InterStateLeave(CStateBase pStateEnd)
	{
		ProtStateLeave(pStateEnd);
	}

    internal void InterStateRemove(CStateBase pStateRemove)  // 외부에서 강제 종료 되었을때
    {
        ProtStateRemove(pStateRemove);
    }

	//----------------------------------------------------------
	protected virtual void ProtStateAction(CStateBase pState, EStateEnterType eStateAction)
	{
		pState.InterStateInitialize(this);
        
		switch (eStateAction)
		{
			case EStateEnterType.Enter:
				PrivStateActionEnter(pState);
				break;
			case EStateEnterType.EnterForce:
				PrivStateActionEnterForce(pState);
				break;
			case EStateEnterType.Interrupt:
				PrivStateActionInterrupt(pState);
				break;
		}		
	}

	protected virtual void ProtStateLeave(CStateBase pState)
	{
		if (m_pStateCurrent != pState) return;

		pState.InterStateLeave(m_pStateCurrent);
		m_pStateCurrent = null;

		OnFSMStateLeave(pState);
	}

    protected virtual void ProtStateRemove(CStateBase pState)
    {
        if (m_pStateCurrent != pState) return;
        pState.InterStateRemove(m_pStateCurrent);
        m_pStateCurrent = null;
        OnFSMStateRemove(pState);
    }

	protected void ProtStatClearAll()
	{		
		m_stackInterruptedState.Clear();
		m_queueState.Clear();

		if (m_pStateCurrent != null)
		{
			m_pStateCurrent.InterStateLeave(m_pStatePrev);
			m_pStateCurrent.InterStateRemove(m_pStatePrev);
		}

		m_pStateCurrent = null;
		m_pStatePrev = null;
	}

    protected bool IsEmpty()
    {
        bool bEmpty = false;
        if (m_queueState.Count == 0 && m_stackInterruptedState.Count == 0 && m_pStateCurrent == null)
        {
            bEmpty = true;
        }

        return bEmpty;
    }

    //------------------------------------------------------------
    private void PrivStateActionEnter(CStateBase pState)
	{
		if (m_pStateCurrent != null)
		{
			m_pStateCurrent.InterStateEnterAnother(pState);
		}

		PrivStateActivate(pState);
	}

	private void PrivStateActionEnterForce(CStateBase pState)
	{
		PrivStateClearAll(pState);
		PrivStateActivate(pState);
	}

	private void PrivStateActionInterrupt(CStateBase pState)
	{
		if (m_pStateCurrent != null)
		{
			m_stackInterruptedState.Push(m_pStateCurrent);
			m_pStateCurrent.InterStateInterrupted(pState);
		}

		PrivStateCurrent(pState);
	}

	private void PrivStateClearAll(CStateBase pStateNew)
	{
		Stack<CStateBase>.Enumerator itStack = m_stackInterruptedState.GetEnumerator();
		while (itStack.MoveNext())
		{
			itStack.Current.InterStateRemove(pStateNew);
		}
		m_stackInterruptedState.Clear();

		Queue<CStateBase>.Enumerator itQueue = m_queueState.GetEnumerator();
		while (itQueue.MoveNext())
		{
			itQueue.Current.InterStateRemove(pStateNew);
		}
		m_queueState.Clear();

		if (m_pStateCurrent != null)
		{
			m_pStateCurrent.InterStateLeave(pStateNew);
			m_pStateCurrent.InterStateRemove(pStateNew);
		}
	}

	private void PrivStateActivate(CStateBase pState)
	{		
        if (m_queueState.Count == 0 && m_stackInterruptedState.Count == 0 && m_pStateCurrent == null)
        {
            PrivStateCurrent(pState);
        }
        else
        {
            m_queueState.Enqueue(pState);
        }
    }

	//-------------------------------------------------------------
	private void PrivLateUpdateAction()
	{
		if (PrivStateUpdateInterrupt() == false)
		{
			PrivStateUpdateQueue();
		}
	}

	private bool PrivStateUpdateInterrupt()
	{
		bool bUpdate = false;
		if (m_stackInterruptedState.Count > 0)
		{
			bUpdate = true;
			CStateBase pState = m_stackInterruptedState.Pop();
			pState.InterStateInterruptedResume(m_pStatePrev);
			PrivStateCurrent(pState);
		}
		return bUpdate;
	}

	private void PrivStateUpdateQueue()
	{
		if (m_queueState.Count > 0)
		{
			CStateBase pState = m_queueState.Dequeue();			
			PrivStateCurrent(pState);
		}
		else
		{			
			OnFSMStateEmpty();
		}
	}

	private void PrivStateCurrent(CStateBase pState)
	{
        m_pStatePrev = m_pStateCurrent;
        m_pStateCurrent = pState;
        StopAllCoroutines();
        pState.InterStateEnter(m_pStatePrev);

        if (m_pStateCurrent != null)
        {
            StartCoroutine(m_pStateCurrent.OnStateUpdate());
        }

        OnFSMStateEnter(pState);
	}

	//---------------------------------------------------------------------
	protected virtual void OnFSMStateEnter(CStateBase pState) { }
	protected virtual void OnFSMStateLeave(CStateBase pState) { }
    protected virtual void OnFSMStateRemove(CStateBase pState) { }
	protected virtual void OnFSMStateEmpty() { }
}
