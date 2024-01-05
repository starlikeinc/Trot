using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public abstract class TTStagePlayMovieBase : TTStagePlayBase
{
	[SerializeField]
	private VideoPlayer MoviePlayer;

	[SerializeField]
	private MeshRenderer MeshDrawer;

	private bool m_bMoviePlayerPlaying = false;
	//-------------------------------------------------------------------
	protected override void OnUnityAwake()
	{
		base.OnUnityAwake();
		MeshDrawer.sortingLayerID = SortingLayer.NameToID(LayerMask.LayerToName(gameObject.layer));
		MeshDrawer.sortingOrder = 100;		
	}


	protected override void OnStageLoad(uint hLoadID, UnityAction delFinish, params object[] aParams)
	{
		PrivStageMovieReset();
		PrivStageMovieLoadAsset(hLoadID, ()=> {
			UIManager.Instance.UIHide<UIFrameLoadingScreen>();
			delFinish.Invoke();
		});
	}

	protected override void OnStageReLoaded(uint hLoadID)
	{
		base.OnStageReLoaded(hLoadID);
		PrivStageMovieReset();
	}

	protected override void OnStageStart(params object[] aParams)
	{
		if (m_bMoviePlayerPlaying && MoviePlayer.clip == null)
		{
			//Error!
			return;
		}

		float fBoardTimeStart = 0;
		if (aParams.Length > 0)
		{
			fBoardTimeStart = (float)aParams[0];
		}

		m_bMoviePlayerPlaying = true;

		//----------------------------------------------------------------------
		MoviePlayer.Prepare();
		MoviePlayer.prepareCompleted += (VideoPlayer pVideoPlayer) =>
		{
			pVideoPlayer.time = fBoardTimeStart;
			pVideoPlayer.seekCompleted += (VideoPlayer pVideoPlayer) =>
			{
				pVideoPlayer.Play();
				pVideoPlayer.started += (VideoPlayer pVideoPlayer) =>
				{
					PrivStageMovieStart(fBoardTimeStart, MoviePlayer.clip.length);
				};
			};
		};
	}

	//-------------------------------------------------------------------
	private void PrivStageMovieLoadAsset(uint hLoadID, UnityAction delFinish)
	{
		TTManagerResourceLoader.Instance.DoMgrResourceVideoClip("TTMovieBasic_JJiniya_yeongtak", (VideoClip pVideoClip) => {
			MoviePlayer.clip = pVideoClip;
			delFinish.Invoke();
		});
	}

	private void PrivStageMovieReset()
	{
		m_bMoviePlayerPlaying = false;
	}

	private void PrivStageMovieStart(float fBoardTimeStart, double fBoardTimeLength)
	{
		ProtStagePlaySubTitleStart(fBoardTimeStart, fBoardTimeLength);
		OnStageMovieStart(fBoardTimeStart);
	}

	//------------------------------------------------------------------------
	protected virtual void OnStageMovieStart(float fBoardTime) { }
	protected virtual void OnStageMovieStop() { }
}
