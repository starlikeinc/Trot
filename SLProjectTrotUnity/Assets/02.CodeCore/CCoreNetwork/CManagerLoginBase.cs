using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CManagerLoginBase : CManagerTemplateBase<CManagerLoginBase>
{
    public enum ELoginState
    {
        None,
        TryLogin,
        LoginSucess,
        LoginFail,
    }

    private ELoginState eLoginState = ELoginState.None;
    private CLoginBase m_pLoginInstance = null;
    //--------------------------------------------------------
    protected void ProtMgrLoginInstance(CLoginBase pLogin)
    {
        m_pLoginInstance = pLogin;
        m_pLoginInstance.InterOAuthLogInInitialize();
    }

    protected void ProtMgrLogIn(bool bAutoLogin, UnityAction<bool> delFinish,  params object [] aParams)
    {
        m_pLoginInstance.InterOAuthLogIn(bAutoLogin, delFinish, aParams);

    }

    protected void ProtMgrLogOut()
    {
        m_pLoginInstance.InterOAuthLogOut();
    }
}
