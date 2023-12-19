using UnityEngine.Events;
using UnityEngine;
public abstract class CLoginBase
{
	private bool m_bLoginStart = false;
	private bool m_bLogin = false;              public bool IsOAuthLogin() { return m_bLogin; }
	private UnityAction<bool> m_delFinish = null;    // 성패, 메시지, 성공시 출력 토큰
	//---------------------------------------------------------------------
	internal void InterOAuthLogInInitialize()
	{
		OnOAuthInitialize();
	}

	//----------------------------------------------------------------------
	internal void InterOAuthLogIn(bool bAutoLogin, UnityAction<bool> delFinish, params object [] aParams)
	{
        m_delFinish = delFinish;
		if (m_bLoginStart || m_bLogin)
		{
			return;
		}
		m_bLoginStart = true;
		OnOAuthLogIn(bAutoLogin, HandleOAuthLoginResult, aParams);
	}

	internal void InterOAuthLogOut()
	{
		m_bLoginStart = false;
		m_bLogin = false;
		OnOAuthLogOut();
	}
	//--------------------------------------------------------------
	private void HandleOAuthLoginResult(bool bResult, string strErrorMessage)
	{
		m_bLoginStart = false;
		if (bResult == true)
		{
			m_bLogin = true;
		}
		m_delFinish?.Invoke(bResult);
	} 

	//-------------------------------------------------------------
	protected virtual void OnOAuthInitialize() { }
	protected virtual void OnOAuthLogIn(bool bAutoLogin, UnityAction<bool, string> delFinish, params object[] aParams) { }
	protected virtual void OnOAuthLogOut() { }
}
