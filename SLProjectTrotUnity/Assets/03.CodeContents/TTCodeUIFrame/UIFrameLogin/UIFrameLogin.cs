using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFrameLogin : UIFrameTTBase
{
    [SerializeField]
    private CButton BtnLogin;

    //----------------------------------------------------------------
    public void HandleOnClickButtonLogin()
    {
        TTManagerSceneLoader.Instance.DoMgrSceneLoaderGoToLobby(() =>
        {
            UIManager.Instance.UIHide<UIFrameLogin>();
        });
    }

}
