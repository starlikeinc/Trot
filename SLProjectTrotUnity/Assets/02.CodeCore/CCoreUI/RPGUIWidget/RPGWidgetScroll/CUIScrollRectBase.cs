using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CScrollRect))]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))] // 드로우 최적화를 위해 켄버스를 분리. 분리 하지 않으면  UIFrame 전체가 리드로우 된다.	
abstract public class CUIScrollRectBase : CUIWidgetTemplateBase
{
    [SerializeField][Header("[AnchorSpring]")]
    private bool AnchorSpring = false;
    [SerializeField][Range(0, 1f)]
    private float SpringOffsetRatioX = 0.5f;
    [SerializeField][Range(0, 1f)]
    private float SpringOffsetRatioY = 0.5f;
    [SerializeField]
    private float SpringUnderVelocity = 500f;
    [SerializeField]
    private AnimationCurve SpringCurve = null;
   
	[SerializeField][Header("[Misc]")] 
	private bool ParentDragEvent = false;   // 스크롤 랙트가 스크롤 랙트를 포함하고 있을 경우	
    [SerializeField]                   	    // 스크롤 바 아이템 위에 다양한 레이어 이미지를 출력하고자 할때 
    private CUIContentsFollow ContentsFollow = null; protected CUIContentsFollow GetUIScrollContentsFollow() {  return ContentsFollow; }

    [SerializeField]
    [Range(1f, 100f)]
    private float ScrollSensitivityScale = 1f; // 100 = 100% 기준으로 한번 스크롤 바 입력이 들어올때 스크롤 할 양

  
    private bool m_bSpringX = false;
    private bool m_bSpringY = false;
    private float m_fSpringXTimeCurrent = 0f;
    private float m_fSpringYTimeCurrent = 0f;       // ToDo 
    private Vector2 m_vecSpringOffset = Vector2.zero;
	protected CScrollRect m_pScrollRect = null;
	protected RectTransform m_pContentTransform = null;
    private Canvas m_pCanvas = null;
	//-------------------------------------------------------
	protected override void OnUIWidgetInitialize(CUIFrameBase pUIFrameParent)
	{
		base.OnUIWidgetInitialize(pUIFrameParent);
		m_pScrollRect = GetComponent<CScrollRect>();
        m_pCanvas = GetComponent<Canvas>();
        
        m_pCanvas.overrideSorting = true;
        m_pCanvas.sortingLayerName = pUIFrameParent.GetUIFrameCanvas().sortingLayerName;

		m_pScrollRect.onValueChanged.AddListener(HandleUIScrollRatioChange);
		m_pContentTransform = m_pScrollRect.content;

		if (m_pScrollRect.viewport == null)
		{
			m_pScrollRect.viewport = m_pScrollRect.transform as RectTransform;
		}

		if (ParentDragEvent)
		{
			m_pScrollRect.FindParentsScrollRect();
		}

        m_vecSpringOffset.x = m_pScrollRect.content.rect.width * SpringOffsetRatioX;
        m_vecSpringOffset.y = m_pScrollRect.content.rect.height * SpringOffsetRatioY;  // Pivot 0 / 1이라 나중에 별도로 케이스 계산 
	}

	protected override void OnUIWidgetRefreshOrder(int iOrder)
	{
		base.OnUIWidgetRefreshOrder(iOrder);
        m_pCanvas.sortingOrder = iOrder + 1;
	}

	private void Update()
    {
        if (AnchorSpring)
        {
            UpdateUIScrollAnchorSpring();
        }

        OnUIScrollUpdate();
	}

    //--------------------------------------------------------
    public Vector2 GetUIScrollViewportSize()
    {
		return m_pScrollRect.viewport.rect.size;
    }

	public Vector2 GetUIScrollPosition()
    {
		return m_pScrollRect.content.anchoredPosition;
    }

    public Vector2 GetUIScrollRatio()
    {
        return m_pScrollRect.normalizedPosition;
    }

    public RectTransform GetUIScrollContents()
    {
        return m_pContentTransform;
    }


