using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using NWebPacket;

public abstract class CSessionWebRequestBase : CSessionBase
{
	public enum EWebRequestSyncType
	{
		Blocking,           // 순서가 중요한 작업. 모든 작업은 큐잉을 하며 순서대로 처리한다. Send는 다음 프레임에 처리된다.
		NoneBlocking,       // 즉시 발송된다. 순서가 중요하지 않는 작업
	}

	public enum EWebRequestMethodType
	{
		GET,
		POST,
	}
     
	private class SWebPacketWork : CObjectInstancePoolBase<SWebPacketWork>
	{
		public string strURL;
		public UnityWebRequest pWebRequest = null;
		public EWebRequestSyncType eRequestSyncType = EWebRequestSyncType.NoneBlocking;
		public EWebRequestMethodType eRequestMethodType = EWebRequestMethodType.GET;
		public IJsonPacketRequest pPacketRequest = null;
		public UnityAction<string> delResponse = null;
        public bool bAutoReleaseResponse = true;
 
		protected override void OnObjectPoolActivate()
		{
			pWebRequest = null;
			pPacketRequest = null;
            eRequestSyncType = EWebRequestSyncType.NoneBlocking;
            eRequestMethodType = EWebRequestMethodType.GET;
            strURL = null;
			delResponse = null;           
        }
	}

	protected struct SRequestHeader
	{
		public string strHeaderName;
		public string strHeaderValue;
	}

	//------------------------------------------------------------------------
	[SerializeField]
	private int TimeOutSecond = 5;

	private SWebPacketWork m_pRequestCurrent = null;
	private Queue<SWebPacketWork> m_queRequestContents = new Queue<SWebPacketWork>();
	private List<SRequestHeader> m_listRequestHeader = new List<SRequestHeader>();
	//-------------------------------------------------------------
	protected override void OnSessionUpdate(float fDeltaTime)
	{
		UpdateWebRequestQueue();
	}

    protected override void OnSessionFocusRemove()
    {		
		if (m_pRequestCurrent != null)
		{
			m_pRequestCurrent.pWebRequest.Abort();
			m_pRequestCurrent.pWebRequest.Dispose();
		}

		m_pRequestCurrent = null;
		m_queRequestContents.Clear();
		m_listRequestHeader.Clear();
	}

    //--------------------------------------------------------------
    protected void ProtWebRequestIniailizeHeader(List<SRequestHeader> listRequestHeader)
	{		
		m_listRequestHeader = listRequestHeader;
	}

	protected void ProtWebRequestSendPacket(string strURL, IJsonPacketRequest pPacket, EWebRequestSyncType eWebRequestSendType, UnityAction<string> delResponse)
	{
		SWebPacketWork pContents = SWebPacketWork.InstancePoolEnable<SWebPacketWork>();
		pContents.delResponse = delResponse;       
		pContents.eRequestSyncType = eWebRequestSendType;
		pContents.eRequestMethodType = EWebRequestMethodType.POST;
		pContents.pPacketRequest = pPacket;
		pContents.strURL = strURL;

        m_queRequestContents.Enqueue(pContents);
    }

    protected void ProtWebRequestSendGet(string strURL, EWebRequestSyncType eWebRequestSyncType, UnityAction<string> delResponse)
    {
        SWebPacketWork pContents = SWebPacketWork.InstancePoolEnable<SWebPacketWork>();
        pContents.delResponse = delResponse;
        pContents.eRequestSyncType = eWebRequestSyncType;
        pContents.eRequestMethodType = EWebRequestMethodType.GET;
        pContents.pPacketRequest = null;
        pContents.strURL = strURL;

        m_queRequestContents.Enqueue(pContents);
    }

    //------------------------------------------------------------
    private void UpdateWebRequestQueue()
	{
		if (m_pRequestCurrent == null)
		{
			if (m_queRequestContents.Count > 0)
			{
				m_pRequestCurrent = m_queRequestContents.Dequeue();
				PrivWebRequestPacketSend(m_pRequestCurrent); 
			}
		}
	}

    private void PrivWebRequestPacketSend(SWebPacketWork pRequestContents)
    {
        string strContents = null;
        // 사용한 패킷은 리턴
        if (pRequestContents.pPacketRequest != null)
        {
            strContents = JsonUtility.ToJson(pRequestContents.pPacketRequest); // ToDo 암호화
            pRequestContents.pPacketRequest.ReturnInstance();
            pRequestContents.pPacketRequest = null;
        }

        pRequestContents.pWebRequest = PrivWebRequestPacketUploadSetting(pRequestContents.strURL, pRequestContents.eRequestMethodType, strContents);

		if (pRequestContents.eRequestSyncType == EWebRequestSyncType.Blocking)
		{
			OnMgrWebRequestBlocking(true);
		}

		ProtSessionCoroutineStart(CoroutineRequestPacketSend(pRequestContents));
	}

	private void PrivWebRequestPacketRemove(SWebPacketWork pRequestContents)
	{
		if (m_pRequestCurrent == pRequestContents)
		{
			m_pRequestCurrent = null;
		}
		SWebPacketWork.InstancePoolDisable(pRequestContents);
	}

	private void PrivWebRequestPacketResponse(SWebPacketWork pRequestContents)
	{
		if (pRequestContents.eRequestSyncType == EWebRequestSyncType.Blocking)
		{
			OnMgrWebRequestBlocking(false);
		}

		if (pRequestContents.pWebRequest.result != UnityWebRequest.Result.Success)
		{          
			ProtSessionError((int)pRequestContents.pWebRequest.responseCode, pRequestContents.pWebRequest.error, "");
            pRequestContents.delResponse?.Invoke(string.Empty);
		}
        else
        {
            pRequestContents.delResponse?.Invoke(pRequestContents.pWebRequest.downloadHandler.text);
        }

        PrivWebRequestPacketRemove(pRequestContents);
	}

	private UnityWebRequest PrivWebRequestPacketUploadSetting(string strURL, EWebRequestMethodType eMethodType, string strContents)
	{
		UnityWebRequest pRequest = null;

		if (eMethodType == EWebRequestMethodType.POST)
		{
			pRequest = UnityWebRequest.Post(strURL, strContents, "application/json");
		}
		else if (eMethodType == EWebRequestMethodType.GET)
		{
			pRequest = UnityWebRequest.Get(strURL);
		}

		for (int i = 0; i < m_listRequestHeader.Count; i++)
		{
			pRequest.SetRequestHeader(m_listRequestHeader[i].strHeaderName, m_listRequestHeader[i].strHeaderValue);
		}
		pRequest.timeout = TimeOutSecond;
		return pRequest;
	}

	//--------------------------------------------------------------------------
	IEnumerator CoroutineRequestPacketSend(SWebPacketWork pRequestContents)
	{
		yield return pRequestContents.pWebRequest.SendWebRequest();
		PrivWebRequestPacketResponse(pRequestContents);
		yield break;
	}

	//---------------------------------------------------------------------------
	protected virtual void OnMgrWebRequestBlocking(bool bGameBlockStart) { }
	protected virtual void OnMgrWebResponse(UnityWebRequest pWebResponse) { }  // 각종 로그 출력용 
	protected virtual void OnMgrWebRequestError(long iReponseCode, string strErrorDescription) { }


}
