using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// [주의!] 네트워크 메니저는 별도의 코루틴을 가지므로 독립된 게임 오브젝트에 설치해야 한다.
//         다른 메니저들과 같이 있을 경우 코루틴 간섭이 있을 수 있다. 

public abstract class CManagerGameNetworkBase : CManagerTemplateBase<CManagerGameNetworkBase>
{   
    private bool m_bNetworkFocus = true;                   public bool p_IsNetworkFocus  { get { return m_bNetworkFocus; } }
    private List<CSessionBase> m_listSession = new List<CSessionBase>();   
    //--------------------------------------------------------------------------------------
    private void OnApplicationQuit()
    {
        m_bNetworkFocus = false;
        PrivMgrGameNetworkSessionRemove();
        OnMgrNetworkSessionRemove();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) 
        {
            m_bNetworkFocus = false;
            PrivMgrGameNetworkSessionFocusOnOff(false);
            OnMgrNetworkSessionFocusOff();
        }
        else
        {
            m_bNetworkFocus = true;
            PrivMgrGameNetworkSessionFocusOnOff(true);
            OnMgrNetworkSessionFocusOn();
        }
    }
    
    //--------------------------------------------------------------------------------------
    protected sealed override void OnUnityUpdate()
	{
	    if (m_bNetworkFocus)
        {
            PrivMgrGameNetworkSessionUpdate(Time.deltaTime);
        }

        OnMgrNetworkSessionUpdate();
    }

    //-------------------------------------------------------------------------------------
    protected void ProtMgrGameNetworkSessionAdd(CSessionBase pSession)
    {
        if (m_listSession.Contains(pSession)) return;

        pSession.InterSessionInitialize(this, HandleMgrGameNetworkErrorMessage);
        m_listSession.Add(pSession);
    }

    protected void ProtMgrGameNetworkReset()
    {
        StopAllCoroutines();
        PrivMgrGameNetworkSessionReset();
        OnMgrNetworkSessionReset();
    }

    protected void ProtMgrGameNetworkRemoveAll()
    {
        PrivMgrGameNetworkSessionRemove();
        StopAllCoroutines();
        m_listSession.Clear();
    }

    //-----------------------------------------------------------------------------
    private void PrivMgrGameNetworkSessionRemove()
    {
        for (int i = 0; i < m_listSession.Count; i++)
        {
            m_listSession[i].InterSessionFocusRemove();
        }
    }

    private void PrivMgrGameNetworkSessionFocusOnOff(bool bOn)
    {
        for (int i = 0; i < m_listSession.Count; i++)
        {
            if (bOn)
            {
                m_listSession[i].InterSessionFocusOn();
            }
            else
            {
                m_listSession[i].InterSessionFocusOff();
            }
        }
    }

    private void PrivMgrGameNetworkSessionUpdate(float fDelta)
    {
        for (int i = 0; i < m_listSession.Count; i++)
        {
            m_listSession[i].InterSessionUpdate(fDelta);
        }
    }

    private void PrivMgrGameNetworkSessionReset()
    {
        for (int i = 0; i < m_listSession.Count; i++)
        {
            m_listSession[i].InterSessionReset();
        }
    }

    //-----------------------------------------------------------------------------
    private void HandleMgrGameNetworkErrorMessage(int iResultID, string strMessage, string strData)
	{
		Debug.Log(string.Format("[Network Session] ResultID : {0} / Message {1} / Result {2}", iResultID, strMessage, strData));
		OnMgrNetworkSessionErrorMessage(iResultID, strMessage, strData);
	}

    //--------------------------------------------------------------------------------
	protected virtual void OnMgrNetworkSessionUpdate() { }
    protected virtual void OnMgrNetworkSessionFocusOn() { }
    protected virtual void OnMgrNetworkSessionFocusOff() { }
	protected virtual void OnMgrNetworkSessionRemove() { }
    protected virtual void OnMgrNetworkSessionErrorMessage(int iResultID, string strMessage, string strResult) { }
    protected virtual void OnMgrNetworkSessionReset() { }
}
