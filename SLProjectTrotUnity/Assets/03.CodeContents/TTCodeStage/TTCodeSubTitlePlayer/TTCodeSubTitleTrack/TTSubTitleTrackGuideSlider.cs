using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSubTitleTrackGuideSlider : TTSubTitleTrackSplineBase
{
	[SerializeField]
	private GameObject EffectThumb = null;

	//----------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		base.OnUnityAwake();
		EffectThumb.SetActive(false);
	}

	protected override void OnSubTitleTrackStart(float fTrackLength)
	{
		base.OnSubTitleTrackStart(fTrackLength);
		EffectThumb.SetActive(true);
	}

	protected override void OnSubTitleTrackSplinePosition(Vector3 vecPosition)
	{
		base.OnSubTitleTrackSplinePosition(vecPosition);
		EffectThumb.transform.position = vecPosition;
	}
}
