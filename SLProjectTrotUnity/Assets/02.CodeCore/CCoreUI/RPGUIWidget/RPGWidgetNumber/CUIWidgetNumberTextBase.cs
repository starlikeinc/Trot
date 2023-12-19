using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
[RequireComponent(typeof(CText))]
public abstract class CUIWidgetNumberTextBase : CUIWidgetNumberBase
{
	public enum ENumberTextType
    {
		None,
		Comma,
		Percent,
    }

	private const int c_CommaCount = 3;
	private const double c_PercentResolution = 100f; //100%
	[SerializeField]
	private ENumberTextType TextType = ENumberTextType.None;
	
	protected CText m_pText = null;	
	protected StringBuilder m_pStringBuilder = new StringBuilder(64);
	//-------------------------------------------------------
	protected override void OnUIWidgetInitialize(CUIFrameBase pParentFrame)
	{
		base.OnUIWidgetInitialize(pParentFrame);
		m_pText = GetComponent<CText>();
	}
	protected override void OnUIWidgetNumber(long iNumber, bool bForce, List<int> pListDigit)
	{
		if (TextType == ENumberTextType.None)
		{
			m_pText.text = iNumber.ToString();
		}
		else if (TextType == ENumberTextType.Comma)
		{
			PrivNumberTextComma(pListDigit);
		}
		else if (TextType == ENumberTextType.Percent)
        {
			PrivNumberTextPercent(iNumber);
		}
	}

	public void SetTextColor(Color rColor)
	{
		m_pText.color = rColor;
	}
	//---------------------------------------------------------
	private void PrivNumberTextComma(List<int> pListDigit)
	{
		m_pStringBuilder.Clear();

		int iCount = c_CommaCount - (pListDigit.Count % c_CommaCount);
		for (int i = 0; i < pListDigit.Count; i++)
		{
			if (iCount++ % c_CommaCount == 0 && i != 0)
			{
				m_pStringBuilder.Append(',');
			}
			m_pStringBuilder.Append(pListDigit[i]);
		}
	
		m_pText.text = m_pStringBuilder.ToString();
	}

    private void PrivNumberTextPercent(long iNumber)
    {
        double fValue = iNumber / c_PercentResolution;
        m_pText.text = string.Format("{0:0.#}%", fValue);
    }
}
