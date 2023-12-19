using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// 스테이지는 스크립트 데이터 로딩 관련 로직과 게임 스타트 및 엔드 로직을 관리한다.
// 스테이지는 반복 수행(Reset)이 가능해야 하며 한 장면에 복수의 스테이지가 존재할 수 있다. 

abstract public class CStageBase : CMonoBase
{
	public enum EStageStatus
    {
		None,
		Loading,
		Ready,
		Start,
		End,
    }
  
	private uint m_hLoadID = 0;								        public uint p_LoadID { get { return m_hLoadID; } } public bool p_StageLoaded { get { return m_eStageStatus == EStageStatus.Ready ? true : false; } }
    private EStageStatus m_eStageStatus = EStageStatus.None;	    public EStageStatus p_StageStatus { get { return m_eStageStatus; } }						
	//---------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		base.OnUnityAwake();
		if (CManagerStageBase.Instance != null)
		{
			CManagerStageBase.Instance.InterStageRegist(this);
		} 
	}

	protected override void OnUnityDestroy()
	{
		base.OnUnityDestroy();
		if (CManagerStageBase.Instance != null)
		{
			CManagerStageBase.Instance.InterStageUnRegist(this);
		}
	}
    //-------------------------------------------------------------
    internal void InterStageStart()
	{
		if (m_eStageStatus != EStageStatus.Ready) return;

		SetMonoActive(true);
		m_eStageStatus = EStageStatus.Start;
		OnStageStart();
	}

	internal void InterStageEnd()	// 스테이지가 종료 되었을 경우. 상호작용 불가
	{
		m_eStageStatus = EStageStatus.End;
		OnStageEnd();
	}

	internal void InterStageReset()	 // 스테이지가 로드 없이 재 시작되어야 할때 
    {
		m_eStageStatus = EStageStatus.None;
		OnStageReset();
	}

    internal void InterStageExit()	// 스테이지가 언로드 될때. 사용한 리소스등을 반납
    {
        SetMonoActive(false);
        OnStageExit();
    }

	internal void InterStageRegister()
	{
		OnStageRegister();
	}

	internal void InterStageUnRegister()
	{
		OnStageUnRegister();
	}

	//-------------------------------------------------------------------------------------
	internal void InterStageLoad(uint hLoadID, UnityAction<CStageBase> delFinish, params object[] aParams) // 스크립트로드 및 동적 할당 같은 메모리 작업
	{
		m_hLoadID = hLoadID;
		m_eStageStatus = EStageStatus.Loading;
		OnStageLoad(hLoadID, () => { m_eStageStatus = EStageStatus.Ready; delFinish?.Invoke(this); OnStageLoaded(hLoadID); }, aParams);
	}

	internal void InterStageReLoad(uint hLoadID, UnityAction<CStageBase> delFinish, params object[] aParams) // 일부 에셋만 메모리 갱신이 필요할때 Load가 된 상태로 호출해야 한다.
    {
		m_hLoadID = hLoadID;
        m_eStageStatus = EStageStatus.Loading;
        OnStageReLoad(hLoadID, () => {m_eStageStatus = EStageStatus.Ready; delFinish?.Invoke(this); OnStageReLoaded(hLoadID); }, aParams);
    }

    //--------------------------------------------------------------------------------------

    protected virtual void OnStageLoad(uint hLoadID, UnityAction delFinish, params object[] aParams) { }
	protected virtual void OnStageReLoad(uint hLoadID, UnityAction delFinish, params object[] aParams) { }
	protected virtual void OnStageLoaded(uint hLoadID) { }
	protected virtual void OnStageReLoaded(uint hLoadID) { }
	protected virtual void OnStageExit()	{ }
	protected virtual void OnStageReset()	{ }
	protected virtual void OnStageStart()	{ }
	protected virtual void OnStageEnd()		{ }
	protected virtual void OnStageRegister()	{ }
	protected virtual void OnStageUnRegister(){ }
}
