using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 켄버스를 사용하므로 드로우콜을 발생시킴
// 자동으로 부모 프레임의 쏘트 오더를 확인하여 갱신
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public abstract class CUIWidgetCanvasBase : CUIWidgetBase
{
	[SerializeField]
	private int SortingOrderOffset = 0;

	private int m_iUIFrameOrder = 0;
	private Canvas m_pWidgetCanvas = null;
	//-------------------------------------------------------------------------
	protected override void OnUIWidgetInitialize(CUIFrameBase pParentFrame)
	{
		base.OnUIWidgetInitialize(pParentFrame);
		m_pWidgetCanvas = GetComponent<Canvas>();
		
		Canvas pCanvas = pParentFrame.GetUIFrameCanvas();
		m_iUIFrameOrder = pCanvas.sortingOrder;
		m_pWidgetCanvas.sortingLayerID = pCanvas.sortingLayerID;
		m_pWidgetCanvas.sortingOrder = m_iUIFrameOrder;
	}

	protected override void OnUIWidgetRefreshOrder(int iOrder)
	{
		base.OnUIWidgetRefreshOrder(iOrder);
		m_iUIFrameOrder = iOrder;
		if (GetUIWidgetShow())
        {
			PrivUIWidgetCanvasSortingOrder();
		}
	}

    protected override void OnUIWidgetShowHide(bool bShow)
    {
        base.OnUIWidgetShowHide(bShow);
		if (bShow)
        {
			PrivUIWidgetCanvasSortingOrder();
		}
    }

    protected override void OnUIWidgetFrameShowHide(bool bShow)
    {
		if (bShow)
        {
			PrivUIWidgetCanvasSortingOrder();
		}
    }

    protected override void OnUIWidgetAdd(CUIFrameBase pParentFrame)
	{
		base.OnUIWidgetAdd(pParentFrame);
	}

	//----------------------------------------------------------------------
	private void PrivUIWidgetCanvasSortingOrder()
    {
		m_pWidgetCanvas.overrideSorting = true;
		m_pWidgetCanvas.sortingOrder = m_iUIFrameOrder + SortingOrderOffset + 1;
	}
}
