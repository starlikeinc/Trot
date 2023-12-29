using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CUIWidgetModelingSlotBase : CUIWidgetBase
{
    private CUIModelingItemBase m_pModelingItem = null;
    //-------------------------------------------------------------
    protected override void OnUIWidgetInitialize(CUIFrameBase pParentFrame)
    {
        m_pModelingItem = GetComponentInChildOneDepth<CUIModelingItemBase>();
    }

   
}
