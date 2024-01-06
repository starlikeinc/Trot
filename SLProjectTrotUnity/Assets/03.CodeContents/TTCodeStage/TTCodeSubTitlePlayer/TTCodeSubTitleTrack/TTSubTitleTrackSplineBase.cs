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
		public float TrackLength;
		public float PlayDuration;
		public AnimationCurve MoveCurve;
		[HideInInspector]
		public float TimeStart;
		[HideInInspector]
		public float LengthStart;
	}

	[SerializeField]
	private SplineContainer SplineTrack = null;

	private float m_fTrackLength = 0;

	[SerializeField]
	private List<SSplineSection> SplineSection = null;
	//----------------------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		float fPrevTime = 0;
		float fPrevLength = 0;
		for (int i = 0; i < SplineSection.Count; i++)
		{
			SplineSection[i].TimeStart = fPrevTime;
			SplineSection[i].LengthStart = fPrevLength;

			fPrevTime = SplineSection[i].PlayDuration;
			fPrevLength = SplineSection[i].TrackLength;
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
		if (fTrackTime >= 10f)
		{
			 
		}	

		float fTrackSplineRate = pSplineSection.TrackLength / m_fTrackLength; // 이번 섹션에서 진행할 스플라인의 비율 
		float fSectionTimeRate = (fTrackTime - pSplineSection.TimeStart) / pSplineSection.PlayDuration;
		float fCurveRate = pSplineSection.MoveCurve.Evaluate(fSectionTimeRate);
		float fSplineLength = fCurveRate * fTrackSplineRate + (pSplineSection.LengthStart / m_fTrackLength); 

		Vector3 vecSplinePosition = SplineTrack.EvaluatePosition(fSplineLength);
		OnSubTitleTrackSplinePosition(vecSplinePosition);
	}

	private SSplineSection FindTrackSplineSection(float fTrackTime)
	{
		SSplineSection pFindSection = null;

		float fPrevValue = 0;
		for (int i = 0; i < SplineSection.Count; i++)
		{
			SSplineSection pSection = SplineSection[i];
			if (fTrackTime >= pSection.TimeStart && fTrackTime < (fPrevValue + pSection.PlayDuration))
			{
				pFindSection = pSection;
				break;
			}

			fPrevValue = pSection.PlayDuration;
		}
		return pFindSection;
	}
	//----------------------------------------------------------------------------
	protected virtual void OnSubTitleTrackSplinePosition(Vector3 vecPosition) { }
}
