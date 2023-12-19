using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public abstract class CUIFrameBase : CMonoBase, IPointerClickHandler
{
	[SerializeField] 
	private CManagerUIFrameFocusBase.EUIFrameFocusType FocusType = CManagerUIFrameFocusBase.EUIFrameFocusType.Panel;

	private bool					m_bShow = false;			public bool IsShow { get { return m_bShow; } }			// 현재 보이는지 안 보이는지 
	private bool					m_bAppear = false;		    public bool IsAppear { get { return m_bAppear; } }	    // 다른 프레임에 의해 임으로 가려진 상태
	private int					    m_iSortOrder = 0;			public int  p_SortOrder { get { return m_iSortOrder; } }
	private Canvas					m_pCanvas;				    public Canvas GetUIFrameCanvas() { return m_pCanvas; }
	private GraphicRaycaster		m_pGraphicRaycaster;
	private RectTransform		    m_pRectTransform;
	private Vector2					m_vecUIFrameSize = Vector2.zero;
	

	//--------------------------------------------------------------------------
	internal void InterUIFrameInitialize()	// UI프레임이 메니저에 의해 로드된 이후 호출
	{
		m_pCanvas = GetComponent<Canvas>();
		m_pRectTransform = GetComponent<RectTransform>();
		m_pGraphicRaycaster = GetComponent<GraphicRaycaster>();

		m_vecUIFrameSize.x = m_pRectTransform.rect.width;
		m_vecUIFrameSize.y = m_pRectTransform.rect.height;

		SetMonoActive(true);
		m_pCanvas.overrideSorting = true;
		m_pCanvas.sortingLayerName = LayerMask.LayerToName(gameObject.layer);
		SetMonoActive(false);
		OnUIFrameInitialize();
	}

	internal void InterUIFrameInitializePost()
	{
		OnUIFrameInitializePost();
	}

	internal void InterUIFrameShow(int iOrder)
	{
		SetMonoActive(true);
		m_bShow = true;
		m_bAppear = true;
		InterUIFrameRefreshOrder(iOrder);
		OnUIFrameShow();
	}

	internal void InterUIFrameAppear()             // 다른 프레임에 의해 임으로 가려진 상태
	{
        if (m_bAppear) return;
		SetMonoActive(true);
		m_bAppear = true;
		OnUIFrameAppear();
	}

	internal void InterUIFrameDisappear()
	{
		if (m_bAppear == false) return;
		SetMonoActive(false);
		m_bAppear = false;
		OnUIFrameDisappear();
	}

	internal void InterUIFrameRefreshOrder(int iOrder)	// 자신의 오더가 변경되었을 경우
	{
		if (iOrder != 0)
		{
			m_iSortOrder = iOrder;
			m_pCanvas.sortingOrder = iOrder;
		}
		else
		{
			m_iSortOrder = m_pCanvas.sortingOrder;
		}
        m_pCanvas.overrideSorting = true;
        OnUIFrameRefreshOrder(iOrder);
    }

	internal void InterUIFrameHide()
	{
		SetMonoActive(false);
		m_bShow = false;
		m_bAppear = false;
		OnUIFrameHide();
	}

	internal void InterUIFrameForceHide()
	{
		SetMonoActive(false);
		m_bShow = false;
		OnUIFrameForceHide();
	}

	internal void InterUIFrameRemove()
	{
		OnUIFrameRemove();
	}

	internal void InterUIFrameClose()  // 자신이 아닌 외부에 의해 종료되었다. (디바이스 뒤로가기 버튼)
	{
		OnUIFrameClose();
	}
 
    //---------------------------------------------------------------------
    private void Update()
	{
		OnUIFrameUpdate();
	}
    // 주의 : 해당 프레임의 사이즈가 화면 전체로 되어 있어야 한다.
	public static Vector2 WorldToCanvas(Vector3 vecWorldPosition, Camera pCamera = null)
	{
		if (pCamera == null)
		{
			pCamera = Camera.main;
		}     
        return pCamera.WorldToScreenPoint(vecWorldPosition);
    }

	public void DoUIFrameSelfHide()
    {
		CManagerUIFrameUsageBase.Instance.UIHide(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnUIFramePointerClick(eventData);
    }

	public void SetUIFrameInputEnable(bool bEnable)
	{
		m_pGraphicRaycaster.enabled = bEnable;
	}
   
    //--------------------------------------------------------------------------
    protected virtual void OnUIFrameInitialize() { }		// 이 단계에서는 다른 UIFrame을 인식 할 수 없다.
	protected virtual void OnUIFrameInitializePost() { }	// 모든 UIFrame 업로드되어 인식할 수 있다.
	protected virtual void OnUIFrameShow() { }
	protected virtual void OnUIFrameRefreshOrder(int iOrder) { }
	protected virtual void OnUIFrameHide() { }
	protected virtual void OnUIFrameForceHide() { }
	protected virtual void OnUIFrameRemove() { }
	protected virtual void OnUIFrameUpdate() { }
	protected virtual void OnUIFrameAppear() { }
	protected virtual void OnUIFrameDisappear() { }
	protected virtual void OnUIFrameClose() { }
	protected virtual void OnUIFramePointerClick(PointerEventData eventData) { }
	//--------------------------------------------------------------------------
	public CManagerUIFrameFocusBase.EUIFrameFocusType GetUIFrameFocusType() { return FocusType; }
}
