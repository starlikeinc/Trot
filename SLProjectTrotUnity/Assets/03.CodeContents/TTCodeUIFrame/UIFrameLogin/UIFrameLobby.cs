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

    //������ ��ư / UIButtonProfile
    public void HandleOnClickButtonProfile() { }

    //�޴� ��ư / UIButtonMenu
    public void HandleOnClickButtonMenu() { }

    //UI������ ��ư / UIButtonHideUI
    public void HandleOnClickButtonHideUI() { }

    //���ӽ��� ��ư / UIButtonPlayGame
    public void HandleOnClickButtonPlayGame() { }

    //ȭ�� ��� ���� ��ȭ ��ư�� / UILobbyResources
    public void HandleOnClickButtonBuyHeart() { }
    public void HandleOnClickButtonBuyGold() { }
    public void HandleOnClickButtonBuyCrystal() { }

    //ȭ�� ���� ��ư�� / UILobbySelectionsLeft
    public void HandleOnClickButtonFriends() { }
    public void HandleOnClickButtonMail() { }

    //ȭ�� �ϴ� ��ư�� / UILobbySelectionsBottom
    public void HandleOnClickButtonListen() { }
    public void HandleOnClickButtonStory() { }
    public void HandleOnClickButtonHome() { }
    public void HandleOnClickButtonGallery() { }
    public void HandleOnClickButtonShop() { }
}