    public void SetUIScrollRatio(Vector2 vecScrollRatio)
    {
        vecScrollRatio.x = Mathf.Clamp(vecScrollRatio.x, 0, 1f);
        vecScrollRatio.y = Mathf.Clamp(vecScrollRatio.y, 0, 1f);
        m_pScrollRect.normalizedPosition = vecScrollRatio;
    }

    public void SetUIScrollPosition(Vector2 vecScrollPosition)
    {
        m_pScrollRect.content.anchoredPosition = vecScrollPosition;
    }

    //-------------------------------------------------------
    protected CUIWidgetTemplateItemBase ProtUIScrollChildItem(int iIndex)
	{
		CUIWidgetTemplateItemBase pChildItem = null;

		if ( iIndex < m_pScrollRect.content.childCount)
		{
			pChildItem = m_pScrollRect.content.GetChild(iIndex).gameObject.GetComponent<CUIWidgetTemplateItemBase>();
		}

		return pChildItem;
	}

	protected List<TEMPLATE> ProtUIScrollChildItem<TEMPLATE>() where TEMPLATE : CUIWidgetTemplateItemBase
	{
		List<TEMPLATE> pListTemplate = new List<TEMPLATE>();
		for (int i = 0; i < m_pScrollRect.content.childCount; i++)
        {
			TEMPLATE pTempate = m_pScrollRect.content.GetChild(i).gameObject.GetComponent<TEMPLATE>();
			if (pTempate != null)
            {
                pListTemplate.Add(pTempate);
            }
        }
		return pListTemplate;
    }

	protected void ProtUIScrollSensitivityRatio(float fAdjustOffset = 1000f)
    {
		m_pScrollRect.scrollSensitivity = m_pScrollRect.content.rect.height * (ScrollSensitivityScale / fAdjustOffset);
    }
    //-----------------------------------------------------------	
    private void UpdateUIScrollAnchorSpring()
    {
        if (m_pScrollRect.IsDragging())
        {
            PrivUIScrollAnchorSpringReset();
        }
        else
        {
            UpdateUIScrollAnchorSpringFilter();          
        }       
    }

    private bool CheckUIScrollAnchorActivity(bool bHorizontal)
    {
        bool bActivity = false;
        if (bHorizontal)
        {
            if (m_pScrollRect.normalizedPosition.x != 1f && m_pScrollRect.normalizedPosition.x != 0f)
            {
                bActivity = true;
            }
        }
        else
        {
            if (m_pScrollRect.normalizedPosition.y != 1f && m_pScrollRect.normalizedPosition.y != 0f)
            {
                bActivity = true;
            }
        }
        return bActivity;
    }

    private void PrivUIScrollAnchorSpringReset()
    {
        m_bSpringX = false;
        m_bSpringY = false;
        m_fSpringXTimeCurrent = 0;
        m_fSpringYTimeCurrent = 0;
    }

    private void UpdateUIScrollAnchorSpringFilter()
    {        
        CUIWidgetTemplateItemBase pItem = GetUIScrollItemByViewportOffset(m_vecSpringOffset);     
        if (pItem == null) return;
        
        Vector2 vecVelocity = m_pScrollRect.velocity;
        Vector2 vecContentPosition = GetUIPositionLeftTop(m_pContentTransform);
        Vector2 vecOffsetPosition = (pItem.GetUIPositionLeftTop() + vecContentPosition);
        vecOffsetPosition += pItem.GetUISize() / 2;
        Vector2 vecDestPosition = Vector2.zero;
        vecDestPosition.x = m_vecSpringOffset.x - vecOffsetPosition.x + vecContentPosition.x;
        vecDestPosition.y = vecOffsetPosition.y - m_vecSpringOffset.y + vecContentPosition.y;

        if (m_pScrollRect.horizontal)
        {
            if (CheckUIScrollAnchorActivity(true))
            {
                if (vecDestPosition.x != m_pContentTransform.anchoredPosition.x)
                {
                    if (Mathf.Abs(vecVelocity.x) < SpringUnderVelocity) // 스크롤의 가속도를 정지시키고 별도의 커브로 이동시켜준다.
                    {
                        vecVelocity.x = 0;
                        m_bSpringX = true;
                    }
                }
            }
        }
        
        if (m_pScrollRect.vertical)
        {
            //ToDo
        }

        m_pScrollRect.velocity = vecVelocity;

        if (m_bSpringX)
        {
            UpdateUIScrollAnchorSpringHorizontal(pItem, vecContentPosition,  vecDestPosition.x);
        }
        if (m_bSpringY)
        {
            //ToDo

        }
    }

