using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public abstract class TTStagePlayMovieBase : TTStagePlayBase
{
	[SerializeField]
	private VideoPlayer MoviePlayer;



	private bool m_bMoviePlayerPlaying = false;
	//-------------------------------------------------------------------
	protected override void OnStageLoad(uint hLoadID, UnityAction delFinish, params object[] aParams)
	{
		base.OnStageLoad(hLoadID, delFinish, aParams);
		PrivStageMovieReset();
		PrivStageMovieLoadAsset(hLoadID, ()=> {
			UIManager.Instance.UIHide<UIFrameLoadingScreen>();
		});
	}

	protected override void OnStageReLoaded(uint hLoadID)
	{
		base.OnStageReLoaded(hLoadID);
		PrivStageMovieReset();
	}

	protected override void OnStageStart()
	{
		if (m_bMoviePlayerPlaying)
		{
			//Error!
			return;
		}

		MoviePlayer.Prepare();
		MoviePlayer.prepareCompleted += (VideoPlayer pVideoPlayer) =>
		{
			PrivStageMovieStartMovie();
		};
	}

	//-------------------------------------------------------------------
	private void PrivStageMovieLoadAsset(uint hLoadID, UnityAction delFinish)
	{
		TTManagerResourceLoader.Instance.DoMgrResourceVideoClip("TTMovieBasic_JJiniya_yeongtak", (VideoClip pVideoClip) => {

			MoviePlayer.clip = pVideoClip;
			
		});
	}

	private void PrivStageMovieReset()
	{
		m_bMoviePlayerPlaying = false;
	}

	private void PrivStageMovieStartMovie()
	{
		m_bMoviePlayerPlaying = true;
		MoviePlayer.Play();
		OnStageMovieStart(MoviePlayer.length);
	}

	//------------------------------------------------------------------------
	protected virtual void OnStageMovieStart(double fLength) { }
	protected virtual void OnStageMovieStop() { }
}
