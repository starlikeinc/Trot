using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 자식 위젯을 관리하기 위한 레이어
abstract public class CUIFrameEntryBase : CUIFrameBase
{
    private List<CUIEntryBase> m_listEntryInstance = new List<CUIEntryBase>();
    private LinkedList<CUIWidgetWindowBase> m_listWidgetWindow = new LinkedList<CUIWidgetWindowBase>(); // 순서대로 Show / Hide되는 내부 창 (클로스 버튼시 하나씩 사라짐)
    //--------------------------------------------------------------
    protected override void OnUIFrameInitialize() 
    {
       	PrivUIFrameInitializeUIWidget(); 
    }

    protected override void OnUIFrameInitializePost()
    {
        PrivUIFrameInitializeUIWidgetPost();
    }

    protected override void OnUIFrameRefreshOrder(int iOrder) 
    {
        for (int i = 0; i < m_listEntryInstance.Count; i++)
        {
            m_listEntryInstance[i].InterUIWidgetRefreshOrder(iOrder);
        }
    }

    protected override void OnUIFrameShow()
    {
        for (int i = 0; i < m_listEntryInstance.Count; i++)
        {
            m_listEntryInstance[i].InterUIWidgetUIFramwShowHide(true);
        }
    }

    protected override void OnUIFrameHide()
    {
        for (int i = 0; i < m_listEntryInstance.Count; i++)
        {
            m_listEntryInstance[i].InterUIWidgetUIFramwShowHide(false);
        }
    }

    //------------------------------------------------------------------
    internal void InterUIWidgetAdd(CUIWidgetBase pWidget)
    {
        m_listEntryInstance.Add(pWidget);
        pWidget.InterUIWidgetAdd(this);
    }

    internal void InterUIWidgetDelete(CUIWidgetBase pWidget)
    {
        m_listEntryInstance.Remove(pWidget);
        pWidget.InterUIWidgetDelete(this);
    }

    internal int InterUIWindowShowHide(CUIWidgetWindowBase pWidgetWindow, bool bShow)
    {
        m_listWidgetWindow.Remove(pWidgetWindow);
        if (IsShow)
        {
            m_listWidgetWindow.AddLast(pWidgetWindow);
        }

        return m_listWidgetWindow.Count;
    }

    internal void InternalUIToolTipShow(CUIWidgetWindowBase pWidgetWindow)
    {
        if (pWidgetWindow == null) return;

    }

    //------------------------------------------------------------------------
    public void DoUIFrameSelfClose()  // 뒤로 가기 버튼등으로 닫는경우. PanelChain 일 경우 마지막 부터 닫힌다. Frame 내부에서도 Close판정을 한다.
    {
        if (PrivUIFrameCloseWindow())
        {
            DoUIFrameSelfHide();
        }
    }

    //-------------------------------------------------------------------------
    private void PrivUIFrameInitializeUIWidget()
    {
        GetComponentsInChildren(true, m_listEntryInstance);

        for (int i = 0; i < m_listEntryInstance.Count; i++)
        {
            m_listEntryInstance[i].InterUIWidgetInitialize(this);
        }
    }

    private void PrivUIFrameInitializeUIWidgetPost()
    {
        for (int i = 0; i < m_listEntryInstance.Count; i++)
        {
            m_listEntryInstance[i].InterUIWidgetInitializePost(this);
        }
    }

    private bool PrivUIFrameCloseWindow()
    {
        bool bAllClose = true;

        return bAllClose;
    }

}
