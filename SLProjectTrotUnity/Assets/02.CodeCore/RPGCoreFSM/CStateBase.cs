using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

public abstract class CStateBase
{
    private CFiniteStateMachineBase m_pFSMOwnr = null;   protected CFiniteStateMachineBase GetStateFSMOwner() { return m_pFSMOwnr; }
	//------------------------------------------------------------------------
    internal void InterStateInitialize(CFiniteStateMachineBase pFSMOwner)
	{
        m_pFSMOwnr = pFSMOwner;
        OnStateInitialize(pFSMOwner);
    }
    internal void InterStateEnterAnother(CStateBase pStatePrev)
	{
        OnStateEnterAnother(pStatePrev);
    }
    internal void InterStateEnter(CStateBase pStatePrev)
	{
        OnStateEnter(pStatePrev);
	}
   
    internal void InterStateLeave(CStateBase pStatePrev)
	{
        OnStateLeave(pStatePrev);
	}
    internal void InterStateRemove(CStateBase pStatePrev)
	{
        OnStateRemove(pStatePrev);
	}
    internal void InterStateInterrupted(CStateBase pStateInterrupt)
	{
        OnStateInterrupted(pStateInterrupt);
    }
    internal void InterStateInterruptedResume(CStateBase pStateInterrupt)
	{
        OnStateInterruptResume(pStateInterrupt);
    }

    //--------------------------------------------------------------------------
    protected void ProtStateSelfEnd()
	{
        m_pFSMOwnr?.InterStateLeave(this);
    } 
    protected void ProtStateSelfRemove()
    {
        m_pFSMOwnr?.InterStateRemove(this);
    }


	//--------------------------------------------------------------------------
	#region StateEventHandle
    protected virtual void OnStateInitialize(CFiniteStateMachineBase pFSMOwner) {}
    protected virtual void OnStateEnterAnother(CStateBase pStatePrev) {}
	protected virtual void OnStateEnter(CStateBase pStatePrev) {}
    protected virtual void OnStateRemove(CStateBase pStatePrev) { }
    protected virtual void OnStateLeave(CStateBase pStatePrev) {}	
    protected virtual void OnStateInterrupted(CStateBase pStateInterrupt) {}
	protected virtual void OnStateInterruptResume(CStateBase pStateInterrupt) { }
    public    virtual IEnumerator OnStateUpdate() { yield break; }
	#endregion
	//---------------------------------------------------------------------------
}
