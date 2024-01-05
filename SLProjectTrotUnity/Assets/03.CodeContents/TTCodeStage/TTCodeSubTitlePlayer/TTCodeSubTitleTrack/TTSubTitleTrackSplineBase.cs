using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using uTools;

public abstract class TTSubTitleTrackSplineBase : TTSubTitleTrackBase
{
	[System.Serializable]
	public class SSplineSection
	{
		public float TimeLength;
		public float Duration;
		public uTweenValue TweenValue;
		[HideInInspector]
		public float TimeStart;
	}

	[SerializeField]
	private SplineContainer SplineTrack = null;

	private float m_fTrackLength = 0;

	[SerializeField]
	private List<SSplineSection> SplineSection = null;
	//----------------------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		float fPrevValue = 0;
		for (int i = 0; i < SplineSection.Count; i++)
		{
			SplineSection[i].TimeStart = fPrevValue;
			fPrevValue = SplineSection[i].TimeLength;
		}
	}

	protected override void OnSubTitleTrackStart(float fTrackLength)
	{
		m_fTrackLength = fTrackLength;
	}
	//-----------------------------------------------------------------------------
	protected sealed override void OnSubTitleTrackUpdate(float fTrackTime)
	{
		SSplineSection pSection = FindTrackSplineSection(fTrackTime);
		if (pSection != null)
		{
			PrivTrackSplineRefresh(pSection, fTrackTime);
		}
	}


	//----------------------------------------------------------------------------
	private void PrivTrackSplineRefresh(SSplineSection pSplineSection, float fTrackTime)
	{
		if (fTrackTime >= 14f)
		{
			int Temp = 0;
		}

		float fSectionTime = fTrackTime - pSplineSection.TimeStart;
		pSplineSection.TweenValue.from = pSplineSection.TimeStart;
		pSplineSection.TweenValue.to = pSplineSection.TimeStart + pSplineSection.TimeLength;
		pSplineSection.TweenValue.duration = pSplineSection.Duration;

		pSplineSection.TweenValue.SampleFromTime(fSectionTime, false);
		float fTrackPositionRate = pSplineSection.TweenValue.value / m_fTrackLength;
		Vector3 vecPosition = SplineTrack.EvaluatePosition(fTrackPositionRate);

		OnSubTitleTrackSplinePosition(vecPosition);
	}

	private SSplineSection FindTrackSplineSection(float fTrackTime)
	{
		SSplineSection pFindSection = null;
		for (int i = 0; i < SplineSection.Count; i++)
		{
			SSplineSection pSection = SplineSection[i];
			if (fTrackTime >= pSection.TimeStart && fTrackTime < pSection.TimeLength)
			{
				pFindSection = pSection;
				break;
			}
		}
		return pFindSection;
	}
	//----------------------------------------------------------------------------
	protected virtual void OnSubTitleTrackSplinePosition(Vector3 vecPosition) { }
}
