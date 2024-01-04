using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTEffectLaserSlider : CEffectParticleLaserBase
{
	[SerializeField]
	private float SliderLength = 10;

	//------------------------------------------------------------
	protected override void OnUnityStart()
	{
		base.OnUnityStart();
		DoEffectStart();
		SetEffectLaserLength(SliderLength);
	}
}
