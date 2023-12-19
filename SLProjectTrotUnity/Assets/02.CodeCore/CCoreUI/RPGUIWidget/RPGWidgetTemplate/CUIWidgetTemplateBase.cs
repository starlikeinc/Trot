using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public abstract class CUIWidgetTemplateBase : CUIEntryBase
{
	[SerializeField]
	private CUIWidgetTemplateItemBase		TemplateItem = null;

	private CUIEntryBase					m_pTemplateOwner = null;
	private List<CUIWidgetTemplateItemBase> m_listCloneInstance = new List<CUIWidgetTemplateItemBase>(); protected List<CUIWidgetTemplateItemBase> GetWidgetTemplateList() { return m_listCloneInstance; }
	//-----------------------------------------------------
	protected override void OnUnityAwake()
	{
		base.OnUnityAwake();
		if (TemplateItem != null)
		{
			TemplateItem.SetMonoActive(false);
		}
	}

    protected override void OnUIWidgetInitialize(CUIFrameBase pParentFrame)
    {
        base.OnUIWidgetInitialize(pParentFrame);
    }

    //------------------------------------------------------
	public TEMPLATE DoUITemplateRequestItem<TEMPLATE>(Transform pParent = null) where TEMPLATE : CUIWidgetTemplateItemBase
    {
		return DoUITemplateRequestItem(pParent) as TEMPLATE;
	}


	public CUIWidgetTemplateItemBase DoUITemplateRequestItem(Transform pParent = null)
	{
		CUIWidgetTemplateItemBase pItem = null;

		for (int i = 0; i < m_listCloneInstance.Count; i++)
		{
			if (m_listCloneInstance[i].gameObject.activeSelf == false)
			{
				pItem = m_listCloneInstance[i];
				break;
			}
		}

		if (pItem == null)
		{
			pItem = MakeUITemplateItem();
		}

		if (pParent)
		{
			pItem.transform.SetParent(pParent, false);
		}
        else
        {
            pItem.transform.SetParent(transform, false);
        }

		pItem.DoUITemplateItemShow(true);

		if (m_pTemplateOwner)
		{
			pItem.InterUIWidgetOwner(m_pTemplateOwner);
		}

		OnUITemplateItem(pItem);

		return pItem;
	}

	public void DoUITemplateReturnAll()
	{
		for (int i = 0; i < m_listCloneInstance.Count; i++)
		{
			PrivUITemplateReturn(m_listCloneInstance[i]);
		}
	}

	public void DoUITemplateReturn(CUIWidgetTemplateItemBase pItem) // 비용이 상당하므로 업데이트 호출 주의
	{
		PrivUITemplateReturn(pItem);
	}
	//--------------------------------------------------------
	private CUIWidgetTemplateItemBase MakeUITemplateItem()
	{
		CUIFrameEntryBase pParentUIFrame = GetUIWidgetParentsUIFrame() as CUIFrameEntryBase;
		if (pParentUIFrame == null)
        {
			Debug.LogErrorFormat($"[UIWidgetTemplate] ParentsUI is NULL : Call OnUIWidgetInitializePost Logic instead of OnUIWidgetInitialize logic");
			return null;
        }

		GameObject NewInstance = Instantiate(TemplateItem.gameObject);
		CUIWidgetTemplateItemBase pNewItem = NewInstance.GetComponent<CUIWidgetTemplateItemBase>();
		m_listCloneInstance.Add(pNewItem);
		
		List<CUIWidgetBase> listWidget = new List<CUIWidgetBase>();
		NewInstance.GetComponentsInChildren(true, listWidget);
		for (int i = 0; i < listWidget.Count; i++)
		{
            listWidget[i].InterUIWidgetOwner(this);
			listWidget[i].InterUIWidgetInitialize(pParentUIFrame);
            pParentUIFrame.InterUIWidgetAdd(listWidget[i]);
		} 

		for (int i = 0; i < listWidget.Count; i++)
        {
            listWidget[i].InterUIWidgetInitializePost(pParentUIFrame);
        }

        return pNewItem;
	}

	private void PrivUITemplateReturn(CUIWidgetTemplateItemBase pItem)
	{
		pItem.DoUITemplateItemShow(false);
		pItem.InterUITemplateItemReturn();
		pItem.transform.SetParent(TemplateItem.transform.parent, false);
    }

	//---------------------------------------------------------------
	protected virtual void OnUITemplateItem(CUIWidgetTemplateItemBase pItem) { }
}
