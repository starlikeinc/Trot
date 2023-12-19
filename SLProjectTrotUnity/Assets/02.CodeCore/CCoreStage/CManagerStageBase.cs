using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// 스테이지는 독립된 게임 플레이 단위이다. AdditiveScene으로 복수의 스테이지가 하나의 씬에 병합 될 수 있다
// 화면을 분할하여 별도의 카메라를 가진다든가 하는 다양한 실행 환경에 대응하기 위한 레이어이다.

abstract public class CManagerStageBase : CManagerTemplateBase<CManagerStageBase>
{
    private bool m_bStageStart = false;     public bool IsStageStart { get { return m_bStageStart; } }
    private uint m_hStageID = 0;            public uint p_StageID { get { return m_hStageID; } }
	private List<CStageBase> m_listStageInstance = new List<CStageBase>();
    //--------------------------------------------------------------
    protected override void OnUnityAwake()
    {
        base.OnUnityAwake();
        PrivStageFindStage();
    }

    //---------------------------------------------------------------
    internal void InterStageRegist(CStageBase pStage)
	{
		PrivStageRegist(pStage);
	}

	internal void InterStageUnRegist(CStageBase pStage)
    {
		PrivStageUnRegist(pStage);
	}
    //-----------------------------------------------------------------

    protected void ProtMgrStageStart()  // 스테이지 시작 기능 진입점
    {
        if (m_bStageStart) return;
		PrivStageStart();
    } 

    protected void ProtMgrStageEnd()  // 스테이지 플레이 종료. 메모리가 유지되므로  스테이지가 재 시작 될 수 있다.
    {
        m_bStageStart = false;
        PrivStageEnd();
    }

    protected void ProtMgrStageReset() // 스테이지의 모든 내용이 초기화 
    {
		PrivStageReset();
	}

    protected void ProtMgrStageExit() // 스테이지가 메모리에서 언로드
    {
        m_bStageStart = false;
        PrivStageExit();
    }

    protected void ProtMgrStageLoad(uint hLoadID, UnityAction delFinish, params object[] aParams)
    {
        if (m_listStageInstance.Count == 0)
        {
            delFinish?.Invoke();
            return;
        }

        PrivStageLoad(hLoadID, delFinish, aParams);
    }

    protected void ProtMgrStageReload(uint hLoadID, UnityAction delFinish, params object[] aParams)
    {
        if (m_listStageInstance.Count == 0)
        {
            delFinish?.Invoke();
            return;
        }

        PrivStageReLoad(hLoadID, delFinish, aParams);
    }

    //---------------------------------------------------------------
    private void PrivStageRegist(CStageBase pStage)
	{
        if (m_listStageInstance.Contains(pStage) == false)
        {
            m_listStageInstance.Add(pStage);
            pStage.InterStageRegister();
            OnMgrStageRegister(pStage);
		}
    }

    private void PrivStageUnRegist(CStageBase pStage)
	{
		m_listStageInstance.Remove(pStage);
        pStage.InterStageUnRegister();
        OnMgrStageUnRegister(pStage);
	}

	private void PrivStageReset()
    {
		for (int i = 0; i < m_listStageInstance.Count; i++)
        {
			m_listStageInstance[i].InterStageReset();
			OnMgrStageStart(m_listStageInstance[i]);
		}
    }

	private void PrivStageStart()
    {
        for (int i = 0; i < m_listStageInstance.Count; i++)
        {
            if (m_listStageInstance[i].p_StageLoaded == false)
            {
                Debug.LogError(string.Format("[Stage] Stage Does not Loaded : {0}", m_listStageInstance[i].p_LoadID));
            }
            else
            {
                CStageBase pStage = m_listStageInstance[i];
                pStage.InterStageStart();
				OnMgrStageStart(pStage);
			}
        }       
    }

    private void PrivStageExit()
    {
        for (int i = 0; i < m_listStageInstance.Count; i++)
        {
            m_listStageInstance[i].InterStageExit();
			OnMgrStageExit(m_listStageInstance[i]);
		}
	}

    private void PrivStageEnd()
    {
        for (int i = 0; i < m_listStageInstance.Count; i++)
        {
            m_listStageInstance[i].InterStageEnd();
			OnMgrStageEnd(m_listStageInstance[i]);
		}        
    }

    private void PrivStageLoad(uint hLoadID, UnityAction delFinish, params object[] aParams)
    {
        int iLoadCount = 0;
        for (int i = 0; i < m_listStageInstance.Count; i++)
        {
            m_listStageInstance[i].InterStageLoad(hLoadID, (CStageBase pLoadStage) =>
            {
                iLoadCount++;
				OnMgrStageLoaded(pLoadStage);
				if (iLoadCount == m_listStageInstance.Count)
                {                    
                    delFinish?.Invoke();
					OnMgrStageLoadFinish();
				}
            }, aParams);
        }
    }

    private void PrivStageReLoad(uint hLoadID, UnityAction delFinish, params object[] aParams)
    {
        int iLoadCount = 0;
        for (int i = 0; i < m_listStageInstance.Count; i++)
        {
            m_listStageInstance[i].InterStageReLoad(hLoadID, (CStageBase pLoadedStage) =>
            {
                iLoadCount++;
				OnMgrStageReLoaded(pLoadedStage);
				if (iLoadCount == m_listStageInstance.Count)
                {                   
                    delFinish?.Invoke();
                    OnMgrStageLoadFinish();
				}
            }, aParams);
        }
    }
    //------------------------------------------------------------------
    private void PrivStageFindStage()
    {
        CStageBase[] aFindStage = FindObjectsByType<CStageBase>(FindObjectsSortMode.None);
        for (int i = 0; i < aFindStage.Length; i++)
        {
			PrivStageRegist(aFindStage[i]);
        }
    }

    //-----------------------------------------------------------------
    protected CStageBase FindMgrStage(int iIndex)
    {
        CStageBase pFindStage = null;
        if (iIndex < m_listStageInstance.Count)
        {
            pFindStage = m_listStageInstance[iIndex];
        }

        return pFindStage;
    }

    protected List<CStageBase>.Enumerator GetMgrStageInstanceIterator()
    {
        return m_listStageInstance.GetEnumerator();
    }

    //-------------------------------------------------------------------
    protected virtual void OnMgrStageStart(CStageBase pStage) { }
    protected virtual void OnMgrStageEnd(CStageBase pStage) { }
    protected virtual void OnMgrStageReset(CStageBase pStage) { }
    protected virtual void OnMgrStageExit(CStageBase pStage) { }
    protected virtual void OnMgrStageLoaded(CStageBase pStage) { }
    protected virtual void OnMgrStageLoadFinish() { }
    protected virtual void OnMgrStageReLoaded(CStageBase pStage) { }
    protected virtual void OnMgrStageRegister(CStageBase pStage) { }
    protected virtual void OnMgrStageUnRegister(CStageBase pStage) { }
}
