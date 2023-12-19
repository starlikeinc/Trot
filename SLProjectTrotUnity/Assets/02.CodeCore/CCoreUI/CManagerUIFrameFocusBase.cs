using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public abstract class CManagerUIFrameFocusBase : CManagerUIFrameBase
{
	private const int c_GapPannel = 10;
	private const int c_GapPopup = 10000;
     
	public enum EUIFrameFocusType
	{
		None,
		Invisible,			// 항상 보여진다. 별도로 관리되지 않는다. 주로 Image없이 기능적인 작동만 하는 경우에 사용한다.
		ToolTip,			// 항상 Topmost 로 배열된다. 툴팁은 하나만 보여진다. 
		Panel,				// Show 순서에 따라 정렬되어 출력된다.
		PanelChain,			// 마지막 프레임만 Show된다. Hide시 이전 패널이 Show된다. 클로즈 버튼에 대응한다.
		PanelTopMost,		// 패널중 항상 위쪽에 위치한다. 
		PanelFullScreen,	// Show시 모든  Panel을 Hide 시킨다. 하나의 포커스만 존재한다. 		
		Popup,				// Panel보다 항상 위에 그려진다.   
		PopupExclusive,		// Popup을 모두 Hide시키고 자신을 보여준다. 닫힐 경우 이전 Popup이 출력된다. 
	}

	private int m_iCanvasOrder = 0;
	private LinkedList<CUIFrameBase> m_listFrameOrderPanel = new LinkedList<CUIFrameBase>();
	private LinkedList<CUIFrameBase> m_listFrameOrderPopup = new LinkedList<CUIFrameBase>();
	private LinkedList<CUIFrameBase> m_listFramePanelChain = new LinkedList<CUIFrameBase>();
	private LinkedList<CUIFrameBase> m_listFrameTopMost	 = new LinkedList<CUIFrameBase>();

	private CUIFrameBase m_pFullScreen = null;
	//--------------------------------------------------------------
	protected override void OnUIMgrInitializeCanvas(Canvas pRootCanvas)
	{
		base.OnUIMgrInitializeCanvas(pRootCanvas);
		m_iCanvasOrder = pRootCanvas.sortingOrder;
	} 
	//--------------------------------------------------------------
	protected void ProtMgrUIFrameFocusShow(CUIFrameBase pUIFrame) 
	{
		PrivMgrUIFrameFocusShow(pUIFrame); 
	} 

	protected CUIFrameBase ProtMgrUIFrameFocusShow(string strFrameName)
	{
		CUIFrameBase pUIFrame = FindUIFrame(strFrameName);
		if (pUIFrame != null)
		{
			PrivMgrUIFrameFocusShow(pUIFrame);
		}
		else
		{
			Debug.LogError("[UIFrame] Invalid UIFrame : " + strFrameName);
		}
		return pUIFrame;
	}

	protected void ProtMgrUIFrameFocusHide(CUIFrameBase pUIFrame) 
	{
		PrivMgrUIFrameFocusHide(pUIFrame); 
	}

	protected CUIFrameBase ProtMgrUIFrameFocusHide(string strFrameName)
	{
		CUIFrameBase pUIFrame = FindUIFrame(strFrameName);
		if (pUIFrame != null)
		{
			PrivMgrUIFrameFocusHide(pUIFrame);
		}
		return pUIFrame;
	}

	protected void ProtMgrUIFrameFocusClose()
	{
		if (m_listFramePanelChain.Count > 0)
		{
			if (m_listFramePanelChain.Last != null)
            {
                CUIFrameBase pUIFrame = m_listFramePanelChain.Last.Value;
                PrivMgrUIFrameFocusClose(pUIFrame);
            }
        }
		else
		{
			OnMgrUIFramePanelChainEmpty();
		}
	}

	protected void ProtMgrUIFrameFocusPanelHideAll()
	{
		List<CUIFrameBase> pListOrder = m_listFrameOrderPanel.ToList();
		for (int i = 0; i < pListOrder.Count; i++)
		{
			pListOrder[i].InterUIFrameForceHide();
		}
		m_listFrameOrderPanel.Clear();
		pListOrder = m_listFrameTopMost.ToList();
		for (int i = 0; i < pListOrder.Count; i++)
		{
			pListOrder[i].InterUIFrameForceHide();
		}

		m_listFrameOrderPanel.Clear();
		if (m_pFullScreen != null)
		{
			m_pFullScreen.InterUIFrameForceHide();
			m_pFullScreen = null;
		}
	}

	//---------------------------------------------------------------
	private void PrivMgrUIFrameFocusShow(CUIFrameBase pUIFrame)
	{
		EUIFrameFocusType eFocusType = pUIFrame.GetUIFrameFocusType();
		switch (eFocusType)
		{
			case EUIFrameFocusType.Invisible:
				PrivMgrUIFrameFocusInvisibleShow(pUIFrame);
				break;
			case EUIFrameFocusType.ToolTip:
				break;
			case EUIFrameFocusType.Panel:
				PrivMgrUIFrameFocusPanelShow(pUIFrame);
				break;
			case EUIFrameFocusType.PanelChain:
				PrivMgrUIFrameFocusPanelChainShow(pUIFrame);
				break;
			case EUIFrameFocusType.PanelFullScreen:
				PrivMgrUIFrameFocusFullScreenShow(pUIFrame);
				break;
			case EUIFrameFocusType.PanelTopMost:
				PrivMgrUIFrameFocusPannelTopMostShow(pUIFrame);
				break;
			case EUIFrameFocusType.Popup:
				PrivMgrUIFrameFocusPopupShow(pUIFrame);
				break;
			case EUIFrameFocusType.PopupExclusive:
				PrivMgrUIFrameFocusPopupShow(pUIFrame);
				break;
		}
	}

	private void PrivMgrUIFrameFocusHide(CUIFrameBase pUIFrame)
	{
		if (pUIFrame.IsShow == false) return;

		EUIFrameFocusType eFocusType = pUIFrame.GetUIFrameFocusType();
		switch (eFocusType)
		{
			case EUIFrameFocusType.Invisible:
				break;
			case EUIFrameFocusType.ToolTip:
				break;
			case EUIFrameFocusType.Panel:
				PrivMgrUIFrameFocusPanelHide(pUIFrame);
				break;
			case EUIFrameFocusType.PanelChain:
				PrivMgrUIFrameFocusPanelChainHide(pUIFrame);
				break;
			case EUIFrameFocusType.PanelFullScreen:
				PrivMgrUIFrameFocusFullScreenHide(pUIFrame);
				break;
			case EUIFrameFocusType.PanelTopMost:
				PrivMgrUIFrameFocusPanelTopMostHide(pUIFrame);
				break;
			case EUIFrameFocusType.Popup:
				PrivMgrUIFrameFocusPopupHide(pUIFrame);
				break;
			case EUIFrameFocusType.PopupExclusive:
				PrivMgrUIFrameFocusPopupHide(pUIFrame);
				break;
		}
	}

	//---------------------------------------------------------------

	private void PrivMgrUIFrameFocusPanelShow(CUIFrameBase pUIFrame)
	{
		m_listFrameOrderPanel.Remove(pUIFrame);
		m_listFrameOrderPanel.AddLast(pUIFrame);
		int iOrder = ExtractUIOrderPanel();
		if (pUIFrame.IsShow)
		{
			PrivMgrUIFramArrageRefresh(m_listFrameOrderPanel, iOrder);
		}
		else
		{
			pUIFrame.InterUIFrameShow(iOrder);
			OnMgrUIFrameShow(pUIFrame);
		}
		PrivMgrUIFramArrageRefresh(m_listFrameTopMost, ExtractUIOrderPanelTopMost());
	}

	private void PrivMgrUIFrameFocusFullScreenShow(CUIFrameBase pUIFrame)
	{
		if (m_pFullScreen)
		{
			PrivMgrUIFrameFocusPanelHide(m_pFullScreen);
		}
		m_pFullScreen = pUIFrame;
		List<CUIFrameBase> pListFrame = m_listFrameOrderPanel.ToList();
		for (int i = 0; i < pListFrame.Count; i++)
		{
			pListFrame[i].InterUIFrameDisappear();
		}

		m_pFullScreen.InterUIFrameShow(ExtractUIOrderPanel());
		PrivMgrUIFramArrageRefresh(m_listFrameTopMost, ExtractUIOrderPanelTopMost());

		CManagerStageCameraBase.Instance?.DoStageCameraRenderEnableAll(false);
	}

	private void PrivMgrUIFrameFocusPanelChainShow(CUIFrameBase pUIFrame)
	{
		PrivMgrUIFrameFocusPanelShow(pUIFrame);
		m_listFramePanelChain.Remove(pUIFrame);
		m_listFramePanelChain.AddLast(pUIFrame);
	}

	private void PrivMgrUIFrameFocusPannelTopMostShow(CUIFrameBase pUIFrame)
	{
		m_listFrameTopMost.Remove(pUIFrame);
		m_listFrameTopMost.AddLast(pUIFrame);
		int iOrder = ExtractUIOrderPanelTopMost();

		if (pUIFrame.IsShow)
		{
			PrivMgrUIFramArrageRefresh(m_listFrameTopMost, iOrder);
		}
		else
		{
			pUIFrame.InterUIFrameShow(iOrder);
			OnMgrUIFrameShow(pUIFrame);
		}
	}

	private void PrivMgrUIFrameFocusInvisibleShow(CUIFrameBase pUIFrame)
	{
		pUIFrame.InterUIFrameShow(0);
	}

	private void PrivMgrUIFrameFocusPanelHide(CUIFrameBase pUIFrame)
	{
		m_listFrameOrderPanel.Remove(pUIFrame);
		pUIFrame.InterUIFrameHide();
		PrivMgrUIFramArrageRefresh(m_listFrameOrderPanel, m_iCanvasOrder);
	}

	private void PrivMgrUIFrameFocusPanelChainHide(CUIFrameBase pUIFrame)
	{
		PrivMgrUIFrameFocusPanelHide(pUIFrame);
		m_listFramePanelChain.Remove(pUIFrame);
	}

	private void PrivMgrUIFrameFocusFullScreenHide(CUIFrameBase pUIFrame)
	{		
		pUIFrame.InterUIFrameHide();

		if (m_pFullScreen == pUIFrame)
		{
			PrivMgrUIFramArrageRefresh(m_listFrameOrderPanel, m_iCanvasOrder);

			List<CUIFrameBase> pListFrame = m_listFrameOrderPanel.ToList();
			for (int i = 0; i < pListFrame.Count; i++)
			{
				pListFrame[i].InterUIFrameAppear();
			}
			m_pFullScreen = null;
			CManagerStageCameraBase.Instance?.DoStageCameraRenderEnableAll(true);
		}
	}

	private void PrivMgrUIFrameFocusPanelTopMostHide(CUIFrameBase pUIFrame)
	{
		m_listFrameTopMost.Remove(pUIFrame);
		pUIFrame.InterUIFrameHide();
		PrivMgrUIFramArrageRefresh(m_listFrameTopMost, ExtractUIOrderPanelTopMost());
	}

	private void PrivMgrUIFrameFocusClose(CUIFrameBase pUIFrame)
	{
		PrivMgrUIFrameFocusHide(pUIFrame);
		pUIFrame.InterUIFrameClose();
		OnMgrUIFrameClose(pUIFrame);
	}

	private void PrivMgrUIFrameFocusPopupShow(CUIFrameBase pUIFrame)
	{
		m_listFrameOrderPopup.Remove(pUIFrame);
		m_listFrameOrderPopup.AddLast(pUIFrame);

		if (pUIFrame.IsShow)
		{
			PrivMgrUIFramArrageRefresh(m_listFrameOrderPopup, ExtractUIOrderPopupBase());
		}
		else
		{
			pUIFrame.InterUIFrameShow(ExtractUIOrderPopup());
			OnMgrUIFrameShow(pUIFrame);
		}
	}

	private void PrivMgrUIFrameFocusPopupHide(CUIFrameBase pUIFrame)
	{
		m_listFrameOrderPopup.Remove(pUIFrame);
		pUIFrame.InterUIFrameHide();
		PrivMgrUIFramArrageRefresh(m_listFrameOrderPopup, ExtractUIOrderPopupBase());
	}
	//------------------------------------------------------------------
	private void PrivMgrUIFramArrageRefresh(LinkedList<CUIFrameBase> pListOrder, int iBaseOrder)
	{
		List<CUIFrameBase> pListFrame = pListOrder.ToList();
		for (int i = 0; i < pListFrame.Count; i++)
		{
			pListFrame[i].InterUIFrameRefreshOrder(iBaseOrder);
			iBaseOrder += c_GapPannel;
		}
	}

	private int ExtractUIOrderPanel()
	{
		return m_listFrameOrderPanel.Count * c_GapPannel + m_iCanvasOrder;
	}

	private int ExtractUIOrderPanelTopMost()
	{
		return ExtractUIOrderPanel() + (m_listFrameTopMost.Count * c_GapPannel);
	}

	private int ExtractUIOrderPopup()
	{
		return ExtractUIOrderPopupBase() + (m_listFrameOrderPopup.Count * c_GapPannel);
	}

	private int ExtractUIOrderPopupBase()
	{
		return ExtractUIOrderPanel() + c_GapPopup;
	}
	//------------------------------------------------------------------
	protected virtual void OnMgrUIFrameShow(CUIFrameBase pUIFrame) { }	
	protected virtual void OnMgrUIFrameHide(CUIFrameBase pUIFrame) { }
	protected virtual void OnMgrUIFrameClose(CUIFrameBase pUIFrame) { }
	protected virtual void OnMgrUIFrameShowFromOther(CUIFrameBase pUIFrame) { }
	protected virtual void OnMgrUIFrameHideFromOther(CUIFrameBase pUIFrame) { }
	protected virtual void OnMgrUIFramePanelChainEmpty() { }	
}
