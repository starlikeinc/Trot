using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public abstract class TTSubTitleTrackSplineBase : TTSubTitleTrackBase
{
	[System.Serializable]
	public class SSplineSection
	{
		public float			TrackLength;
		public float			TrackDuration;
		public AnimationCurve   TrackCurve;
		[HideInInspector]
		public float TrackPrevDuration;
		[HideInInspector]
		public float TrackPrevLength;
	}

	[SerializeField]
	private SplineContainer		SplineTrack = null;

	private float m_fTrackLength = 0;
	private float m_fSplinePerSecond = 0;
	private float m_fSplineLength = 0;
	

	[SerializeField]
	private List<SSplineSection> SplineSection = null;
	//----------------------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		float fPrevDuration = 0;
		float fPrevLength = 0;
		for (int i = 0; i < SplineSection.Count; i++)
		{
			SplineSection[i].TrackPrevDuration = fPrevDuration;
			SplineSection[i].TrackPrevLength = fPrevLength;
			fPrevDuration += SplineSection[i].TrackDuration;
			fPrevLength += SplineSection[i].TrackLength;
		}
	}

	protected override void OnSubTitleTrackStart(float fTrackLength)
	{
		m_fTrackLength = fTrackLength;
		m_fSplinePerSecond = 1f / fTrackLength;
		m_fSplineLength = SplineTrack.Spline.GetLength();
	}
	//-----------------------------------------------------------------------------
	protected sealed override void OnSubTitleTrackUpdate(float fTrackTime, float fTrackDelta)
	{
	//	fTrackTime = 10.1f;

		SSplineSection pSection = FindTrackSplineSection(fTrackTime);
		if (pSection != null)
		{
			PrivTrackSplineRefresh(pSection, fTrackTime, fTrackDelta);
		}
	}


	//----------------------------------------------------------------------------
	private void PrivTrackSplineRefresh(SSplineSection pSplineSection, float fTrackTime, float fTrackDelta)
	{
		float fSplineRateStartLength = pSplineSection.TrackPrevLength / m_fSplineLength;
		float fTrackPerSecond = pSplineSection.TrackLength / pSplineSection.TrackDuration;
		float fTrackLengthRate = (fTrackTime - pSplineSection.TrackPrevDuration) / pSplineSection.TrackDuration;
		float fCurveValue = pSplineSection.TrackCurve.Evaluate(fTrackLengthRate);
		float fTrackPosition = fCurveValue * fTrackPerSecond * pSplineSection.TrackDuration;
		float fSplineRate =  (fTrackPosition / m_fSplineLength) + fSplineRateStartLength;

		
		Vector3 vecSplinePosition = SplineTrack.EvaluatePosition(fSplineRate);
		Quaternion rQuat = ExtractTrackSplineRotation(fSplineRate);
		OnSubTitleTrackSplinePositionAndRotation(vecSplinePosition, rQuat);
	}

	private SSplineSection FindTrackSplineSection(float fTrackTime)
	{
		SSplineSection pFindSection = null;	
		for (int i = 0; i < SplineSection.Count; i++)
		{
			SSplineSection pSection = SplineSection[i];
			if (fTrackTime >= pSection.TrackPrevDuration && fTrackTime < (pSection.TrackPrevDuration + pSection.TrackDuration))
			{
				pFindSection = pSection;
				break;
			}		
		}
		return pFindSection;
	}
	//-----------------------------------------------------------------------------
	private Quaternion ExtractTrackSplineRotation(float fSplineRate)
	{
		Quaternion rQuat = Quaternion.identity;
		Vector3 vecForward = Vector3.Normalize(SplineTrack.EvaluateTangent(fSplineRate));
		Vector3 vecUp = SplineTrack.EvaluateUpVector(fSplineRate);
		rQuat = Quaternion.LookRotation(vecForward, vecUp);

		return rQuat;
	}

	//----------------------------------------------------------------------------
	protected virtual void OnSubTitleTrackSplinePositionAndRotation(Vector3 vecPosition, Quaternion rQuat) { }
}
