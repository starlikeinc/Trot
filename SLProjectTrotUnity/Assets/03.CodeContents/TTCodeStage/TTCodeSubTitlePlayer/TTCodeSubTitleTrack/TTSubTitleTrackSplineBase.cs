using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
public abstract class TTSubTitleTrackSplineBase : TTSubTitleTrackBase
{
	[System.Serializable]
	public class SSplineSection
	{
		public float TimeLength;

	}


	[SerializeField]
	private SplineContainer SplineTrack = null;


	//----------------------------------------------------------------------------

}
