using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFrameLobby : UIFrameTTBase
{
    [SerializeField] private CText TxtLevel;
    [SerializeField] private CText TxtNickname;
    [SerializeField] private CText TxtHeart;
    [SerializeField] private CText TxtGold;
    [SerializeField] private CText TxtCrystal;
    //---------------------------------------------------------

    //프로필 버튼 / UIButtonProfile
    public void HandleOnClickButtonProfile() { }

    //메뉴 버튼 / UIButtonMenu
    public void HandleOnClickButtonMenu() { }

    //UI가리기 버튼 / UIButtonHideUI
    public void HandleOnClickButtonHideUI() { }

    //게임시작 버튼 / UIButtonPlayGame
    public void HandleOnClickButtonPlayGame() { }

    //화면 상단 각종 재화 버튼들 / UILobbyResources
    public void HandleOnClickButtonBuyHeart() { }
    public void HandleOnClickButtonBuyGold() { }
    public void HandleOnClickButtonBuyCrystal() { }

    //화면 좌측 버튼들 / UILobbySelectionsLeft
    public void HandleOnClickButtonFriends() { }
    public void HandleOnClickButtonMail() { }

    //화면 하단 버튼들 / UILobbySelectionsBottom
    public void HandleOnClickButtonListen() { }
    public void HandleOnClickButtonStory() { }
    public void HandleOnClickButtonHome() { }
    public void HandleOnClickButtonGallery() { }
    public void HandleOnClickButtonShop() { }
}