    private void UpdateUIScrollAnchorSpringHorizontal(CUIWidgetTemplateItemBase pItem, Vector2 vecAnchorPosition, float fDestPosition)
    {      
        float fCurveValue = 0;
        float fMoveValue = 0;
        float fVelocity = 0;
        
        m_fSpringXTimeCurrent += Time.deltaTime;
        fCurveValue = SpringCurve.Evaluate(m_fSpringXTimeCurrent);
        fVelocity = m_pScrollRect.decelerationRate * SpringUnderVelocity * fCurveValue;
    
        if (fDestPosition - vecAnchorPosition.x > 0)
        {
            fMoveValue = vecAnchorPosition.x + fVelocity;
            fMoveValue = Mathf.Clamp(fMoveValue, vecAnchorPosition.x, fDestPosition);
        }
        else if (fDestPosition - vecAnchorPosition.x < 0)
        {
            fMoveValue = vecAnchorPosition.x - fVelocity;
            fMoveValue = Mathf.Clamp(fMoveValue, fDestPosition, vecAnchorPosition.x);
        }


        if (fMoveValue == fDestPosition)
        {
            m_bSpringX = false;
            m_fSpringXTimeCurrent = 0;
            OnUIScrollAnchorSpringEnd();
        }
        else
        {          
            OnUIScrollAnchorRemainOffsetX(pItem, fDestPosition - fMoveValue);
        }
        vecAnchorPosition.x = fMoveValue;
        m_pContentTransform.anchoredPosition = vecAnchorPosition;
    }

    private void UpdateUIScrollAnchorSpringVertical() // Todo
    {

    }

    private void PrivUIScrollAnchorSpringUpdateOffset()
    {
        int iChildTotal = m_pScrollRect.content.childCount;
        Vector2 vecAnchorOffset = GetUIPositionLeftTop(m_pContentTransform);
        vecAnchorOffset.x = Mathf.Abs(vecAnchorOffset.x) + m_vecSpringOffset.x;
        vecAnchorOffset.y = Mathf.Abs(vecAnchorOffset.y) + m_vecSpringOffset.y;

        for (int i = 0; i < iChildTotal; i++)
        {
            CUIWidgetTemplateItemBase pItem = m_pScrollRect.content.GetChild(i).GetComponent<CUIWidgetTemplateItemBase>();
            if (pItem != null)
            {
                OnUIScrollAnchorUpdatePosition(pItem, vecAnchorOffset);
            }
        }
    }

	//-----------------------------------------------------------	
	private void HandleUIScrollRatioChange(Vector2 vecChangeValue)
	{
        if (AnchorSpring)
        {
            PrivUIScrollAnchorSpringUpdateOffset();
        }

        OnUIScrollValueChange(vecChangeValue);
	}
    //-----------------------------------------------------------
    public virtual CUIWidgetTemplateItemBase GetUIScrollItemByViewportOffset(Vector2 vecViewportOffset) { return null; }
    //-----------------------------------------------------------
    protected virtual void OnUIScrollValueChange(Vector2 vecScrollRatio) { }
    protected virtual void OnUIScrollUpdate() { }
    protected virtual void OnUIScrollAnchorRemainOffsetX(CUIWidgetTemplateItemBase pItem, float fRemainOffsetX) { }
    protected virtual void OnUIScrollAnchorRemainOffsetY(CUIWidgetTemplateItemBase pItem, float fRemainOffsetY) { }
    protected virtual void OnUIScrollAnchorUpdatePosition(CUIWidgetTemplateItemBase pItem, Vector2 vecAnchorPosition) { }
    protected virtual void OnUIScrollAnchorSpringEnd() { }   
}
