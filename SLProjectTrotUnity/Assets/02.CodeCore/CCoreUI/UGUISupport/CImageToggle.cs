using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
// 크기와 형태가 동일한 이미지의 토글 

public class CImageToggle : CImage
{
    [SerializeField]
    private Sprite SpriteOff = null;

    private Sprite m_pSpriteOn = null;
    private bool m_bToggleOn = true;
    //----------------------------------------------------------------------------
    protected override void Awake()
    {
        base.Awake();
        m_pSpriteOn = this.sprite;
    }

    //-----------------------------------------------------------------------------
    public void DoImageToggle(bool bOn)
    {
        PrivImageToggle(bOn);
    }

    public void DoImageToggle()
    {
        PrivImageToggle(!m_bToggleOn);
    }

    //--------------------------------------------------------------------------
    private void PrivImageToggle(bool bOn)
    {
        m_bToggleOn = bOn;
        if (m_bToggleOn)
        {
            sprite = m_pSpriteOn;
        }
        else
        {
            sprite = SpriteOff;
        }
    }

} 
