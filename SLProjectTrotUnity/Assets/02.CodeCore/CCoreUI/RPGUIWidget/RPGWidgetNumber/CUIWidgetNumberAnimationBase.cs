using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 숫자를 스프라이트 이미지로 변환해서 보여주는 기능
public abstract class CUIWidgetNumberAnimationBase : CUIWidgetNumberBase
{
    [SerializeField]
    private float Duration = 1f;        // 에니메이션 되는 전체 시간
    [SerializeField]
    private float PassSpeed = 0.04f;    // 각 숫자가 넘어가는 시간

	[SerializeField]
	private HorizontalLayoutGroup NumberRoot = null;  // ContentsSizeFitter 또한 할당되어 있어야 한다.
	[SerializeField]
	private List<Sprite> NumberSprite = null;         // 0번부터 순서대로 입력할것
    [SerializeField]
	private List<CUIWidgetNumberImageChildBase> NumberImage = null;  // 대충 자릿수 만큼 입력할 것. 100백만 = 7자리

    private long    m_iTargetValue = 0;
    private bool    m_bAnimation = false;
    private float   m_fCurrentAnimationTime = 0;
    private float   m_fCountValuePerSecond = 0;
    private float   m_fCurrentCountValue = 0;
    private int     m_iCurrentAnimationDigit = 0;  // 마지막 자리부터 에니메이션 
    //--------------------------------------------------------------------------
    protected override void OnUIWidgetInitialize(CUIFrameBase pParentFrame)
    {
        base.OnUIWidgetInitialize(pParentFrame);

		NumberRoot.GetComponentsInChildren(NumberImage);
        for (int i = 0; i < NumberImage.Count; i++)
        {
            NumberImage[i].gameObject.SetActive(false);
        }
    }

    

    //--------------------------------------------------------------------------
    protected override void OnUIWidgetNumber(long iTargetNumber, bool bForce, List<int> pListDigit) 
	{
        if (m_iTargetValue == iTargetNumber) return;

        if (bForce)
        {
            m_iTargetValue = iTargetNumber;
            OnUINumberAnimationFinish();
            PrivNumberImagePrint(pListDigit);
        }
        else
        {
            PrivNumberImageAnimationStart(iTargetNumber);
        }
	}

	//---------------------------------------------------------------------------
	private void PrivNumberImageReset()
    {
        m_fCurrentAnimationTime = 0f;
        m_fCountValuePerSecond = 0f;
    }

    private void PrivNumberImagePrint(List<int> pListDigit)
    {
        for (int i = 0; i < NumberImage.Count; i++)
        {
            if (i < pListDigit.Count)
            {
                int iDigitNumber = pListDigit[i];
                Sprite pSprite = ExtractNumberImageSprite(iDigitNumber);
                NumberImage[i].DoUIWidgetNumberImageChildOn(pSprite, iDigitNumber);
            }
            else
            {
                NumberImage[i].DoUIWidgetNumberImageChildOff();
            }
        }
    }

    private void PrivNumberImagePrint(int iDigit, int iNumber)
    {
        if (iDigit < NumberImage.Count)
        {
            CUIWidgetNumberImageChildBase pNumberImage = NumberImage[iDigit];
            pNumberImage.DoUIWidgetNumberImageChildOn(ExtractNumberImageSprite(iNumber), iNumber);
        }
    }

    private void PrivNumberImageAnimationStart(long iTargetNumber)
    {
        m_bAnimation = true;
        m_fCurrentCountValue = m_iTargetValue;
        m_iTargetValue = iTargetNumber;
        m_fCountValuePerSecond = (iTargetNumber - m_fCurrentCountValue) / Duration;


        OnUINumberAnimationStart(m_fCurrentCountValue);
    }

    private Sprite ExtractNumberImageSprite(int iNumber)
    {
        Sprite pSprite = null;
        if (iNumber < NumberSprite.Count)
        {
            pSprite = NumberSprite[iNumber];
        }

        return pSprite;
    }

    //-----------------------------------------------------------------
    protected virtual void OnUINumberAnimationFinish() { }
    protected virtual void OnUINumberAnimationStart(float fCurrentValue) { }

}
