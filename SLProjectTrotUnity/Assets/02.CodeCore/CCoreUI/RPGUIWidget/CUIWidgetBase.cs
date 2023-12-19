using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [주의] UIWidget의 ShowHide는 그래픽 컴포넌트를 비활성한다.
// 게임오브젝트를 키거나 끌 경우 버텍스 버퍼가 재 할당되어
// UI프레임에 상당한 부하가 걸리게 되기 때문이다. 

public abstract class CUIWidgetBase : CUIEntryBase
{
	private bool m_bShow = false; public bool GetUIWidgetShow() { return m_bShow; }
	private List<Graphic>           m_listChildGraphic = new List<Graphic>();
    private List<CUIWidgetBase>     m_listChildWidget  = new List<CUIWidgetBase>();

	//-----------------------------------------------------------
	protected override void OnUIWidgetInitialize(CUIFrameBase pParentFrame)
	{
		base.OnUIWidgetInitialize(pParentFrame);
		PrivUIWidgetCollectChildWidget();
	}


    internal void InterUIWidgetShowHideForce(bool bShow)
    {
        if (bShow)
        {
            if (m_bShow)
            {
				PrivUIWidgetGraphicShowHide(true);
				PrivUIWidgetChildShowHideForce(true);
                OnUIWidgetShowHideForce(true);
			}
        }
        else
        {
            PrivUIWidgetGraphicShowHide(false);
            PrivUIWidgetChildShowHideForce(false);
			OnUIWidgetShowHideForce(false);
		}
	}

    //-----------------------------------------------------------
    public void DoUIWidgetShowHide(bool bShow)  // 게임 오브젝트를 끄지 않으므로 업데이트는 이루어짐을 유의
	{
        PrivUIWidgetShowHide(bShow);
		OnUIWidgetShowHide(bShow);
    }

	public void DoUIWidgetSelectFromOtherWidget(CUIWidgetBase pSelectedWidget)
	{
		OnUIWidgetSelect(pSelectedWidget);
	}

	public void DoUIWidgetReset()
    {
		OnUIWidgetReset();
    }

    //----------------------------------------------------------
    public void SetUIWidgetRayCastTarget(bool bOn, bool bChild = false)  
	{       
		for (int i = 0; i < m_listChildGraphic.Count; i++)
		{
            m_listChildGraphic[i].raycastTarget = bOn;
		}
	}


	//--------------------------------------------------------
	private void PrivUIWidgetShowHide(bool bShow)
    {
        m_bShow = bShow;
		PrivUIWidgetGraphicShowHide(bShow);
		PrivUIWidgetChildShowHideForce(bShow);
	}

    private void PrivUIWidgetGraphicShowHide(bool bShow)
    {
		for (int i = 0; i < m_listChildGraphic.Count; i++)
		{
			m_listChildGraphic[i].enabled = bShow;
		}
	}

    private void PrivUIWidgetChildShowHideForce(bool bShow)
    {
		for (int i = 0; i < m_listChildWidget.Count; i++)
		{
            CUIWidgetBase pUIWidget = m_listChildWidget[i];
            pUIWidget.InterUIWidgetShowHideForce(bShow);
		}
	}

	private void PrivUIWidgetCollectChildWidget()
    {
        RecursiveUIWidgetChildTransform(transform);
	}

    private void RecursiveUIWidgetChildTransform(Transform pRecursiveTransform)
    {
        int iTotalCount = pRecursiveTransform.childCount;

        for (int i = 0; i < iTotalCount; i++) 
        {
            Transform pChildTransform = pRecursiveTransform.GetChild(i);
            CUIWidgetBase pChildWidget = pChildTransform.GetComponent<CUIWidgetBase>();
            if (pChildWidget != null) 
            {
                m_listChildWidget.Add(pChildWidget);
            }    
            else
            {
				MaskableGraphic pGraphic = pChildTransform.GetComponent<MaskableGraphic>();
                if (pGraphic != null)
                {
                    m_listChildGraphic.Add(pGraphic);
                    RecursiveUIWidgetChildTransform(pChildTransform);
                }
            }        
        }
    }

    //----------------------------------------------------------- 
    protected virtual void OnUIWidgetSelect(CUIWidgetBase pSelectedWidget) { }
    protected virtual void OnUIWidgetReset() { }
    //------------------------------------------------------------
   
}


