using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFrameLogin : UIFrameTTBase
{

    //----------------------------------------------------------------
    public void HandleOnClickButtonLogin()
    {
        TTManagerSceneLoader.Instance.DoMgrSceneLoaderGoToLobby(() =>
        {
            UIManager.Instance.UIHide<UIFrameLogin>();
            UIManager.Instance.UIShow<UIFrameLobby>();
        });
    }

}
