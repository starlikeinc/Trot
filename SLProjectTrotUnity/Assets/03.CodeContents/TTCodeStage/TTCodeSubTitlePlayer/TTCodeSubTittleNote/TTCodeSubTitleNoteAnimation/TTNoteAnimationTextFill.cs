using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTNoteAnimationTextFill : TTNoteAnimationBase
{
	[SerializeField]
	private float FadeValue;
	[SerializeField]
	private AnimationCurve FadeCurve;

	[SerializeField]
	private SpriteRenderer TextRenderer;
	[SerializeField]
	private string PropertyName = "_DirectionalGlowFadeFade";
}
