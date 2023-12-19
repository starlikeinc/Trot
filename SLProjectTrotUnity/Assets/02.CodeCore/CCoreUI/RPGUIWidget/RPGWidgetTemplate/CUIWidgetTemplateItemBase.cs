using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CUIWidgetTemplateItemBase : CUIWidgetBase
{
	private int m_iItemIndex = 0;
	private CUIWidgetTemplateBase m_pParent = null;

    //-----------------------------------------------------------
    protected override void OnUIWidgetOwner(CUIEntryBase pWidgetOwner)
    {
        base.OnUIWidgetOwner(pWidgetOwner);
		m_pParent = pWidgetOwner as CUIWidgetTemplateBase;
    }
    //------------------------------------------------------------
    public void DoUITemplateItemShow(bool bShow)
    {
		DoUIWidgetShowHide(bShow);
        OnUIWidgetTemplateItemShow(bShow);
    }

    public void DoUITemplateItemReturn() // 비용이 상당하므로 사용시 주의
	{
		m_pParent.DoUITemplateReturn(this);		
	}

	public void DoUITemplateItemRefreshIndex(int iIndex)
	{
		m_iItemIndex = iIndex;
		OnUIWidgetTemplateItemRefresh(iIndex);
	}

	public int GetUITemplateItemIndex()
	{
		return m_iItemIndex;
	}

	internal void InterUITemplateItemReturn()
	{
		OnUIWidgetTemplateItemReturn();
	}

	//--------------------------------------------------------
	protected virtual void OnUIWidgetTemplateItemShow(bool bShow) { }
	protected virtual void OnUIWidgetTemplateItemRefresh(int iIndex) { }
	protected virtual void OnUIWidgetTemplateItemReturn() { }
}
